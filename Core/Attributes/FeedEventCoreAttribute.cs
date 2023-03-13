using MonoMod.Utils;
using System;
using System.Reflection;

namespace AltLibrary.Core.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
internal sealed class FeedEventCoreAttribute : Attribute {
	public string Name { get; }

	public FeedEventCoreAttribute(string name) {
		Name = name;
	}

	public static string GetName(Type type) => type?.GetCustomAttribute<FeedEventCoreAttribute>()?.Name;
	public static MethodInfo GetMethod(Type type) => type?.FindMethod(GetName(type));
}
