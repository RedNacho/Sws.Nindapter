[![Build status](https://ci.appveyor.com/api/projects/status/7ygq3na764bu7st8/branch/master?svg=true)](https://ci.appveyor.com/project/RedNacho/sws-nindapter/branch/master)

Sws.Nindapter
=============

Syntax extension for Ninject which lets you put an adapter between a bound service and its implementation.

Usage:

1. Import Sws.Nindapter.Extensions.
2. Use .Bind(...).ThroughAdapter(...).To... (or alternatives, see below for more...)

e.g.

kernel.Bind<string>().ThroughAdapter((int i) => i.ToString()).ToConstant(42);

There is also a ThroughDecorator extension method, which is identical to ThroughAdapter except that the adaptee must be the same type as the adapter (i.e. you want to modify or wrap the bound class, but you don't need to change its interface).

There are .ToService(...) methods available after you call ThroughAdapter or ThroughDecorator, which let you reuse an existing binding for the adaptee rather than explicitly specifying the implementation.  This functionality is very basic at present.

Finally, there is now a .WithDecorator(...) method, which you can use after calling .To... to decorate an existing binding. This is useful in the scenario where the .To... method is an extension method and is not implemented by ThroughAdapter/ThroughDecorator, for example .ToNative in Sws.Spinvoke. This functionality has only been implemented for the special case of decorators because in this case Bind and To will have the same generic type parameters.