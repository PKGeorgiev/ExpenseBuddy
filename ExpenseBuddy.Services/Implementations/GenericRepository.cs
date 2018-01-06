using ExpenseBuddy.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Internal;

namespace ExpenseBuddy.Services.Implementations
{
    public class GenericRepository<ContextType, EntityType, KeyType> : IGenericRepository<ContextType, EntityType, KeyType> where ContextType: DbContext where EntityType : class
    {
        private readonly ContextType _ctx;

        public GenericRepository(ContextType dbContext)
        {
            _ctx = dbContext;
        }

        public async Task<EntityType> CreateAsync(EntityType entity) {
            return (await _ctx.Set<EntityType>().AddAsync(entity)).Entity;
        }

        public async Task<EntityType> CreateAsync<TSourceType>(TSourceType entity)
        {
            var tmp = Mapper.Map<TSourceType, EntityType>(entity);
            return (await _ctx.Set<EntityType>().AddAsync(tmp)).Entity;
        }

        protected IQueryable<EntityType> AllAsyncInternal(
            Expression<Func<EntityType, bool>> predicate, 
            Expression<Func<EntityType, object>> include,
            Expression<Func<EntityType, object>> sortByAscending,
            Expression<Func<EntityType, object>> sortByDescending)
        {
            var tmp = _ctx.Set<EntityType>().AsQueryable();

            if (include != null)
            {
                tmp = tmp.Include(include);
            }

            if (predicate != null)
            {
                tmp = tmp.Where(predicate);
            }

            if (sortByAscending != null) {
                tmp = tmp.OrderBy(sortByAscending);
            }

            if (sortByDescending != null)
            {
                tmp = tmp.OrderByDescending(sortByDescending);
            }

            return tmp;
        }

        public async Task<List<EntityType>> AllAsync(
            Expression<Func<EntityType, bool>> predicate,
            Expression<Func<EntityType, object>> include,
            Expression<Func<EntityType, object>> sortByAscending,
            Expression<Func<EntityType, object>> sortByDescending)
        {
            var tmp = AllAsyncInternal(predicate, include, sortByAscending, sortByDescending);

            return await tmp.ToListAsync();
        }

        public async Task<List<TDestination>> AllAsync<TDestination>(
            Expression<Func<EntityType, bool>> predicate,
            Expression<Func<EntityType, object>> include,
            Expression<Func<EntityType, object>> sortByAscending,
            Expression<Func<EntityType, object>> sortByDescending)
        {
            var tmp = AllAsyncInternal(predicate, include, sortByAscending, sortByDescending);

            return await tmp
                .ProjectTo<TDestination>()
                .ToListAsync();
        }

        public async Task<EntityType> FindByIdAsync(KeyType id)
        {            
            return await _ctx.Set<EntityType>().FindAsync(id);
        }

        public async Task<TDestination> FindByIdAsync<TDestination>(KeyType id)
        {
            var tmp = await _ctx.Set<EntityType>().FindAsync(id);

            return Mapper.Map<EntityType, TDestination>(tmp);
        }


        public void Update(EntityType entity)
        {
            //_ctx.Set<EntityType>().Attach(entity);
            //_ctx.Entry(entity).State = EntityState.Modified;
            _ctx.Set<EntityType>().Update(entity);
        }

        public void Delete(EntityType entity)
        {
            _ctx.Set<EntityType>().Attach(entity);
            _ctx.Set<EntityType>().Remove(entity);      
        }

        public async Task SaveChangesAsync()
        {
            await _ctx.SaveChangesAsync();
        }


    }
}
