using AltLibrary.Common.AltTypes;
using System;

namespace AltLibrary.Common.OrderGroups;

public abstract class BiomeGroup : AOrderGroup<BiomeGroup, IAltBiome> {
	private protected override Type GetMainSubclass() {
		return typeof(AltBiome<>);
	}
}
