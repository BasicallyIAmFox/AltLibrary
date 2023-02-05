using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Data;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class CopperOre : AltOre<CopperOreGroup> {
	public override string Texture => $"Terraria/Images/Item_{ItemID.CopperOre}";

	public override void SetStaticDefaults() {
		DataHandler.Add(new OreData {
			Bar = ItemID.CopperBar,
			Ore = ItemID.CopperOre,
			Tile = TileID.Copper
		});
	}
}
public sealed class TinOre : AltOre<CopperOreGroup> {
	public override string Texture => $"Terraria/Images/Item_{ItemID.TinOre}";

	public override void SetStaticDefaults() {
		DataHandler.Add(new OreData {
			Bar = ItemID.TinBar,
			Ore = ItemID.TinOre,
			Tile = TileID.Tin
		});
	}
}
