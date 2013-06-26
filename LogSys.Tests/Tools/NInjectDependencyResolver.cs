#region Usings

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;

#endregion

namespace LogSys.Tests.Tools
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel) { _kernel = kernel; }

        public object GetService(Type serviceType) { return _kernel.TryGet(serviceType); }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _kernel.GetAll(serviceType);
            }
            catch
            {
                return new List<object>();
            }
        }
    }
}