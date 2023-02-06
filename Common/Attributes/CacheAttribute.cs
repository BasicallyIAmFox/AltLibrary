using System;

namespace AltLibrary.Common.Attributes;


[AttributeUsage(AttributeTargets.Class)]
public class CacheAttribute : Attribute {
	public string MethodName { get; }

	public CacheAttribute(string methodName) {
		MethodName = methodName;
	}
}
