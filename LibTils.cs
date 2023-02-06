using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
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
		var array = ArrayPool<T>.Shared.Rent(enumerable.Count());
		for (int i = 0, c = array.Length; i < c; i++) {
			array[i] = enumerable.ElementAt(i);
			action(array[i]);
		}
		ArrayPool<T>.Shared.Return(array, true);
		return enumerable;
	}

	public static void ForEachSpecificMod(Mod mod, Func<Type, bool> whereFunc, Action<Type, Mod> action) {
		var enumerable = mod.Code.GetTypes().Where(x => whereFunc(x));
		for (int i = enumerable.Count() - 1; i >= 0; i--) {
			action(enumerable.ElementAt(i), mod);
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
