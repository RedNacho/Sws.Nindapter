using System;
using Ninject.Infrastructure.Introspection;
using Ninject.Planning.Bindings;
using Ninject.Syntax;

namespace Sws.Nindapter.Extensions
{
    public static class BindingWhenInNamedWithOrOnSyntaxExtensions
    {
        public static IBindingWhenInNamedWithOrOnSyntax<T> WithDecorator<T>(this IBindingWhenInNamedWithOrOnSyntax<T> bindingWhenInNamedWithOrOnSyntax, Func<T, T> decoratorFactory)
        {
            var bindingConfiguration = bindingWhenInNamedWithOrOnSyntax.BindingConfiguration;

            var providerCallback = bindingConfiguration.ProviderCallback;

            var adapterProviderFactory = new AdapterProviderFactory();

            bindingConfiguration.ProviderCallback = context =>
                adapterProviderFactory.CreateAdapterProvider(providerCallback(context), decoratorFactory);

            return new BindingConfigurationBuilder<T>(bindingConfiguration, typeof(T).Format(), bindingWhenInNamedWithOrOnSyntax.Kernel);
        }
    }
}
