namespace WT.B2C.API.BuildingBlocks.Domain;

public class Entity : Entity<long>
{
}

public class Entity<T> : AuditedEntity<T>
{
}

public class AuditedEntity<T> : ICreated, ISoftDelete
{
    public T Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public long CreatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public long? DeletedBy { get; set; }
}

public class FullAuditedEntity<T> : Entity<T>, IUpdated
{
    public DateTime? UpdatedAt { get; set; }
    public long UpdatedBy { get; set; }
}

public interface ISoftDelete
{
    bool IsDeleted { get; set; }
    DateTime? DeletedAt { get; set; }
    public long? DeletedBy { get; set; }
}

public interface ICreated
{
    public DateTime CreatedAt { get; set; }
    public long CreatedBy { get; set; }
}

public interface IUpdated
{
    public DateTime? UpdatedAt { get; set; }
    public long UpdatedBy { get; set; }
}

public interface IHasCountry
{
    public string CountryCode { get; set; }
}