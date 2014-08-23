using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Activation;

namespace Sws.Nindapter
{
    public class AdapterProviderFactory : IAdapterProviderFactory
    {

        public Provider<T> CreateAdapterProvider<TAdaptee, T>(IProvider provider, Func<TAdaptee, T> adapterFactory)
        {
            return new AdapterProvider<TAdaptee, T>(provider, adapterFactory);
        }

    }
}
