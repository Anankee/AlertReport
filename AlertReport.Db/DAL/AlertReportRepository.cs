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
        private readonly IDbSet<T> dbSet;

        public AlertReportRepository()
        {
            dbContext= new AlertReportContext(); ;
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

        public IEnumerable<T> Get(Expression<Func<T, bool>> expression)
        {
            return dbSet.Where(expression);
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
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

        public void Dispose()
        {
            dbContext.Dispose();
            dbContext = null;
        }
    }
}
