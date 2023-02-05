using AltLibrary.Common.AltTypes;
using System;

namespace AltLibrary.Common.Attributes;

[AttributeUsage(AttributeTargets.Struct)]
public class DataAlwaysExistsAttribute : Attribute {
}
public interface IDataAlwaysExists<A> where A : IAAltType {
	static abstract void CheckThere(A biome);
}