using AltLibrary.Common.AltTypes;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class CobaltOre : AltOre<CobaltOreGroup> {
	public override string Texture => $"Terraria/Images/Item_{ItemID.CobaltOre}";

	public override void SetStaticDefaults() {
	}
}
public sealed class PalladiumOre : AltOre<CobaltOreGroup> {
	public override string Texture => $"Terraria/Images/Item_{ItemID.PalladiumOre}";

	public override void SetStaticDefaults() {
	}
}
