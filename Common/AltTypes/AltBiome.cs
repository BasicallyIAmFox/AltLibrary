using AltLibrary.Common.MaterialContexts;
using AltLibrary.Common.OrderGroups;
using System.Collections.Generic;

namespace AltLibrary.Common.AltTypes;

public interface IAltBiome : IAAltType {
	internal static List<IAltBiome> altBiomes = new(8);
}
public abstract class AltBiome<T> : AAltType<AltBiome<T>, T, IAltBiome, BiomeMaterialContext>, IAltBiome where T : BiomeGroup {
	private protected override List<IAltBiome> GetListOfTypes() => IAltBiome.altBiomes;
}
