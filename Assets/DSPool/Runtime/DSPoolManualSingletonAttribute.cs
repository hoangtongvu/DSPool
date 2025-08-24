using System;

namespace DSPool;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class DSPoolManualSingletonAttribute  : Attribute
{
}