using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Data;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Biomes;

public sealed class CorruptBiome : AltBiome<EvilBiomeGroup> {
	public override void SetStaticDefaults() {
		DataHandler.Add(new ConversionData {
			Stone = TileID.Ebonstone,
			Sandstone = TileID.CorruptSandstone,
			HardSand = TileID.CorruptHardenedSand,

			ThornBush = TileID.CorruptThorns,

			Grass = TileID.CorruptGrass,
			JungleGrass = TileID.CorruptJungleGrass,

			Sand = TileID.Ebonsand,
			Ice = TileID.CorruptIce
		});
	}
}
public sealed class CrimsonBiome : AltBiome<EvilBiomeGroup> {
	public override void SetStaticDefaults() {
		DataHandler.Add(new ConversionData {
			Stone = TileID.Crimstone,
			Sandstone = TileID.CrimsonSandstone,
			HardSand = TileID.CrimsonHardenedSand,

			ThornBush = TileID.CrimsonThorns,

			Grass = TileID.CrimsonGrass,
			JungleGrass = TileID.CrimsonJungleGrass,

			Sand = TileID.Crimsand,
			Ice = TileID.FleshIce
		});
	}
}