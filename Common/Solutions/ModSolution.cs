using Terraria.ModLoader;

namespace AltLibrary.Common.Solutions;

public abstract class ModSolution : ModType {
	public int Type { get; private set; }

	public abstract void FillTileEntries(int currentTile, ref int convertedTile, ref bool forceConversionCode);

	public abstract void FillWallEntries(int currentWall, ref int convertedWall, ref bool forceConversionCode);

	public virtual void OnTileConversion(int oldTile, int i, int j) {
	}

	public virtual void OnWallConversion(int oldWall, int i, int j) {
	}

	public sealed override void SetupContent() {
		SetStaticDefaults();
	}

	protected sealed override void Register() {
		ModTypeLookup<ModSolution>.Register(this);

		SolutionLoader.solutions.Add(this);
		Type = SolutionLoader.ReserveID();
	}
}
