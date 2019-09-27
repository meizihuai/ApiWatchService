using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace ApiWatchService
{
    public class MyDbContext : DbContext
    {
        //增加 DbSet




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            /// 除了startup里面通过appsetting配置之外，还可以直接在此配置 如下：
            /// server = 123.207.31.37; port = 3306; database = efTest; uid = root; password = Mei19931129; sslmode = none
            string conn = Module.MysqlConnstr;
            optionsBuilder.UseMySQL(conn);//配置连接字符串 必须加sslmode=none
        }

        public int Update<T>(T entity, T newObject, string[] ignoreProperties = null, bool ignoreNull = true) where T : class
        {
            List<PropertyInfo> updatePropertyInfos = new List<PropertyInfo>();
            foreach (var p in typeof(T).GetProperties())
            {
                if (ignoreProperties != null && ignoreProperties.Contains(p.Name)) continue;
                if (ignoreNull)
                {
                    if (p.GetValue(newObject) == null) continue;
                }
                if (p.GetValue(entity) != p.GetValue(newObject))
                {
                    p.SetValue(entity, p.GetValue(newObject));
                    updatePropertyInfos.Add(p);
                }
            }
            if (updatePropertyInfos.Count == 0) return 0;
            int changeCount = Update(entity, updatePropertyInfos);
            return changeCount;
        }
        public int Update<TEntity>(TEntity entity, List<PropertyInfo> updatePropertyInfos) where TEntity : class
        {
            var dbEntityEntry = this.Entry(entity);
            if (updatePropertyInfos == null || updatePropertyInfos.Count == 0) return 0;
            List<string> entityProperties = new List<string>();
            var plist = dbEntityEntry.OriginalValues.Properties.ToList().Select(a => new { a.Name }).ToList();
            plist.ForEach(a => entityProperties.Add(a.Name));
            updatePropertyInfos
                .Where(a => entityProperties.Contains(a.Name)).ToList()
                .ForEach(a => dbEntityEntry.Property(a.Name).IsModified = true);
            return this.SaveChanges();
        }
        public int Update<TEntity>(TEntity entity, params Expression<Func<TEntity, object>>[] updatedProperties) where TEntity : class
        {
            var dbEntityEntry = this.Entry(entity);
            if (updatedProperties != null && updatedProperties.Any())
            {
                foreach (var property in updatedProperties)
                {
                    dbEntityEntry.Property(property).IsModified = true;
                }
            }
            else
            {
                foreach (var property in dbEntityEntry.OriginalValues.Properties)
                {
                    string pName = property.Name;
                    var original = dbEntityEntry.OriginalValues.GetValue<object>(pName);
                    var current = dbEntityEntry.CurrentValues.GetValue<object>(pName);
                    if (original != null && !original.Equals(current))
                    {
                        dbEntityEntry.Property(pName).IsModified = true;
                    }
                }
            }
            return this.SaveChanges();
        }
    }
}
