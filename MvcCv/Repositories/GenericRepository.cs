using MvcCv.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace MvcCv.Repositories
{
    public class GenericRepository<T> where T : class, new() 
    {
        dbcvEntities db = new dbcvEntities();

        public List<T> List()
        {
            return db.Set<T>().ToList();
        }

        public List<T> ListByAdmin(int kullaniciId)
        {
            var dbSet = db.Set<T>();
            IQueryable<T> query = dbSet;

            // Dinamik Include ve filtreleme işlemleri
            if (HasProperty("tbladmin") && HasProperty("kullaniciID"))
            {
                query = query.Include("tbladmin");
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, "kullaniciID");
                var condition = Expression.Equal(property, Expression.Constant(kullaniciId));
                var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);

                query = query.Where(lambda);
            }
            return query.ToList();
        }

        private bool HasProperty(string propertyName)
        {
            return typeof(T).GetProperty(propertyName) != null;
        }

        public void TAdd(T param)
        {
            db.Set<T>().Add(param);
            db.SaveChanges();
        }

        public void TDelete(T param)
        {
            db.Set<T>().Remove(param);
            db.SaveChanges();
        }

        public void TUpdate(T param)
        {
            db.SaveChanges();
        }

        public T TGet(int id)
        {
            return db.Set<T>().Find(id);
        }

        public T Find(Expression<Func<T,bool>> where)
        {
            return db.Set<T>().FirstOrDefault(where);
        }
    }
}