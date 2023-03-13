using AltLibrary.Common.Groups;
using AltLibrary.Content.Groups;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace AltLibrary.Common.Ores;

public static class AltOreLoader {
	internal static readonly IList<ModAltOre> modOres = new List<ModAltOre>();

	public static int Count => modOres.Count;

	public static ModAltOre Get(int id) => (uint)id >= Count ? modOres[id] : null;

	internal static int Register<T>(ModAltOre<T> ore) where T : Group<ModAltOre> {
		ModTypeLookup<ModAltOre>.Register(ore);
		ModTypeLookup<ModAltOre<T>>.Register(ore);
		return modOres.AddIndexReturn(ore);
	}

	public static IEnumerable<ModAltOre<T>> OfType<T>() where T : OreGroup {
		return ModContent.GetContent<ModAltOre<T>>();
	}
}
