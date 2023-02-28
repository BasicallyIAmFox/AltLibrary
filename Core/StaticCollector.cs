using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AltLibrary.Core;

internal static class StaticCollector {
	private const BindingFlags Flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

	public static List<FieldInfo> Collect() {
		var list = new List<FieldInfo>();
		foreach (var type in typeof(AltLib).Assembly.DefinedTypes) {
			list.AddRange(type.GetFields(Flags));
		}
		return list;
	}

	public static void Clean() {
		var list = Collect();
		for (int i = list.Count - 1; i >= 0; i--) {
			var current = list[i];
			if (current.IsLiteral || current.DeclaringType.GetCustomAttribute<CompilerGeneratedAttribute>() != null || current.IsInitOnly || !current.FieldType.IsClass) {
				continue;
			}

			current.SetValue(null, null);
		}
	}
}
