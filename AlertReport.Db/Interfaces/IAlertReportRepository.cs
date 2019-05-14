using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AlertReport.Db.Interfaces
{
    public interface IAlertReportRepository<T> : IDisposable where T: class
    {
        void Add(T entity);
        T Get(int id);
        IEnumerable<T> Get(Expression<Func<T, bool>> expression);
        IEnumerable<T> GetAll();
        void Update(T entity);
        void Delete(int id);
    }
}
