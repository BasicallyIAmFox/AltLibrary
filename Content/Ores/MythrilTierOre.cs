using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Data;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class MythrilOre : AltOre<MythrilOreGroup> {
	public override string Texture => $"Terraria/Images/Item_{ItemID.MythrilOre}";

	public override void SetStaticDefaults() {
		DataHandler.Add(new OreData {
			Bar = ItemID.MythrilBar,
			Ore = ItemID.MythrilOre,
			Tile = TileID.Mythril
		});
	}
}
public sealed class OrichalcumOre : AltOre<MythrilOreGroup> {
	public override string Texture => $"Terraria/Images/Item_{ItemID.OrichalcumOre}";

	public override void SetStaticDefaults() {
		DataHandler.Add(new OreData {
			Bar = ItemID.OrichalcumBar,
			Ore = ItemID.OrichalcumOre,
			Tile = TileID.Orichalcum
		});
	}
}
