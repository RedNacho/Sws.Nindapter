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

    public class AdaptedBindingBuilder<TAdaptee, TAdapted> : BindingBuilder, IBindingToSyntax<TAdaptee>
    {

        private readonly Func<TAdaptee, TAdapted> _adapterFactory;

        public AdaptedBindingBuilder(IBindingConfiguration bindingConfiguration, IKernel kernel, string serviceNames, Func<TAdaptee, TAdapted> adapterFactory)
            : base(bindingConfiguration, kernel, serviceNames)
        {
            if (adapterFactory == null)
            {
                throw new ArgumentNullException("adapterFactory");
            }

            _adapterFactory = adapterFactory;
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
                new AdapterProvider<TAdaptee, TAdapted>(providerCallback(context), _adapterFactory);
        }

        private IBindingWhenInNamedWithOrOnSyntax<TImplementation> GetWhenInNamedWithOrOnSyntax<TImplementation>()
        {
            return new BindingConfigurationBuilder<TImplementation>(BindingConfiguration,
                ServiceNames, Kernel);
        }

    }

}
