using AltLibrary.Common.Conversion;
using AltLibrary.Common.Globals;
using System;
using Terraria;
using Terraria.ModLoader;

namespace AltLibrary.Common.Solutions;

public abstract class ModSolution : ModType, IModTypeWithGlobal<ModSolution, GlobalSolution> {
	public ConversionData Conversion { get; private set; }
	public int Type { get; private set; }

	private Instanced<GlobalSolution>[] globalsArray = Array.Empty<Instanced<GlobalSolution>>();
	public ref Instanced<GlobalSolution>[] GlobalsArray { get => ref globalsArray; }

	public sealed override void SetupContent() {
		SolutionLoader.SetStaticDefaults(this);
	}

	protected sealed override void Register() {
		Type = SolutionLoader.Register(this);
		Conversion = new(this);
	}

	protected static void SquareFrameTile(int i, int j) {
		WorldGen.SquareTileFrame(i, j);
		NetMessage.SendTileSquare(-1, i, j);
	}

	protected static void TryKillingTreesAboveIfTheyWouldBecomeInvalid(Tile tile, int oldTileType, int i, int j) {
		WorldGen.TryKillingTreesAboveIfTheyWouldBecomeInvalid(i, j, tile.TileType);
	}
}