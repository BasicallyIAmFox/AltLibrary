using AltLibrary.Common.Conversion;
using Terraria;
using Terraria.ModLoader;

namespace AltLibrary.Common.Solutions;

public abstract class ModSolution : ModType {
	public ConversionData Conversion { get; private set; }
	public int Type { get; private set; }

	public sealed override void SetupContent() {
		SetStaticDefaults();
	}

	protected sealed override void Register() {
		ModTypeLookup<ModSolution>.Register(this);

		Type = SolutionLoader.solutions.Count;
		SolutionLoader.solutions.Add(this);
		Conversion = new(this);
	}

	public static void TryKillingTreesAboveIfTheyWouldBecomeInvalid(Tile tile, int oldTileType, int i, int j) {
		WorldGen.TryKillingTreesAboveIfTheyWouldBecomeInvalid(i, j, tile.TileType);
	}
}