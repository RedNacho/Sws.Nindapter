using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Syntax;

namespace Sws.Nindapter
{
    public interface IAdaptedBindingToSyntax<T> : IBindingToSyntax<T>
    {
        IBindingWhenInNamedWithOrOnSyntax<TService> ToService<TService>() where TService : T;
        IBindingWhenInNamedWithOrOnSyntax<T> ToService(Type service);
        IBindingWhenInNamedWithOrOnSyntax<T> ToService();
    }
}
