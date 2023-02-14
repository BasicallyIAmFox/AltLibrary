using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Data;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Biomes;

public sealed class CorruptBiome : AltBiome<EvilBiomeGroup> {
	public override void SetStaticDefaults() {
		DataHandler.Add(new WorldIconData {
			NormalWorldIcon = "AltLibrary/Assets/WorldIcons/Normal/Corrupt",
			DrunkWorldIcon = "AltLibrary/Assets/WorldIcons/Drunk/Corrupt",
			DrunkBaseWorldIcon = "AltLibrary/Assets/WorldIcons/DrunkBase/Corrupt",
			ForTheWorthyWorldIcon = "AltLibrary/Assets/WorldIcons/ForTheWorthy/Corrupt",
			NotTheBeesWorldIcon = "AltLibrary/Assets/WorldIcons/NotTheBees/Corrupt",
			Celebrationmk10WorldIcon = "AltLibrary/Assets/WorldIcons/Anniversary/Corrupt",
			TheConstantWorldIcon = "AltLibrary/Assets/WorldIcons/DontStarve/Corrupt",
			NoTrapsWorldIcon = "AltLibrary/Assets/WorldIcons/NoTraps/Corrupt",
			DontDigUpWorldIcon = "AltLibrary/Assets/WorldIcons/Remix/Corrupt",

			GetFixedBoiLeftWorldIcon = "AltLibrary/Assets/WorldIcons/Normal/Corrupt",
			GetFixedBoiFullWorldIcon = "AltLibrary/Assets/WorldIcons/DrunkBase/Corrupt",
			GetFixedBoiRightWorldIcon = "AltLibrary/Assets/WorldIcons/Drunk/Corrupt",
		});
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
		DataHandler.Add(new WorldIconData {
			NormalWorldIcon = "AltLibrary/Assets/WorldIcons/Normal/Crimson",
			DrunkWorldIcon = "AltLibrary/Assets/WorldIcons/Drunk/Crimson",
			DrunkBaseWorldIcon = "AltLibrary/Assets/WorldIcons/DrunkBase/Crimson",
			ForTheWorthyWorldIcon = "AltLibrary/Assets/WorldIcons/ForTheWorthy/Crimson",
			NotTheBeesWorldIcon = "AltLibrary/Assets/WorldIcons/NotTheBees/Crimson",
			Celebrationmk10WorldIcon = "AltLibrary/Assets/WorldIcons/Anniversary/Crimson",
			TheConstantWorldIcon = "AltLibrary/Assets/WorldIcons/DontStarve/Crimson",
			NoTrapsWorldIcon = "AltLibrary/Assets/WorldIcons/NoTraps/Crimson",
			DontDigUpWorldIcon = "AltLibrary/Assets/WorldIcons/Remix/Crimson",

			GetFixedBoiLeftWorldIcon = "AltLibrary/Assets/WorldIcons/Normal/Crimson",
			GetFixedBoiFullWorldIcon = "AltLibrary/Assets/WorldIcons/DrunkBase/Crimson",
			GetFixedBoiRightWorldIcon = "AltLibrary/Assets/WorldIcons/Drunk/Crimson",
		});
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