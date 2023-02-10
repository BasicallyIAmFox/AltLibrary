using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Data;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common.CID;

public sealed class ConversionInheritanceDataTile : ConversionInheritanceData {
	public override void Bake() {
		for (int x = 0; x < TileLoader.TileCount; x++) {
			if (TileID.Sets.Conversion.GolfGrass[x] && x != TileID.GolfGrass) {
				Set(DecleminationId, x, TileID.GolfGrass);
			}
			else if (TileID.Sets.Conversion.Grass[x] && x != TileID.Grass) {
				Set(DecleminationId, x, TileID.Grass);
			}
			else if (TileID.Sets.Conversion.JungleGrass[x] && x != TileID.JungleGrass) {
				Set(DecleminationId, x, TileID.JungleGrass);
			}
			else if ((Main.tileMoss[x] || TileID.Sets.Conversion.Stone[x]) && x != TileID.Stone) {
				Set(DecleminationId, x, TileID.Stone);
			}
			else if (TileID.Sets.Conversion.Ice[x] && x != TileID.IceBlock) {
				Set(DecleminationId, x, TileID.IceBlock);
			}
			else if (TileID.Sets.Conversion.Sandstone[x] && x != TileID.Sandstone) {
				Set(DecleminationId, x, TileID.Sandstone);
			}
			else if (TileID.Sets.Conversion.HardenedSand[x] && x != TileID.HardenedSand) {
				Set(DecleminationId, x, TileID.HardenedSand);
			}
			else if (TileID.Sets.Conversion.Sand[x] && x != TileID.Sand) {
				Set(DecleminationId, x, TileID.Sand);
			}
			else if (TileID.Sets.Conversion.Thorn[x] && x != TileID.JungleThorns) {
				Set(DecleminationId, x, TileID.JungleThorns);
			}

			// Brown Solution
			if ((TileID.Sets.Conversion.Stone[x] || TileID.Sets.Conversion.Ice[x] || TileID.Sets.Conversion.Sandstone[x]) && x != TileID.Stone) {
				Set(BrownSolutionId, x, TileID.Dirt);
			}
			else if (TileID.Sets.Conversion.GolfGrass[x] && x != TileID.GolfGrass) {
				Set(BrownSolutionId, x, TileID.GolfGrass);
			}
			else if (TileID.Sets.Conversion.Grass[x] && x != TileID.Grass && x != TileID.GolfGrass) {
				Set(BrownSolutionId, x, TileID.Grass);
			}

			// White Solution
			if ((TileID.Sets.Conversion.Grass[x] || TileID.Sets.Conversion.Sand[x] || TileID.Sets.Conversion.HardenedSand[x] || TileID.Sets.Conversion.Snow[x] || TileID.Sets.Conversion.Dirt[x]) && x != TileID.SnowBlock) {
				Set(WhiteSolutionId, x, TileID.SnowBlock);
			}
			else if ((Main.tileMoss[x] || TileID.Sets.Conversion.Stone[x] || TileID.Sets.Conversion.Ice[x] || TileID.Sets.Conversion.Sandstone[x]) && x != TileID.IceBlock) {
				Set(WhiteSolutionId, x, TileID.IceBlock);
			}

			// TODO: Add Yellow Solution
		}

		// Purity
		Set(DecleminationId, TileID.Stone, TileID.Stone);
		Set(DecleminationId, TileID.Grass, TileID.Grass);
		Set(DecleminationId, TileID.GolfGrass, TileID.GolfGrass);
		Set(DecleminationId, TileID.JungleGrass, TileID.JungleGrass);
		Set(DecleminationId, TileID.IceBlock, TileID.IceBlock);
		Set(DecleminationId, TileID.Sand, TileID.Sand);
		Set(DecleminationId, TileID.HardenedSand, TileID.HardenedSand);
		Set(DecleminationId, TileID.Sandstone, TileID.Sandstone);
		Set(DecleminationId, TileID.JungleThorns, TileID.JungleThorns);

		Set(DecleminationId, TileID.CorruptThorns, TileID.JungleThorns);
		Set(DecleminationId, TileID.CrimsonThorns, TileID.JungleThorns);

		// Mushroom
		Set(DarkBlueSolutionId, TileID.JungleGrass, TileID.MushroomGrass);
		Set(DecleminationId, TileID.MushroomGrass, TileID.JungleGrass);

		ForeachTileAltBiome();
	}

	private void ForeachTileAltBiome() {
		var union = UniteAltBiomes().ToArray();
		foreach (IAltBiome u in union) {
			var data = u.DataHandler.Get<ConversionData>();
			for (int x = 0; x < TileLoader.TileCount; x++) {
				int t = 5 + u.Type;

				if (TileID.Sets.Conversion.GolfGrass[x] && data.MowedGrass != Keep && x != data.MowedGrass) {
					Set(t, x, data.MowedGrass);
				}
				else if (TileID.Sets.Conversion.Grass[x] && data.Grass != Keep && x != data.Grass) {
					Set(t, x, data.Grass);
				}
				else if (TileID.Sets.Conversion.JungleGrass[x] && data.JungleGrass != Keep && x != data.JungleGrass) {
					Set(t, x, data.JungleGrass);
				}
				else if ((Main.tileMoss[x] || TileID.Sets.Conversion.Stone[x]) && data.Stone != Keep && x != data.Stone) {
					Set(t, x, data.Stone);
				}
				else if (TileID.Sets.Conversion.Snow[x] && data.Snow != Keep && x != data.Snow) {
					Set(t, x, data.Snow);
				}
				else if (TileID.Sets.Conversion.Ice[x] && data.Ice != Keep && x != data.Ice) {
					Set(t, x, data.Ice);
				}
				else if (TileID.Sets.Conversion.Sandstone[x] && data.Sandstone != Keep && x != data.Sandstone) {
					Set(t, x, data.Sandstone);
				}
				else if (TileID.Sets.Conversion.HardenedSand[x] && data.HardSand != Keep && x != data.HardSand) {
					Set(t, x, data.HardSand);
				}
				else if (TileID.Sets.Conversion.Sand[x] && data.Sand != Keep && x != data.Sand) {
					Set(t, x, data.Sand);
				}
				else if (TileID.Sets.Conversion.Thorn[x] && data.ThornBush != Keep && x != TileID.JungleThorns) {
					Set(t, x, data.ThornBush);
				}
			}
		}
	}
}
