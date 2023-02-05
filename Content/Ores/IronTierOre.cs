using AltLibrary.Common.AltTypes;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class IronOre : AltOre<IronOreGroup> {
	public override string Texture => $"Terraria/Images/Item_{ItemID.IronOre}";

	public override void SetStaticDefaults() {
	}
}
public sealed class LeadOre : AltOre<IronOreGroup> {
	public override string Texture => $"Terraria/Images/Item_{ItemID.LeadOre}";

	public override void SetStaticDefaults() {
	}
}
