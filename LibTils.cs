using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace AltLibrary;

public static class LibUtils {
	public static IEnumerable<IGrouping<Type, TArray>> SeparateTypes<TArray>(this IEnumerable<TArray> objects, Type baseDeclaringType) {
		return objects.GroupBy(x => {
			Type oldType = null;
			Type type = x.GetType().BaseType;
			// It's a special case where I should NOT use ToString() or FullName.
			// For whatever reason, doing this causes to result type name be C[[B[[A]]]] instead of C
			while ($"{type.Namespace}.{type.Name}" != baseDeclaringType?.FullName) {
				oldType = type;
				type = type.BaseType;
			}
			return Type.GetType($"{oldType.Namespace}.{oldType.Name}");
		});
	}

	public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action) {
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

	public static void ForEachType(Func<Type, bool> whereFunc, Action<Type, Mod> action, bool parallelize = false) {
		var mods = ModLoader.Mods;
		if (parallelize) {
			Parallel.ForEach(mods, new ParallelOptions { MaxDegreeOfParallelism = 4 }, x => ForEachSpecificMod(x, whereFunc, action));
			return;
		}
		for (int i = mods.Length - 1; i >= 1; i--) {
			ForEachSpecificMod(mods[i], whereFunc, action);
		}
	}

	public static void ForEachContent<T>(Action<T> action) where T : ILoadable => ModContent.GetContent<T>().ForEach(type => action(type));

	internal static T To<T>(this string fullName) where T : IModType
		=> ModContent.TryFind<T>(fullName, out var value) ? value : default;
}
