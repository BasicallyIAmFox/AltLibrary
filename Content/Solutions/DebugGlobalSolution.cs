using AltLibrary.Common.Solutions;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Content.Solutions;

public sealed class DebugGlobalSolution : GlobalSolution {
	public override bool AppliesToEntity(ModSolution entity, bool lateInstantiation) {
		return entity.Type == ModContent.GetInstance<BlueSolution>().Type;
	}

	public override void SetStaticDefaults(ModSolution solution) {
		solution.Conversion
			.From(TileID.AshWood)
			.To(TileID.DynastyWood)
			.RegisterTile();
	}
}
