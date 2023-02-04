using AltLibrary.Common.AltTypes;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class CopperOre : AltOre<CopperOreGroup> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Copper;
		OreItem = ItemID.CopperOre;
		OreBar = ItemID.CopperBar;
	}
}
public sealed class TinOre : AltOre<CopperOreGroup> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Tin;
		OreItem = ItemID.TinOre;
		OreBar = ItemID.TinBar;
	}
}
