using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.BiomeTypes;

namespace AltLibrary.Content.Biomes;

public sealed class EvilTypeBiome : BiomeType {
	public override void SetupContent() {
		Order = 0f;
		base.SetupContent();
	}
}

public sealed class CorruptBiome : AltBiome<EvilTypeBiome> {
	public override void SetStaticDefaults() {
	}
}
public sealed class CrimsonBiome : AltBiome<EvilTypeBiome> {
	public override void SetStaticDefaults() {
	}
}