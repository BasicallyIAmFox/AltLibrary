using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;

namespace AltLibrary;

internal static partial class LibUtils {
	public static TCast As<TCast>(this object value) => (TCast)value;

	public static string[] CreateNamesBasedOnFields(Type type, BindingFlags flags) {
		return type.GetFields(flags).Select(x => x.Name).ToArray();
	}

	internal static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action) {
		var array = enumerable.ToArray();
		for (int i = array.Length - 1; i >= 0; i--) {
			action(array[i]);
		}
		return enumerable;
	}

	public static void ForEachSpecificMod(Mod mod, Func<Type, bool> whereFunc, Action<Type, Mod> action) {
		var array = mod.Code.GetTypes().Where(x => whereFunc(x)).ToArray();
		for (int i = array.Length - 1; i >= 0; i--) {
			action(array[i], mod);
		}
	}

	public static void ForEachType(Func<Type, bool> whereFunc, Action<Type, Mod> action) {
		var mods = ModLoader.Mods;
		for (int i = mods.Length - 1; i >= 0; i--) {
			ForEachSpecificMod(mods[i], whereFunc, action);
		}
	}

	public static void ForEachContent<T>(Action<T> action) where T : ILoadable => ModContent.GetContent<T>().ForEach(type => action(type));
}
