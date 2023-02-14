namespace AltLibrary.Common.AltTypes;

public partial interface IAltBiome {
	void ModifyUnderworldLighting(ref float r, ref float g, ref float b, ref bool shouldTilesAffectLighting);
}
public abstract partial class AltBiome<T> {
	public virtual void ModifyUnderworldLighting(ref float r, ref float g, ref float b, ref bool shouldTilesAffectLighting) {
	}
}
