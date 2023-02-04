using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;

namespace AltLibrary.Common;

/// <summary>
/// Used to null-ify static fields.
/// </summary>
public static class StaticCollector {
	private const BindingFlags Flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

	/// <summary>
	/// Collects all types with static fields.
	/// </summary>
	/// <returns></returns>
	public static List<FieldInfo> Collect(Mod mod) {
		var list = new List<FieldInfo>();
		LibTils.ForEachSpecificMod(mod,
			x => x.GetFields(Flags).Length > 0,
			type => list.AddRange(type.GetFields(Flags)));
		return list;
	}

	/// <summary>
	/// Nullifies (and clears) static fields from mod assembly.
	/// </summary>
	public static void Clean(Mod mod) {
		var list = Collect(mod);
		for (int i = list.Count - 1; i >= 0; i--) {
			var current = list[i];
			if (current.IsInitOnly || !current.FieldType.IsClass) {
				continue;
			}

			current.SetValue(null, null);
		}
	}
}