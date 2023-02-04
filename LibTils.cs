using System;
using System.Linq;
using Terraria.ModLoader;

namespace AltLibrary;

public static class LibTils {
	public static void ForEachSpecificMod(Mod mod, Func<Type, bool> whereFunc, Action<Type, Mod> action) {
		using var enumerator = mod.Code.GetTypes().Where(x => whereFunc(x)).GetEnumerator();
		enumerator.Reset();
		while (enumerator.MoveNext()) {
			action(enumerator.Current, mod);
		}
	}

	public static void ForEachType(Func<Type, bool> whereFunc, Action<Type, Mod> action) {
		var mods = ModLoader.Mods;
		for (int i = 1, c = mods.Length; i < c; i++) {
			ForEachSpecificMod(mods[i], whereFunc, action);
		}
	}

	public static void ForEachContent<T>(Action<T> action) where T : ILoadable {
		using var content = ModContent.GetContent<T>().GetEnumerator();
		content.Reset();
		while (content.MoveNext()) {
			action(content.Current);
		}
	}
}
