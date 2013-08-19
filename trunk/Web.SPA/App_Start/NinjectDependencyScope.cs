using Ninject;
using Ninject.Syntax;
using System;
using System.Web.Http.Dependencies;

namespace Web.SPA.App_Start
{
    public class NinjectDependencyScope : IDependencyScope
    {
        private bool disposed = false;
        private IResolutionRoot resolver;

        public NinjectDependencyScope(IResolutionRoot resolver)
        {
            this.resolver = resolver;
        }

        ~NinjectDependencyScope()
        {
            Dispose(false);
        }

        public object GetService(Type serviceType)
        {
            if (resolver == null)
            {
                throw new ObjectDisposedException("this", "This scope has been disposed");
            }

            return resolver.TryGet(serviceType);
        }

        public System.Collections.Generic.IEnumerable<object> GetServices(Type serviceType)
        {
            if (resolver == null)
            {
                throw new ObjectDisposedException("this", "This scope has been disposed");
            }

            return resolver.GetAll(serviceType);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    IDisposable disposable = resolver as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }

                    resolver = null;
                }
                disposed = true;
            }
        }
    }
}