Sws.Nindapter
=============

Syntax extension for Ninject which lets you put an adapter between a bound service and its implementation.

Usage:

1. Import Sws.Nindapter.Extensions.
2. Use .Bind(...).ThroughAdapter(...).To...

e.g.

kernel.Bind<string>().ThroughAdapter((int i) => i.ToString()).ToConstant(42);

There is also a ThroughDecorator extension method, which is identical to ThroughAdapter except that the adaptee must be the same type as the adapter (i.e. you want to modify or wrap the bound class, but you don't need to change its interface).

There are .ToService(...) methods available after you call ThroughAdapter or ThroughDecorator, which let you reuse an existing binding for the adaptee rather than explicitly specifying the implementation.  This functionality is very basic at present.
