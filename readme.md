# Domain Driven Design Framework

The Domain Driven Design (DDD) framework provides the building blocks for creating a DDD bounded context. It is designed to be minimal and tries to implement the DDD concepts without getting in the way. It addresses the following DDD concerns:

### Domain Concerns

- Identity
- Entities
- Aggregates
- Repositories & Unit of Work
- Domain Commands
- Domain Events
- Domain Queries

### Application/Infrastructure Concerns

In addition to the core DDD framework some implementation specific packages are available which takes care of some of the boiler plate.

- Autofac
- MediatR
- Entity Framework Persistence
- Dapper Queries (TODO)
- Messaging (TODO)

The remainder of this guide will provide details on each of the above sections.

## Identity

Identities in a DDD system need to be strongly typed in order to be passed around as first class objects. All entities within the system must have an identity. Naturally this includes aggregates since an aggregate is an entity.

The framework contains an `Identity` base class which all identities should inherit from. It would be implemented as follows:

```
public class PersonId : Identity
{
	public PersonId(string value) : base(value)
	{
			
	}
}
```

When referencing aggregates from within other aggregates it should reference the identity of that aggregate, and not the aggregate itself. For example:

```
public class Team : Aggregate<TeamId> {

	...

	public IReadOnlyCollection<PersonId> PeopleIds { get; }

	...

}
```

> A note on collections - In the sample we have chosen to name the collections by convention with the names ending in `Ids`. This is a preference which allows you to distinguish between a collection of identities and a collection of entities or value objects. You may choose to drop the `Ids` post fix and you would simply have `People` to make it more DDDish however it can create confusion down the line.

The framework does not currently deal with the creation of identity values. This is left to the persistence layer since it may be required to generate unique ids based on some persistence details.

## Entities

Entities represent objects which can be changed over time. Entities therefore require a unique identity.

Entities contain data (Properties) and behaviour (Methods) as you would expect in any object orientated system. For example, en entity could be defined as follows:

```
public class ContactId : Identity
{
	public ContactId(string value) : base(value)
	{
	}
}

public class Contact : Entity<ContactId>
{
	public TelNumber Work { get; set; }

	public Email Email { get; set; }

	public TelNumber WorkTel { get; set; }
}
```

When an aggregate has a relationship to entities it will contain a reference to the object itself and not the identity as would be the case when referencing other aggregates. This would provide a more natural way of working with the aggregate. The choice of persistence would have an impact here since your ORM would need to map and track changes to the nested entities. If this becomes a problem it is acceptable to reference entities by id in the same way you would reference aggregates.

## Aggregates

Aggregates, or aggregate roots, are objects which are at the root of a given object hierarchy. Typically they would be identified by their transaction boundaries. i.e. one transaction per aggregate.

It is good practice to keep the aggregates small, which means you would have many aggregates within any bounded context. This is a good thing since it will keep the amount of work which happens in a given transaction to a minimum.

The definition of an aggregate within the framework is similar to an entity however it provides the ability to create domain events which entities are not able to do. More details will be provide on domain events later in this guide. An aggregate would be defined as follows:

```
public class PersonId : Identity
{
	public PersonId(string value) : base(value)
	{
	}
}

public class Person : Aggregate<PersonId>
{
	private readonly List<Contact> _contacts;

	public Person()
	{
		_contacts = new List<Contact>();
	}

	public Name Name { get; set; }

	public IReadOnlyCollection<Contact> Contacts => _contacts;
}
```

## Repositories & Unit of Work

Repositories and the Unit of Work are there to allow us to decouple our domain from the persistence layer. There is one unit of work created per transaction and its job is to track aggregates through the duration of the transaction. Once the transaction is complete the unit of work will request one or more behaviours to be applied to the tracked aggregates. There will be more on that later.

Although the repository and unit of work patterns add significant complexity to the code base it offers two benefits which cannot be overlooked.

- Decoupling the domain layer form the persistence layer allows you to test the domain in isolation without getting caught up in dealing with the complexity of the persistence layer. While in a data driven system the benefits of this may not be obvious (or even possible) with a behaviour rich domain model it becomes essential.

- The unit of work allows us to use aspect orientated patterns to inject behaviour at the application layer without coupling the domain layer to the specific implementations. Logging is an obvious example which would be an important non functional requirement but *may* not be relevant within domain layer.

Each aggregate type must have its own repository. The framework provides a generic base class which makes implementing this straightforward.

The repositories should be used exclusively for adding new aggregates, deleting aggregates and retrieving aggregates via their id or a collection of id's. Querying concerns are handled by domain queries which provide the CQRS (Command Query Responsibility Segregation) capabilities. There will be more on domain queries later in this guide.

There will be persistence technology specific implementations of the repository however there is an abstract base class which can be used to create technology specific repositories. The base class hooks up the repository to the unit of work so that concern does not need to be handled by the specific implementation. An implementation my appear as follows:

```
public class MyCustomPersistenceRepository<TAggregate, TAggregateId> : Repository<TAggregate, TAggregateId>
	where TAggregate : Aggregate<TAggregateId>
	where TAggregateId : Identity
{
	public MyCustomPersistenceRepository(IDomainUnitOfWork unitOfWork) : base(unitOfWork)
	{
	}

	protected override Task<TAggregate> DoLoad(Identity id)
	{
		// custom db logic here
	}

	protected override Task<IReadOnlyDictionary<Identity, TAggregate>> DoLoad(IEnumerable<Identity> ids)
	{
		// custom db logic here
	}

	protected override Task DoAdd(TAggregate aggregate)
	{
		// custom db logic here
	}

	protected override void DoRemove(TAggregate aggregate)
	{
		// custom db logic here
	}
}
```
Technology specific implementations will be described later in this guide. The example above illustrates how you could go about implementing your own technology specific implementation of a repository. Notice how the class above is marked as abstract. This is because you would need to create a concrete instance of the repository for each type. This would appear as follows:

```
public class PersonRepository : MyCustomPersistenceRepository<Person, PersonId>
{
	public PersonRepository(IDomainUnitOfWork unitOfWork) : base(unitOfWork)
	{
	}
}
```

The concrete types would need to be registered within the IoC container so it could be resolved when required. If you use the Autofac package the repositories will be wired up automatically. More information on IoC will be provided later in this guide.

## Domain Commands

Domain commands are used to request an action to be performed by the domain. The command handlers would coordinate the interactions between repositories and the application layer. As far as possible business logic should be pushed down into the aggregate and not implemented within the handler. Where this is not possible a domain service should be built which will help encapsulate the necessary business logic within the domain.

The domain commands themselves are defined within the domain. There are two flavours of domain commands, ones which don't return anything and others which do. CQRS purists would argue that commands should not return a value. This is not a pure CQRS framework and we have purposefully chosen to ignore that guidance to remove some complexity introduced by the eventual consistency paradigm.

A domain command would be fined as follows:

```
public class CreatePersonDomainCommand : IDomainCommand<bool>
{
	public string Name { get; set; }
}
```

Here we have created a command which asks the domain to create a person and return whether it was successful or not.

### Dispatching Commands

The framework provides two interfaces to dispatch commands. The first would use the `IDomainCommandDispatcher` service directly. This is the preferred approach but will require you to be working within a class which was created using the IoC container. It is sometimes helpful to dispatch commands (and queries) from within an aggregate. Since the aggregate does not have access to the IoC container a static interface is provide via the `DomainCommandDispatcher.Send` static method. The `DomainCommandDispatcher` is wired up to the correct IoC scope. More details will be provided on this later on in the guide.

Typically the domain commands will be sent from some external facing api. For example it could be sent from within a WebAPI controller class and would appear as follows:

```
public PersonController : ApiController 
{
	private readonly IDomainCommandDispatcher _domainCommandDispatcher;

	public PersonController(IDomainCommandDispatcher domainCommandDispatcher) 
	{
		_domainCommandDispatcher = domainCommandDispatcher;
	}

	...

	public async Task Post(PersonData data) 
	{
		var command = CreateCommand(data)
		await _domainCommandDispatcher.Dispatch(command)
	}

	...
}

```

### Handling Commands

A command handler could be defined in the _application layer_ or the _domain layer_ as follows:

```
public class CreatePersonDomainCommandHandler : IDomainCommandHandler<CreatePersonDomainCommand, bool>
{
	private readonly IRepository<Person> _people;

	public CreatePersonDomainCommandHandler(IRepository<Person> people)
	{
		_people = people;
	}

	public async Task<bool> Handle(CreatePersonDomainCommand request, CancellationToken cancellationToken)
	{
		if (request.Name == null)
		{
			return false;
		}

		var person = new Person(request.Name);
		await _people.Add(person);
		return true;
	}
}
```

> Important - Note how we are coding against the IRepository<> interface. This is essential to enable testing in isolation.

> Note - Clearly the validation logic shown here does not represent a very useful validation implementation. A more robust implementation would be put in place and would be up to the implementation of the specific bounded context on how to best handle this feature.

> Note - The command handler may request multiple repositories and/or application services. Remember it's job is to coordinate interactions between the application and domain layers. If the handler requests application layer services then the handler itself would need to be defined within the application layer. If not then you would be pulling application layer dependencies into the domain which should be a punishable offence :/

## Domain Events

The technical implementation of Domain Events is very similar to Domain Commands described above however there are some fundamental logical differences.

- Domain Events operate using a pub/sub model. This means that there could be 0 or more subscribers for any given domain event. Because of this there can be no return value from a domain event, its purely there to indicate to interested parties that something interesting happened within the domain.

- Dispatching domain events should as far as possible only occur within an aggregate. There are rare cases where there domain event itself is an aggregate since it may not belong to another aggregate.

A domain event would be defined as follows:

```
public class NameChangedDomainEvent : IDomainEvent
{
	public NameChangedDomainEvent(PersonId personId, Name name)
	{
		PersonId = personId;
		Name = name;
	}

	public PersonId PersonId { get; }

	public Name Name { get; }
}
```

This domain event would be published by the Person aggregate to let other areas of the system know that this persons name has changed. It would be up to you to determine the granularity of the domain events since it would depend on the specific requirement.

Generally the guidance on domain events is that it should contain just enough data to express what has happened. It would be up to the handlers to request additional data should it be required to handle the event. Recall that the domain event would be published with in the same unit of work and as such you would not incur additional costs of loading aggregates in the handler since they would already exist in the in memory session. No additional remote calls would need to be made.

### Dispatching Domain Events

Domain events would be dispatched from within the aggregate as follows:

```
public class Person : Aggregate<PersonId>
{
	...

	public void ChangeName(Name name)
	{
		Name = name;
		AddDomainEvent(new NameChangedDomainEvent(Id, Name));
	}
}
```

There are some important things to note here:

- The behaviour is explicitly defined on the aggregate. You would not simply change the name with an accessor but rather explicitly define a method which describes what is being done.
- Once the action has been performed the aggregate creates a domain event which will let the rest of the system know that this has happened.
- The event is not immediately dispatched but rather _queued_ up within the aggregate. Just before the database transaction is committed the unit of work will physically dispatch the events to their subscribers. Any additional work which done within the domain event handlers would take place within the same transaction.

> Note - There is a train of thought which says that the work performed by the handlers should take place in a separate transaction. We have chosen not to follow that guidance to remove some eventual consistency challenges. If this is required the unit of work can be modified to publish events after the database transaction has been committed.

### Handling Domain Events

In our sample this event will be handled at the application layer. The handler will then dispatch an email through the infrastructure layer informing the person than an update has been made on their account. The handler may appear as follows:

```
public class NameChangedDomainEventHandler : IDomainEventHandler<NameChangedDomainEvent>
{
	private readonly IRepository<Person> _people;
	private readonly IEmailDispatcherService _emailDispatcherService;

	public NameChangedDomainEventHandler(IRepository<Person> people, IEmailDispatcherService emailDispatcherService)
	{
		_people = people;
		_emailDispatcherService = emailDispatcherService;
	}

	public async Task Handle(NameChangedDomainEvent notification, CancellationToken cancellationToken)
	{
		var person = await _people.Load(notification.PersonId);

		foreach (var contact in person.Contacts)
		{
			await _emailDispatcherService.Dispatch(contact.Email, $"Dear {person.Name.Full}, your account has been modified");
		}
	}
}
```

> Note - Don't focus on the email service explicitly. This has been over simplified to illustrate a point, which is that the event handlers within the application domain can coordinate with infrastructure services. Again, this handler MUST exist in the application layer otherwise you would be pulling infrastructure dependencies into the domain layer.

## Domain Queries

Domain Queries allow us to decouple the specific implementation of a query from our domain. This is done to allow you to explicitly represent queries within the domain layer, but implement it within the application layer.

The technical process of dispatching domain queries is very similar to commands and events however their logical use if very different. Queries are used to request data from the domain.

### CQRS - Command Query Responsibility Segregation

Domain Queries provide a mechanism for us to implement a type of CQRS where the query model (reads) can be different to the command model (writes). It is important to understand that the intention of this framework is not to fully implement CQRS to the point where the command and query models exist in different data stores. An explicit decision was made to avoid this to remove eventual consistency complexity. This being said it is encouraged to model a different read model using RDBMS views as an example. 

We say that it CAN be different since your queries could return aggregates and in some cases this can be useful. When doing this it is important to consider the transactional boundaries and relationships between queries and repositories. YOu may want to query unique aggregate id's from the query layer then load up the aggregates with the list of id's. If performance becomes a concern with this approach then additional work would need to be performed to consolidate the command and query worlds.

### Defining a Query

A query can be defined as follows:

```    
public class FindPeopleByName : IDomainQuery<PagedCollection<PersonByName>>
{
	public string Term { get; set; }
}

```

In the above the example the query is called `FindPeopleByName` and returns a `PagedCollection<>` for `PersonByName` objects. Note we have created an explicit contract here to represent the query results (hence CQRS). It would be acceptable to share contracts between queries where it makes sense.

### Dispatching a Query

Similar to dispatching a command, a query can be dispatched from one of two interfaces. Where you can inject dependencies you would make use of the `IDomainQueryDispatcher` service. Where IoC is not possible, inside an aggregate for instance, you can make use of the static `DomainQueryDispatcher`.

Example of dispatching a query from an api controller:

```
public PersonController : ApiController 
{
	private readonly IDomainQueryDispatcher _domainQueryDispatcher;

	public PersonController(IDomainQueryDispatcher domainQueryDispatcher) 
	{
		_domainQueryDispatcher = domainQueryDispatcher;
	}

	...

	public async Task<PersonDto> Search(string term) 
	{
		var query = new FindPeopleByName {Term = Name.Full};
		var results = await _domainQueryDispatcher.Dispatch(query);
		return CreateDtos(results)
	}

	...
}

```

And a contrived example of what it would look like within an aggregate:

```
public class Person : Aggregate<PersonId>
{
	...

	public Task<PagedCollection<PersonByName>> FindOtherPeopleWithTheSameName()
	{
		var query = new FindPeopleByName {Term = Name.Full};
		return DomainQueryDispatcher.Execute(query);
	}
}
```

### Handling a Query

The purpose of the query handler is to implement the data access code within the application layer. The actual implementation would depend on the persistence mechanism being used (e.g. sql) and method/framework which makes the most sense within your particular bounded context. The example below shows a boiler plate query handler implementation and emits the actual implementation. Typically when working with SQL you could inject a IDbConnection and use Dapper to map the result.

```
public class FindPeopleByNameQueryHandler : IDomainQueryHandler<FindPeopleByName, PagedCollection<PersonByName>>
{
	public Task<PagedCollection<PersonByName>> Handle(FindPeopleByName request, CancellationToken cancellationToken)
	{
		// implementation goes here
	}
}
```

## IoC

IoC is used to bind all the components together. Out of the box an Autofac implementation is available. Autofac was chosen for the following reasons:

- Active community and one of the more popular containers.
- Unity was another consideration however the project has been scaled on citing that existing IoC projects are mature enough not to warrant a IoC container being developed by Microsoft, with Autofac being mentioned as one of the recommended replacements.

The framework can be setup using the static `AutofacConfiguration` class defined in `Ddd.Autofac`.

```
var assembliesToScan = new List<Assembly> {typeof(Person).GetTypeInfo().Assembly};

var container = AutofacConfiguration.CreateContainer(assembliesToScan, builder =>
{
		// configure custom bindings here
});

return container;
```

You need to provide the `Create` method with one or more assemblies to scan. These assemblies are the ones which contain your command handlers, query handlers, event handlers and repositories. The static factory method will create the autofac bindings using reflection on those assemblies.

There is a callback which you can implement which will give you an opportunity to register additional services with the autofac `ContainerBuilder`.

The Autofac configuration will also wire up the static `DomainCommandDispatcher` and `DomainQueryDispatcher`. It will manage child scopes to ensure that dependencies are resolved within the correct scope.

Once a container has been created it will be up to you to wire it into the application framework being used. For example WebAPI or an NServiceBus endpoint.

## MediatR

MediatR is the only dependency of the domain framework. It was used because it is a simple and mature mediator pattern implementation which works well with Autofac. It would be possible to remove this dependency by implementing the mediator pattern ourselves. It would not be worth the effort until MediatR gets in out way for whatever reason.

The mediator pattern is used to implement the command, event and query dispatchers. It provides loose coupling with synchronous dispatch using the IoC container to resolve the handlers. The specific implementation can be seen in `DomainDispatcher` which implements `IDomainCommandDispatcher`, `IDomainQueryDispatcher` and `IDomainEventDispatcher`.

```
public class DomainDispatcher : IDomainCommandDispatcher, IDomainQueryDispatcher, IDomainEventDispatcher
{
	private readonly IMediator _mediator;

	public DomainDispatcher(IMediator mediator)
	{
		_mediator = mediator;
	}

	public Task Dispatch(IDomainCommand command)
	{
		return _mediator.Send(command);
	}

	public Task<TResponse> Dispatch<TResponse>(IDomainCommand<TResponse> command)
	{
		return _mediator.Send(command);
	}

	public Task<TResponse> Dispatch<TResponse>(IDomainQuery<TResponse> query)
	{
		return _mediator.Send(query);
	}

	public async Task Dispatch(IPublishDomainEvents entity)
	{
		foreach (var domainEvent in entity.FlushDomainEvents())
		{
			await _mediator.Publish(domainEvent).ConfigureAwait(false);
		}
	}
}
```