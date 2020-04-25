using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wcs.Common.Config;
using Wcs.Models;

namespace ORMExplore.unility
{
    /// <summary>
    /// sql生成器，同时利用泛型缓存对sql进行缓存，
    /// 泛型静态构造函数对每个类型的T会缓存一份，下次再来调用，会直接用
    /// 此种缓存不会释放，所以适合那种体积比较小，每个类别有一个的缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SqlBuilder<T> where T : BaseModel  //继承一下BaseModel为了限制表一定要有Id字段，不然会出错
    {
        private static string _findSql = string.Empty;
        private static string _insertSql = string.Empty;
        static SqlBuilder()
        {
            Type type = typeof(T);
            string tableName = type.GetMappingName();
            string para = type.GetProperties().Select(p => $"[{ p.GetMappingName()}]").Aggregate((x, y) => x + "," + y);
            _findSql = $" select {para} from {tableName} where Id=";

            //扩展方法GetPropertiesWithoutKey把主键过滤掉
            string propString = type.GetPropertiesWithoutKey().Select(s => s.GetMappingName()).Aggregate((x, y) => x + "," + y);
            string valueString = type.GetPropertiesWithoutKey().Select(s => $"@{s.GetMappingName()}").Aggregate((x, y) => x + "," + y);
            _insertSql = $@" insert into {type.GetMappingName()}({propString}) values({valueString});select @@identity;"; 
        }

        public static string GetFindSql()
        {
            return _findSql;
        }
        public static string GetInertSql()
        {
            return _insertSql;
        }
    }



}
