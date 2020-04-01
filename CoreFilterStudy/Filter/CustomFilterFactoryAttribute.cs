using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreFilterStudy.Filter
{
    public class CustomFilterFactoryAttribute : Attribute, IFilterFactory
    {
        private Type _filterType = null;
        public CustomFilterFactoryAttribute(Type filterType)
        {
            _filterType = filterType;
        }
        public bool IsReusable => true;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            // IServiceProvider serviceProvider 是个容器
            return (IFilterMetadata)serviceProvider.GetService(_filterType);
        }
    }
}
