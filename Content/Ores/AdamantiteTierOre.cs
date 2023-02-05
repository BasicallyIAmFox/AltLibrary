using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Data;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class AdamantiteOre : AltOre<AdamantiteOreGroup> {
	public override string Texture => $"Terraria/Images/Item_{ItemID.AdamantiteOre}";

	public override void SetStaticDefaults() {
		DataHandler.Add(new OreData {
			Bar = ItemID.AdamantiteBar,
			Ore = ItemID.AdamantiteOre,
			Tile = TileID.Adamantite
		});
	}
}
public sealed class TitaniumOre : AltOre<AdamantiteOreGroup> {
	public override string Texture => $"Terraria/Images/Item_{ItemID.TitaniumOre}";

	public override void SetStaticDefaults() {
		DataHandler.Add(new OreData {
			Bar = ItemID.TitaniumBar,
			Ore = ItemID.TitaniumOre,
			Tile = TileID.Titanium
		});
	}
}
