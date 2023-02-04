using AltLibrary.Common.AltTypes;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class GoldOre : AltOre<GoldOreGroup> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Gold;
		OreItem = ItemID.GoldOre;
		OreBar = ItemID.GoldBar;
	}
}
public sealed class PlatinumOre : AltOre<GoldOreGroup> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Platinum;
		OreItem = ItemID.PlatinumOre;
		OreBar = ItemID.PlatinumBar;
	}
}
