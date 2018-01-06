using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseBuddy.Services
{
    public interface IGenericRepository<ContextType, EntityType, KeyType> where ContextType : DbContext where EntityType : class
    {
        Task<EntityType> CreateAsync(EntityType entity);
        Task<EntityType> CreateAsync<TSourceType>(TSourceType entity);
        Task<List<EntityType>> AllAsync(
            Expression<Func<EntityType, bool>> predicate = null, 
            Expression<Func<EntityType, object>> include = null,
            Expression<Func<EntityType, object>> sortByAscending = null,
            Expression<Func<EntityType, object>> sortByDescending = null);
        Task<List<TDestination>> AllAsync<TDestination>(
            Expression<Func<EntityType, bool>> predicate = null,
            Expression<Func<EntityType, object>> include = null,
            Expression<Func<EntityType, object>> sortByAscending = null,
            Expression<Func<EntityType, object>> sortByDescending = null);
        Task<EntityType> FindByIdAsync(KeyType id);
        Task<TDestination> FindByIdAsync<TDestination>(KeyType id);
        void Update(EntityType entity);
        void Delete(EntityType entity);
        Task SaveChangesAsync();
    }
}
