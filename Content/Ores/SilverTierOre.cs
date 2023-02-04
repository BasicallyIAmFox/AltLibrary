using AltLibrary.Common.AltTypes;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class SilverOre : AltOre<SilverOreGroup> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Silver;
		OreItem = ItemID.SilverOre;
		OreBar = ItemID.SilverBar;
	}
}
public sealed class TungstenOre : AltOre<SilverOreGroup> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Tungsten;
		OreItem = ItemID.TungstenOre;
		OreBar = ItemID.TungstenBar;
	}
}
