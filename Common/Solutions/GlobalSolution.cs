using AltLibrary.Core;
using Terraria.ModLoader;

namespace AltLibrary.Common.Solutions;

public abstract class GlobalSolution : GlobalType<ModSolution, GlobalSolution> {
	public GlobalSolution Instance(ModSolution solution) => Instance(solution.GlobalsArray, Index);
	public sealed override void SetupContent() => SetStaticDefaults();

	public sealed override void SetStaticDefaults() {
	}

	public virtual void SetStaticDefaults(ModSolution solution) {
	}

	protected sealed override void Register() {
		SourceAccess.GlobalType_set_Index.Value(this, (ushort)SolutionLoader.Register(this));
	}
}
