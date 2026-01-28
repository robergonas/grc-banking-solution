using GRC.BuildingBlocks.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GRC.BuildingBlocks.Infrastructure.Data;

public abstract class Repository<T> where T : Entity, IAggregateRoot
{
    protected readonly DbContext Context;
    protected readonly DbSet<T> DbSet;
    public IUnitOfWork UnitOfWork => Context as IUnitOfWork;
    protected Repository(DbContext context, DbSet<T> DbSet)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        DbSet = Context.Set<T>();
    }
    public virtual async Task<T> GetByIdAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
    }
    public virtual async Task<IEnumerable> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }
    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.Where(predicate).ToListAsync();
    }
    public virtual T Add(T entity)
    {
        return DbSet.Add(entity).Entity;
    }
    public virtual void Update(T entity)
    {
        Context.Entry(entity).State = EntityState.Modified;
    }
    public virtual void Remove(T entity)
    {
        DbSet.Remove(entity);
    }
    public virtual async Task<bool> ExistsAsync(Guid id)
    {
        return await DbSet.AnyAsync(e => e.Id.Equals(id));
    }
    public virtual async Task<int> CountAsync()
    {
        return await DbSet.CountAsync();
    }
    public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.CountAsync(predicate);
    }
}