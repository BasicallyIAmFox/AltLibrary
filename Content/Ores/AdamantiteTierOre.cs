using AltLibrary.Common.AltTypes;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class AdamantiteOre : AltOre<AdamantiteOreGroup> {
	public override string Texture => $"Terraria/Images/Item_{ItemID.AdamantiteOre}";

	public override void SetStaticDefaults() {
	}
}
public sealed class TitaniumOre : AltOre<AdamantiteOreGroup> {
	public override string Texture => $"Terraria/Images/Item_{ItemID.TitaniumOre}";

	public override void SetStaticDefaults() {
	}
}
