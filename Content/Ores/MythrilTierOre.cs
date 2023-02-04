using AltLibrary.Common.AltTypes;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class MythrilOre : AltOre<MythrilOreGroup> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Mythril;
		OreItem = ItemID.MythrilOre;
		OreBar = ItemID.MythrilBar;
	}
}
public sealed class OrichalcumOre : AltOre<MythrilOreGroup> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Orichalcum;
		OreItem = ItemID.OrichalcumOre;
		OreBar = ItemID.OrichalcumBar;
	}
}
