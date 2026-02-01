using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WT.B2C.API.BuildingBlocks.Domain;

namespace WT.B2C.API.BuildingBlocks.Data.Repositories;

public class GenericRepository<T>(DbContext dbContext)
    : GenericRepository<T, long>(dbContext), IGenericRepository<T> where T : Entity<long>
{
}

public class GenericRepository<T, Tkey>(DbContext dbContext)
    : IGenericRepository<T, Tkey> where T : Entity<Tkey>
{
    public virtual void Add(T entity)
    {
        dbContext.Add(entity);
    }

    public async Task<T?> GetById(Tkey id)
    {
        return await dbContext.Set<T>().FindAsync(id);
    }

    public async Task<T?> Get(Expression<Func<T, bool>> predicate)
    {
        return await dbContext.Set<T>().FirstOrDefaultAsync(predicate);
    }

    public async Task<T> GetById(Tkey id, string[] includes)
    {
        var dbset = dbContext.Set<T>();
        foreach (string include in includes)
        {
            dbset.Include(include);
        }

        return await dbset.FirstOrDefaultAsync(entity => entity.Id.Equals(id));
    }
    
    public async Task<bool> Exists(Tkey id)
    {
        return await dbContext.Set<T>().AnyAsync(entity => entity.Id.Equals(id));
    }

    public void Attach(T entity)
    {
        dbContext.Attach(entity);
    }

    public async Task<bool> Remove(Tkey id)
    {
        var entity = await dbContext.Set<T>().FindAsync(id);
        if (entity != null)
        {
            dbContext.Remove(entity);
            return true;
        }

        return false;
    }

    public async Task<bool> RemoveAll(Expression<Func<T, bool>> predicate)
    {
        var entities = await dbContext.Set<T>().Where(predicate).ToListAsync();
        if (entities != null)
        {
            dbContext.RemoveRange(entities);
            return true;
        }

        return false;
    }

    public void Remove(T entity)
    {
        dbContext.Remove(entity);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}