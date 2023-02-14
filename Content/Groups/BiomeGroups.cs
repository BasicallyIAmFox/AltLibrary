using AltLibrary.Common.OrderGroups;

namespace AltLibrary.Content.Groups;

public sealed class EvilBiomeGroup : BiomeGroup {
	public sealed override void SetupContent() {
		Order = 0f;
		base.SetupContent();
	}
}
public sealed class GoodBiomeGroup : BiomeGroup {
	public sealed override void SetupContent() {
		Order = 1f;
		base.SetupContent();
	}
}
public sealed class TropicsBiomeGroup : BiomeGroup {
	public sealed override void SetupContent() {
		Order = 2f;
		base.SetupContent();
	}
}
public sealed class UnderworldBiomeGroup : BiomeGroup {
	public sealed override void SetupContent() {
		Order = 3f;
		base.SetupContent();
	}
}