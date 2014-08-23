using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Sws.Nindapter.Extensions;

namespace Sws.Nindapter.Demo
{
    class Program
    {

        static void Main(string[] args)
        {
            var kernel = new StandardKernel();

            kernel.Bind<string>().ThroughAdapter((int[] values) =>
                {
                    var stringBuilder = new StringBuilder();
                    foreach (int value in values)
                    {
                        stringBuilder.AppendLine(value.ToString());
                    }
                    return stringBuilder.ToString();
                }
            ).ToConstant(new [] { 1, 2, 3, 4 });

            var data = kernel.Get<string>();

            Console.WriteLine(data);

            Console.ReadKey();
        }

    }
}
