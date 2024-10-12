using System.Linq.Expressions;
using DataLayer.DbContext;
using DataLayer.Interface;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class AbstractRepository<T> : IAbstractRepository<T> where T : BaseEntity
{
    public AbstractRepository(BreweryDbContext breweryDbContext)
    {
        _breweryDbContext = breweryDbContext;
        _entities = _breweryDbContext.Set<T>();
    }

    #region Fields

    private readonly BreweryDbContext _breweryDbContext;
    private readonly DbSet<T> _entities;

    #endregion

    #region Public Methods

    public async Task<T?> GetById(int id)
    {
        var baseEntities = _breweryDbContext.Set<T>();
        return await baseEntities.FindAsync(id);
    }

    public async Task<T?> FirstOrDefault(Expression<Func<T?, bool>> predicate)
    {
        var entity = await _entities.FirstOrDefaultAsync(predicate);
        return entity;
    }

    public async Task Add(T? entity)
    {
        await _entities.AddAsync(entity ?? throw new ArgumentNullException(nameof(entity)));
        await _breweryDbContext.SaveChangesAsync();
    }

    public async Task<int> Update(T entity)
    {
        _breweryDbContext.Entry(entity).State = EntityState.Modified;
        return await _breweryDbContext.SaveChangesAsync();
    }

    public async Task<int> Remove(T? entity)
    {
        _entities.Remove(entity ?? throw new ArgumentNullException(nameof(entity)));
        return await _breweryDbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<T?>> GetAll()
    {
        var entries = await _entities.ToListAsync();
        return entries;
    }

    public async Task<IEnumerable<T?>> GetWhere(Expression<Func<T?, bool>> predicate)
    {
        return await _entities.Where(predicate).ToListAsync();
    }

    public async Task<int> CountAll()
    {
        return await _entities.CountAsync();
    }

    public async Task<int> CountWhere(Expression<Func<T?, bool>> predicate)
    {
        return await _entities.CountAsync(predicate);
    }

    #endregion
}