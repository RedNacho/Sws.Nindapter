using System;
using System.Collections.Generic;
using System.Linq;
using Ninject.Syntax;
using Ninject.Infrastructure.Introspection;

namespace Sws.Nindapter.Extensions
{
    public static class BindingToSyntaxExtensions
    {

        public static IBindingToSyntax<T> ThroughDecorator<T>(this IBindingToSyntax<T> bindingToSyntax, Func<T, T> decoratorFactory)
        {
            return ThroughAdapter<T, T>(bindingToSyntax, decoratorFactory);
        }

        public static IBindingToSyntax<TAdaptee> ThroughAdapter<TAdaptee, T>(this IBindingToSyntax<T> bindingToSyntax, Func<TAdaptee, T> adapterFactory)
        {
            if (adapterFactory == null)
            {
                throw new ArgumentNullException("adapterFactory");
            }

            return new AdaptedBindingBuilder<TAdaptee, T>(bindingToSyntax.BindingConfiguration, bindingToSyntax.Kernel, typeof(T).Format(), adapterFactory);
        }

    }
}
