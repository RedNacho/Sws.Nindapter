using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Activation;

namespace Sws.Nindapter
{

    public class AdapterProvider<TAdaptee, T> : Provider<T>
    {

        private readonly IProvider _adapteeProvider;

        private readonly Func<TAdaptee, T> _adapterFactory;

        public AdapterProvider(IProvider adapteeProvider, Func<TAdaptee, T> adapterFactory)
        {
            if (adapteeProvider == null)
            {
                throw new ArgumentNullException("adapteeProvider");
            }

            if (adapterFactory == null)
            {
                throw new ArgumentNullException("adapterFactory");
            }

            _adapteeProvider = adapteeProvider;
            _adapterFactory = adapterFactory;
        }

        protected override T CreateInstance(IContext context)
        {
            return _adapterFactory((TAdaptee)_adapteeProvider.Create(context));
        }

    }
}
