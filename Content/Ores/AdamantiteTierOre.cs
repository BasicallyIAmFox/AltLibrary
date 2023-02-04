using AltLibrary.Common.AltTypes;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class AdamantiteOre : AltOre<AdamantiteOreGroup> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Adamantite;
		OreItem = ItemID.AdamantiteOre;
		OreBar = ItemID.AdamantiteBar;
	}
}
public sealed class TitaniumOre : AltOre<AdamantiteOreGroup> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Titanium;
		OreItem = ItemID.TitaniumOre;
		OreBar = ItemID.TitaniumBar;
	}
}
