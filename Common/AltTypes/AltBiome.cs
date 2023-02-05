using AltLibrary.Common.Attributes;
using AltLibrary.Common.Data;
using AltLibrary.Common.OrderGroups;
using System.Collections.Generic;
using System.Reflection;

namespace AltLibrary.Common.AltTypes;

public interface IAltBiome : IAAltType {
	internal static List<IAltBiome> altBiomes = new(8);
}
public abstract class AltBiome<T> : AAltType<AltBiome<T>, T, IAltBiome>, IAltBiome where T : BiomeGroup {
	public sealed override void SetupContent() {
		DataHandler = new BiomeDataHandler();

		base.SetupContent();
	}

	private protected override List<IAltBiome> GetListOfTypes() => IAltBiome.altBiomes;
}
