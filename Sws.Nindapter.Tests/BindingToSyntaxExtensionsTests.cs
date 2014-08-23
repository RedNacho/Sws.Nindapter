using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Ninject.Activation;
using Ninject.Activation.Providers;
using FluentAssertions;
using Sws.Nindapter.Extensions;

namespace Sws.Nindapter.Tests
{
    [TestClass]
    public class BindingToSyntaxExtensionsTests
    {

        public interface IAdaptee
        {
            string AdapteeValue { get; }
        }

        public class Adaptee : IAdaptee
        {
            private readonly string _parameter;

            public Adaptee() : this("Adaptee Value")
            {   
            }

            public Adaptee(string parameter)
            {
                _parameter = parameter;
            }

            public string AdapteeValue
            {
                get { return _parameter; }
            }
        }

        public interface IAdapted
        {
            string AdaptedValue { get; }
        }

        public class Adapter : IAdapted
        {

            private readonly IAdaptee _adaptee;

            public Adapter(IAdaptee adaptee)
            {
                _adaptee = adaptee;
            }

            public string AdaptedValue
            {
                get { return "Adapted " + _adaptee.AdapteeValue; }
            }

        }

        [TestMethod]
        public void ThroughAdapterToWithGenericTypeParamInvokesAdapterWhenBound()
        {
            var kernel = new StandardKernel();

            kernel.Bind<IAdapted>().ThroughAdapter((IAdaptee adaptee) => new Adapter(adaptee)).To<Adaptee>();

            var adapted = kernel.Get<IAdapted>();

            adapted.AdaptedValue.Should().Be("Adapted Adaptee Value");
        }

        [TestMethod]
        public void ThroughAdapterToWithoutGenericTypeParamInvokesAdapterWhenBound()
        {
            var kernel = new StandardKernel();

            kernel.Bind<IAdapted>().ThroughAdapter((IAdaptee adaptee) => new Adapter(adaptee)).To(typeof(Adaptee));

            var adapted = kernel.Get<IAdapted>();

            adapted.AdaptedValue.Should().Be("Adapted Adaptee Value");
        }

        [TestMethod]
        public void ThroughAdapterToConstantInvokesAdapterWhenBound()
        {
            var kernel = new StandardKernel();

            kernel.Bind<IAdapted>().ThroughAdapter((IAdaptee adaptee) => new Adapter(adaptee)).ToConstant(new Adaptee());

            var adapted = kernel.Get<IAdapted>();

            adapted.AdaptedValue.Should().Be("Adapted Adaptee Value");
        }

        [TestMethod]
        public void ThroughAdapterToConstructorInvokesAdapterWhenBound()
        {
            var kernel = new StandardKernel();

            kernel.Bind<IAdapted>().ThroughAdapter((IAdaptee adaptee) => new Adapter(adaptee)).ToConstructor(constructorArgumentSyntax => new Adaptee("Constructor Param"));

            var adapted = kernel.Get<IAdapted>();

            adapted.AdaptedValue.Should().Be("Adapted Constructor Param");
        }

        [TestMethod]
        public void ThroughAdapterToMethodWithGenericTypeParamInvokesAdapterWhenBound()
        {
            var kernel = new StandardKernel();

            kernel.Bind<IAdapted>().ThroughAdapter((IAdaptee adaptee) => new Adapter(adaptee)).ToMethod<Adaptee>(context => new Adaptee());

            var adapted = kernel.Get<IAdapted>();

            adapted.AdaptedValue.Should().Be("Adapted Adaptee Value");
        }

        [TestMethod]
        public void ThroughAdapterToMethodWithoutGenericTypeParamInvokesAdapterWhenBound()
        {
            var kernel = new StandardKernel();

            kernel.Bind<IAdapted>().ThroughAdapter((IAdaptee adaptee) => new Adapter(adaptee)).ToMethod(context => (IAdaptee)(new Adaptee()));

            var adapted = kernel.Get<IAdapted>();

            adapted.AdaptedValue.Should().Be("Adapted Adaptee Value");
        }

        [TestMethod]
        public void ThroughAdapterToProviderWithInstanceInvokesAdapterWhenBound()
        {
            var kernel = new StandardKernel();

            kernel.Bind<IAdapted>().ThroughAdapter((IAdaptee adaptee) => new Adapter(adaptee)).ToProvider<Adaptee>(new ConstantProvider<Adaptee>(new Adaptee()));

            var adapted = kernel.Get<IAdapted>();

            adapted.AdaptedValue.Should().Be("Adapted Adaptee Value");
        }

        [TestMethod]
        public void ThroughAdapterToProviderWithGenericTypeParamInvokesAdapterWhenBound()
        {
            var kernel = new StandardKernel();

            kernel.Bind<IProvider<IAdaptee>>().ToConstant(new ConstantProvider<IAdaptee>(new Adaptee()));
            kernel.Bind<IAdapted>().ThroughAdapter((IAdaptee adaptee) => new Adapter(adaptee)).ToProvider<IProvider<IAdaptee>>();

            var adapted = kernel.Get<IAdapted>();

            adapted.AdaptedValue.Should().Be("Adapted Adaptee Value");
        }

        [TestMethod]
        public void ThroughAdapterToProviderWithoutGenericTypeParamInvokesAdapterWhenBound()
        {
            var kernel = new StandardKernel();

            kernel.Bind<IProvider<IAdaptee>>().ToConstant(new ConstantProvider<IAdaptee>(new Adaptee()));
            kernel.Bind<IAdapted>().ThroughAdapter((IAdaptee adaptee) => new Adapter(adaptee)).ToProvider(typeof(IProvider<IAdaptee>));

            var adapted = kernel.Get<IAdapted>();

            adapted.AdaptedValue.Should().Be("Adapted Adaptee Value");
        }

        [TestMethod]
        public void ThroughAdapterToSelfInvokesAdapterWhenBound()
        {
            var kernel = new StandardKernel();

            kernel.Bind<IAdapted>().ThroughAdapter((Adaptee adaptee) => new Adapter(adaptee)).ToSelf();

            var adapted = kernel.Get<IAdapted>();

            adapted.AdaptedValue.Should().Be("Adapted Adaptee Value");
        }

    }
}
