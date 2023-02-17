using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Data;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Biomes;

#if DEBUG
internal sealed class EverlightBiome : AltBiome<GoodBiomeGroup> {
	public override string Texture => "AltLibrary/Assets/Debug/Everlight";

	public override void SetStaticDefaults() {
		DataHandler.Add(new ConversionData {
			Grass = TileID.GoldBrick,
			MowedGrass = TileID.PlatinumBrick,

			Stone = TileID.Gold,
		});
	}
}
#endif
