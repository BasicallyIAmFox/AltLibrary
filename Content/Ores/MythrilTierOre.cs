using AltLibrary.Common.AltOres;
using AltLibrary.Common.TierOres;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class MythrilTierOre : TierOre {
	public sealed override void SetupContent() {
		Order = 5f;
		base.SetupContent();
	}
}

public sealed class MythrilOre : AltOre<MythrilTierOre> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Mythril;
		OreItem = ItemID.MythrilOre;
		OreBar = ItemID.MythrilBar;
	}
}
public sealed class OrichalcumOre : AltOre<MythrilTierOre> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Orichalcum;
		OreItem = ItemID.OrichalcumOre;
		OreBar = ItemID.OrichalcumBar;
	}
}
