using AlertReport.Db.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AlertReport.Db.DAL
{
    public class AlertReportRepository<T> : IAlertReportRepository<T> where T : class
    {
        private DbContext dbContext;
        private IDbSet<T> dbSet;
        
        public void SetContextAndDbSet(DbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
            dbContext.SaveChanges();
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> expression)
        {
            return dbSet.Where(expression);
        }

        public IQueryable<T> GetAll()
        {
            return dbSet;
        }

        public void Update(T entity)
        {
            dbSet.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            T dbEntity = dbSet.Find(id);
            dbSet.Remove(dbEntity);
            dbContext.SaveChanges();
        }
    }
}
