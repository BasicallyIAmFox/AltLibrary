using System.Collections.Generic;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltOres;

public interface IAltOre : IModType {
	internal static List<IAltOre> altOres = new(8);

	int OreTile { get; }
	int OreBar { get; }
	int OreItem { get; }
}
