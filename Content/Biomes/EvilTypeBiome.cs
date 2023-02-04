using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Data;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Biomes;

public sealed class CorruptBiome : AltBiome<EvilBiomeGroup> {
	public override void SetStaticDefaults() {
		DataHandler.Add(new ConversionData {
			Grass = TileID.CorruptGrass,
			Stone = TileID.Ebonstone,
			Ice = TileID.CorruptIce
		});
	}
}
public sealed class CrimsonBiome : AltBiome<EvilBiomeGroup> {
	public override void SetStaticDefaults() {
		DataHandler.Add(new ConversionData {
			Grass = TileID.CrimsonGrass,
			Stone = TileID.Crimstone,
			Ice = TileID.FleshIce
		});
	}
}