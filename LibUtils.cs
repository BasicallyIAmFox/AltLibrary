using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace AltLibrary;

internal static partial class LibUtils {
	public static TCast As<TCast>(this object value) => (TCast)value;

	public static bool TryFindIndex<T>(this List<T> values, Predicate<T> predicate, out int i) {
		i = values.FindIndex(predicate);
		return i != -1;
	}

	public static bool TryFindIndex<T>(this List<T> values, int startIndex, Predicate<T> predicate, out int i) {
		i = values.FindIndex(startIndex, predicate);
		return i != -1;
	}

	public static bool TryFindLastIndex<T>(this List<T> values, Predicate<T> predicate, out int i) {
		i = values.FindLastIndex(predicate);
		return i != -1;
	}

	public static bool TryFindLastIndex<T>(this List<T> values, int startIndex, Predicate<T> predicate, out int i) {
		i = values.FindLastIndex(startIndex, predicate);
		return i != -1;
	}

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
