using AltLibrary.Common.AltOres;
using AltLibrary.Common.TierOres;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class AdamantiteTierOre : TierOre {
	public sealed override void SetupContent() {
		Order = 6f;
		base.SetupContent();
	}
}

public sealed class AdamantiteOre : AltOre<AdamantiteTierOre> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Adamantite;
		OreItem = ItemID.AdamantiteOre;
		OreBar = ItemID.AdamantiteBar;
	}
}
public sealed class TitaniumOre : AltOre<AdamantiteTierOre> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Titanium;
		OreItem = ItemID.TitaniumOre;
		OreBar = ItemID.TitaniumBar;
	}
}
