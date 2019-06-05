using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AlertReport.Db.Interfaces
{
    public interface IAlertReportRepository<T> where T: class
    {
        void Add(T entity);
        T Get(int id);
        IQueryable<T> Get(Expression<Func<T, bool>> expression);
        IQueryable<T> GetAll();
        void Update(T entity);
        void Delete(int id);
        void SetContextAndDbSet(DbContext dbContext);
    }
}
