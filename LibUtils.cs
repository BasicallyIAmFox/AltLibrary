using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace AltLibrary;

public static class LibUtils {
	public static TCast As<TCast>(this object value) => (TCast)value;

	internal static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action) {
		var array = enumerable.ToArray();
		for (int i = array.Length - 1; i >= 0; i--) {
			action(array[i]);
		}
		return enumerable;
	}

	public static void ForEachSpecificMod(Mod mod, Func<Type, bool> whereFunc, Action<Type, Mod> action) {
		var enumerable = mod.Code.GetExportedTypes().Where(x => whereFunc(x));
		var array = enumerable.ToArray();
		for (int i = array.Length - 1; i >= 0; i--) {
			action(array[i], mod);
		}
	}

	public static void ForEachType(Func<Type, bool> whereFunc, Action<Type, Mod> action) {
		var mods = ModLoader.Mods;
		for (int i = mods.Length - 1; i >= 1; i--) {
			ForEachSpecificMod(mods[i], whereFunc, action);
		}
	}

	public static void ForEachContent<T>(Action<T> action) where T : ILoadable => ModContent.GetContent<T>().ForEach(type => action(type));
}
