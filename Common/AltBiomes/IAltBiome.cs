using AltLibrary.Common.MaterialContexts;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltBiomes;

public interface IAltBiome : IModType {
	internal static List<IAltBiome> altBiomes = new(8);

	int Type { get; }
	IMaterialContext MaterialContext { get; }
}
