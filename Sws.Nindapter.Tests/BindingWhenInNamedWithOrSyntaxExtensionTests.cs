using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Sws.Nindapter.Extensions;

namespace Sws.Nindapter.Tests
{
    [TestClass]
    public class BindingWhenInNamedWithOrSyntaxExtensionTests
    {
        [TestMethod]
        public void WithDecoratorWrapsExistingImplementation()
        {
            var kernel = new StandardKernel();

            Func<int, int> addOneFunc = i => i + 1;

            Func<Func<int, int>, Func<int, int>> multiplyByTwoDecoratorFactory
                = f => i => f(i) * 2;

            kernel.Bind<Func<int, int>>().ToConstant(addOneFunc)
                .WithDecorator(multiplyByTwoDecoratorFactory);

            var resolved = kernel.Get<Func<int, int>>();

            var result = resolved(4);

            Assert.AreEqual(10, result);
        }
    }
}
