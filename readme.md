# Domain Driven Design Framework

The Domain Driven Design (DDD) framework provides some building blocks for creating a DDD bounded context. It addresses the following DDD concerns:

### Domain Concerns

- Identity
- Entities
- Aggregates
- Repositories & Unit of Work
- Domain Commands
- Domain Events
- Domain Queries

### Infrastructure Concerns

- Entity Framework Persistence
- Dapper Queries (TODO)
- Messaging (TODO)


The remainder of this guide will provide details on each of the above sections.

## Identity

Identities in a DDD system need to be strongly typed so they can be passed around as first class objects. All entities within the system need to be have an identity, this includes aggregates since aggregates are entities. 

The framework contains an `Identity` base class which all identities should inherit from. It would be implemented as follows:

```
public class PersonId : Identity
{
		public PersonId(string value) : base(value)
		{
				
		}
}
```

When referencing aggregates from other aggregates or entities it should reference the identity of that aggregate and not the aggregate itself. For example:

```
public class Team : Aggregate<TeamId> {

	...

	public IReadOnlyCollection<PersonId> PeopleIds { get; }

	...

}
```

> A note on collections - In the sample we have chosen to name the collections of aggregates with a name ending in `Ids`. This is a preference which allows you to distinguish quickly when a collection is a collection of identities or objects. You may choose to drop the `Ids` post fix and you would simply have `People` to make it more DDDish however it can create confusion down the line.

The framework does not currently deal with the creation of identities. This is left to the persistence layer since it may be required to generate unique ids based on some persistence details.

## Entities

Entities represent objects which can be changed over time. Due to this property they require an identity so they can be uniquely identified and updates/deleted as required.

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

When an entity is referenced by an aggregate it is possible for the entity object to be referenced. This would provide a more natural way of working with the aggregate. The choice of persistence would have an impact here since your ORM would need to map and track changes to the nested entities. If this becomes a problem it is acceptable to reference entities by id in the same way you would reference aggregates.

## Aggregates

Aggregates, or aggregate roots, are objects which are at the root of a given object hierarchy. Typically they would be identified by their transaction boundaries. i.e. Typically you would want to update a single aggregate within a transaction.

It is good practice to keep the aggregates small, which will mean you would have many aggregates within any bounded context. This is a good thing since it will keep the amount of work which happens in a given transaction to a minimum.

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

Repositories and the Unit of Work are here to allow us to decouple our domain from the persistence layer. Although this adds significant complexity to the code base it offers a great number of benefits which cannot be overlooked.

- Decoupling the domain layer form the persistence layers allows you to test the domain in isolation without getting caught up in handling persistence complexity. While in a data driven system the benefits of this may not be obvious (or even possible) when we build out a behaviour rich domain model it becomes essential.

- The unit of work allows us to use aspect orientated patterns to inject behaviour in the application layer without needing the domain to be aware of them. Logging is an obvious example which is crucial but *may* not be part of the domain layer.

Each aggregate type must have its own repository. The framework provides a generic base class which makes implementing this straight forward.

The repositories should be used exclusively for adding new aggregates, deleting aggregates and retrieving aggregates via their id or a collection of id's. Querying concerns are handled by domain queries which provide the CQRS (Command Query Responsibility Segregation) capabilities. There will be more on domain queries later in this guide.

There will be persistence technology specific implementations of the repository however there is an abstract base class which should be used to create those specific repositories. The base class hooks up the repository to the unit of work so that concern does not need to be handled by the specific implementation. An implementation my appear as follows:

```
public class MyCustomPersistenceRepository<TAggregate, TAggregateId> : 		Repository<TAggregate, TAggregateId>
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
Technology specific implementations will be described later this guide. The example above illustrates how you could go about implementing your own technology specific implementation of a repository. Notice how the class above is marked as abstract. This is because you would need to create a concrete instance of the repository for each type. This would appear as follows:

```
public class PersonRepository : MyCustomPersistenceRepository<Person, PersonId>
{
	public PersonRepository(IDomainUnitOfWork unitOfWork) : base(unitOfWork)
	{
	}
}
```

The concrete types would need to be registered within the IoC container so it could be resolved when required. More information on IoC will be provided later in this guide.

## Domain Commands

Domain commands are used to request an action to be performed by the domain. The command handlers would coordinate the interactions between repositories and the application layer. As far as possible business logic should be pushed down into the aggregate and business logic should not be implemented within the handler. Where this is not possible a domain service should be built which will help encapsulate the necessary business logic within the domain.

The domain commands themselves are defined within the domain. There are two flavours of domain commands, ones which dont return anything and others which do. CQRS purists would say commands should never return anything. This is not a pure CQRS framework and we have purposefully chosen to ignore that guidance to remove some complexity introduced by the eventually consistent paradigm.

A domain command could be fined as follows:

```
public class CreatePersonDomainCommand : IDomainCommand<bool>
{
	public Name Name { get; set; }
}
```

Here we have created a command which asks the domain to create a person and return whether it was successful or not.

### Dispatching Commands

The framework provides two interfaces to dispatch commands. The first would use the `IDomainCommandDispatcher` service directly. This is the preferred approach but will require you to be working within a class which was created using the IoC container. It is sometimes helpful to dispatch commands (and queries) from within an aggregate. Since the aggregate does not have access to the IoC container a static interface is provide via the `DomainCommandDispatcher.Send` static method. The `DomainCommandDispatcher` is wired up to the correct IoC scope. More details will be provided on this later on in the guide.
