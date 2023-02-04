using System;
using System.Linq;
using Terraria.ModLoader;

namespace AltLibrary;

public static class LibTils {
	public static void ForEachSpecificMod(Mod mod, Func<Type, bool> whereFunc, Action<Type, Mod> action) {
		var enumerator = mod.Code.GetTypes().Where(x => whereFunc(x));
		for (int i = enumerator.Count() - 1; i >= 0; i--) {
			action(enumerator.ElementAt(i), mod);
		}
	}

	public static void ForEachType(Func<Type, bool> whereFunc, Action<Type, Mod> action) {
		var mods = ModLoader.Mods;
		for (int i = mods.Length - 1; i >= 1; i--) {
			ForEachSpecificMod(mods[i], whereFunc, action);
		}
	}

	public static void ForEachContent<T>(Action<T> action) where T : ILoadable {
		var content = ModContent.GetContent<T>();
		for (int i = content.Count() - 1; i >= 0; i--) {
			action(content.ElementAt(i));
		}
	}
}
