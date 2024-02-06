using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Shared;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class CrudRepository<T>:ICrudRepository<T> where T : class
    {
        private readonly DbEntities _context;
        private readonly DbSet<T> _dbSet;
        private readonly IDuplicationRepository<T> _repository;
        private readonly ILogger _logger;
        public CrudRepository(DbEntities context, IDuplicationRepository<T> repository,ILogger logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
            _repository = repository;
            _logger = logger;
        }
        public async Task<T> GetById(int id) 
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting an entity by id from the database - type {typeof(T).Name}");
                throw;
            }
        }

        public async Task<T> GetLast()
        {
            try
            {
                return await _dbSet.LastAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the last entity from the database - type {typeof(T).Name}");
                throw;
            }
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting a list from the database - type {typeof(T).Name}");
                throw;
            }
        }

        public async Task<OperationResult> Add(T entity)
        {
            try
            {
                var duplicate = await _repository.IsDuplicate(entity, null);
                if (!duplicate)
                {
                    _dbSet.Add(entity);
                    await _context.SaveChangesAsync();
                    return await OperationResult.CreateAsync(true, HttpStatusCode.Created);
                }
                return await OperationResult.CreateAsync(false, HttpStatusCode.Conflict);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding an entity to the database - type {typeof(T).Name}");
                return await OperationResult.CreateAsync(false, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<OperationResult> Update(int id,T entity)
        {
            try
            {
                var duplicate = await _repository.IsDuplicate(entity, id);
                if (!duplicate)
                {
                    _dbSet.Attach(entity);
                    _context.Entry(entity).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return await OperationResult.CreateAsync(true, HttpStatusCode.Created);
                }
                return await OperationResult.CreateAsync(false, HttpStatusCode.Conflict);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating an entity in the database - type {typeof(T).Name}");
                return await OperationResult.CreateAsync(false, HttpStatusCode.InternalServerError);
            }
        }

        public async Task Remove(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while removing an entity from the database - type {typeof(T).Name}");
            }
        }
    }
}
