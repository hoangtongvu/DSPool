using System;

namespace DSPool;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class DSPoolSingletonAttribute : Attribute
{
}