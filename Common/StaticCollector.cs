using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AltLibrary.Common;

// TODO: Do something for structs...
/// <summary>
/// Used to null-ify static fields.
/// </summary>
public static class StaticCollector {
	private const BindingFlags Flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

	/// <summary>
	/// Collects all static fields.
	/// </summary>
	/// <returns></returns>
	public static IEnumerable<FieldInfo> Collect() {
		var list = new List<FieldInfo>();
		LibTils.ForEachSpecificMod(AltLibrary.Instance,
			x => x.GetFields(Flags).Length > 0,
			(type, mod) => list.AddRange(type.GetFields(Flags)));
		return list;
	}

	/// <summary>
	/// Nullifies (and clears) static fields from AltLibrary assembly.
	/// </summary>
	public static void Clean() {
		using var enumerator = Collect().GetEnumerator();
		enumerator.Reset();
		while (enumerator.MoveNext()) {
			var current = enumerator.Current;
			if (!current.FieldType.IsClass) {
				continue;
			}

			var type = current.GetValue(null).GetType();
			var clearMethod = type.GetMethod("Clear");
			if (clearMethod != null && !clearMethod.GetParameters().Any()) {
				clearMethod.Invoke(null, null);
			}

			current.SetValue(null, null);
		}
	}
}
