using System;

namespace DSPool;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class DSPoolUsePrefabAttribute : Attribute
{
	public DSPoolUsePrefabAttribute(Type prefabType)
	{
	}
}