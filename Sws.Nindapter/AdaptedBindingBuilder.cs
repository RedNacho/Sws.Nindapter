using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Ninject;
using Ninject.Activation;
using Ninject.Planning.Bindings;
using Ninject.Syntax;

namespace Sws.Nindapter
{

    public class AdaptedBindingBuilder<TAdaptee, TAdapted> : BindingBuilder, IAdaptedBindingToSyntax<TAdaptee>
    {

        private readonly Func<TAdaptee, TAdapted> _adapterFactory;

        private readonly IAdapterProviderFactory _adapterProviderFactory;

        public AdaptedBindingBuilder(IBindingConfiguration bindingConfiguration, IKernel kernel, string serviceNames, Func<TAdaptee, TAdapted> adapterFactory, IAdapterProviderFactory adapterProviderFactory)
            : base(bindingConfiguration, kernel, serviceNames)
        {
            if (adapterFactory == null)
            {
                throw new ArgumentNullException("adapterFactory");
            }

            if (adapterProviderFactory == null)
            {
                throw new ArgumentNullException("adapterProviderFactory");
            }

            _adapterFactory = adapterFactory;

            _adapterProviderFactory = adapterProviderFactory;
        }

        public IBindingWhenInNamedWithOrOnSyntax<TAdaptee> To(Type implementation)
        {
            return AdaptedInternalTo<TAdaptee>(implementation);
        }

        public IBindingWhenInNamedWithOrOnSyntax<TImplementation> To<TImplementation>() where TImplementation : TAdaptee
        {
            return AdaptedInternalTo<TImplementation>(typeof(TImplementation));
        }

        public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstant<TImplementation>(TImplementation value) where TImplementation : TAdaptee
        {
            InternalToConfiguration(_adapterFactory(value));

            return GetWhenInNamedWithOrOnSyntax<TImplementation>();
        }

        public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstructor<TImplementation>(Expression<Func<IConstructorArgumentSyntax, TImplementation>> newExpression) where TImplementation : TAdaptee
        {
            InternalToConstructor<TImplementation>(newExpression);

            AdaptProviderCallback(BindingConfiguration);

            return GetWhenInNamedWithOrOnSyntax<TImplementation>();
        }

        public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToMethod<TImplementation>(Func<IContext, TImplementation> method) where TImplementation : TAdaptee
        {
            InternalToMethod(context => _adapterFactory(method(context)));

            return GetWhenInNamedWithOrOnSyntax<TImplementation>();
        }

        public IBindingWhenInNamedWithOrOnSyntax<TAdaptee> ToMethod(Func<IContext, TAdaptee> method)
        {
            return ToMethod<TAdaptee>(method);
        }

        public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToProvider<TImplementation>(IProvider<TImplementation> provider) where TImplementation : TAdaptee
        {
            InternalToProvider(provider);

            AdaptProviderCallback(BindingConfiguration);

            return GetWhenInNamedWithOrOnSyntax<TImplementation>();
        }

        public IBindingWhenInNamedWithOrOnSyntax<TAdaptee> ToProvider(Type providerType)
        {
            ToProviderInternal<TAdaptee>(providerType);

            AdaptProviderCallback(BindingConfiguration);

            return GetWhenInNamedWithOrOnSyntax<TAdaptee>();
        }

        public IBindingWhenInNamedWithOrOnSyntax<TAdaptee> ToProvider<TProvider>() where TProvider : IProvider
        {
            return ToProvider(typeof(TProvider));
        }

        public IBindingWhenInNamedWithOrOnSyntax<TAdaptee> ToSelf()
        {
            return To<TAdaptee>();
        }

        public IBindingWhenInNamedWithOrOnSyntax<TService> ToService<TService>() where TService : TAdaptee
        {
            return InternalToService<TService>(typeof(TService));
        }

        public IBindingWhenInNamedWithOrOnSyntax<TAdaptee> ToService(Type service)
        {
            return InternalToService<TAdaptee>(service);
        }

        public IBindingWhenInNamedWithOrOnSyntax<TAdaptee> ToService()
        {
            return InternalToService<TAdaptee>(typeof(TAdaptee));
        }

        private IBindingWhenInNamedWithOrOnSyntax<TService> InternalToService<TService>(Type service) where TService : TAdaptee
        {
            InternalTo<NindapterServiceContainer<TService, TAdapted>>(typeof(NindapterServiceContainer<TService, TAdapted>));

            var providerCallback = BindingConfiguration.ProviderCallback;

            BindingConfiguration.ProviderCallback = context =>
                _adapterProviderFactory.CreateAdapterProvider(providerCallback(context),
                    (NindapterServiceContainer<TService, TAdapted> nindapterServiceContainer)
                        => _adapterFactory(nindapterServiceContainer.Service));

            return GetWhenInNamedWithOrOnSyntax<TService>();
        }

        private IBindingWhenInNamedWithOrOnSyntax<TImplementation> AdaptedInternalTo<TImplementation>(Type implementation) where TImplementation : TAdaptee
        {
            InternalTo<TImplementation>(implementation);

            AdaptProviderCallback(BindingConfiguration);

            return GetWhenInNamedWithOrOnSyntax<TImplementation>();
        }

        private void AdaptProviderCallback(IBindingConfiguration bindingConfiguration)
        {
            var providerCallback = bindingConfiguration.ProviderCallback;

            bindingConfiguration.ProviderCallback = context =>
                _adapterProviderFactory.CreateAdapterProvider(providerCallback(context), _adapterFactory);
        }

        private IBindingWhenInNamedWithOrOnSyntax<TImplementation> GetWhenInNamedWithOrOnSyntax<TImplementation>()
        {
            return new BindingConfigurationBuilder<TImplementation>(BindingConfiguration,
                ServiceNames, Kernel);
        }

    }

}
