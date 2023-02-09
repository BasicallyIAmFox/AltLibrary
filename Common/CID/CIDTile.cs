using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Data;
using AltLibrary.Content.Groups;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common.CID;

public sealed class CIDTile : CIData {
	public override void Bake() {
		for (int x = 0; x < TileLoader.TileCount; x++) {
			if (TileID.Sets.Conversion.GolfGrass[x] && x != TileID.GolfGrass) {
				tiles[DecleminationId][x] = TileID.GolfGrass;
			}
			else if (TileID.Sets.Conversion.Grass[x] && x != TileID.Grass) {
				tiles[DecleminationId][x] = TileID.Grass;
			}
			else if (TileID.Sets.Conversion.JungleGrass[x] && x != TileID.JungleGrass) {
				tiles[DecleminationId][x] = TileID.JungleGrass;
			}
			else if ((Main.tileMoss[x] || TileID.Sets.Conversion.Stone[x]) && x != TileID.Stone) {
				tiles[DecleminationId][x] = TileID.Stone;
			}
			else if (TileID.Sets.Conversion.Ice[x] && x != TileID.IceBlock) {
				tiles[DecleminationId][x] = TileID.IceBlock;
			}
			else if (TileID.Sets.Conversion.Sandstone[x] && x != TileID.Sandstone) {
				tiles[DecleminationId][x] = TileID.Sandstone;
			}
			else if (TileID.Sets.Conversion.HardenedSand[x] && x != TileID.HardenedSand) {
				tiles[DecleminationId][x] = TileID.HardenedSand;
			}
			else if (TileID.Sets.Conversion.Sand[x] && x != TileID.Sand) {
				tiles[DecleminationId][x] = TileID.Sand;
			}

			// Brown Solution
			if ((TileID.Sets.Conversion.Stone[x] || TileID.Sets.Conversion.Ice[x] || TileID.Sets.Conversion.Sandstone[x]) && x != TileID.Stone) {
				tiles[BrownSolutionId][x] = TileID.Stone;
			}
			else if (TileID.Sets.Conversion.GolfGrass[x] && x != TileID.GolfGrass) {
				tiles[BrownSolutionId][x] = TileID.GolfGrass;
			}
			else if (TileID.Sets.Conversion.Grass[x] && x != TileID.Grass && x != TileID.GolfGrass) {
				tiles[BrownSolutionId][x] = TileID.Grass;
			}

			// White Solution
			if ((TileID.Sets.Conversion.Grass[x] || TileID.Sets.Conversion.Sand[x] || TileID.Sets.Conversion.HardenedSand[x] || TileID.Sets.Conversion.Snow[x] || TileID.Sets.Conversion.Dirt[x]) && x != TileID.SnowBlock) {
				tiles[WhiteSolutionId][x] = TileID.SnowBlock;
			}
			else if ((Main.tileMoss[x] || TileID.Sets.Conversion.Stone[x] || TileID.Sets.Conversion.Ice[x] || TileID.Sets.Conversion.Sandstone[x]) && x != TileID.IceBlock) {
				tiles[WhiteSolutionId][x] = TileID.SnowBlock;
			}

			// TODO: Add Yellow Solution
		}

		// Purity
		tiles[DecleminationId][TileID.Stone] = TileID.Stone;
		tiles[DecleminationId][TileID.Grass] = TileID.Grass;
		tiles[DecleminationId][TileID.GolfGrass] = TileID.GolfGrass;
		tiles[DecleminationId][TileID.JungleGrass] = TileID.JungleGrass;
		tiles[DecleminationId][TileID.IceBlock] = TileID.IceBlock;
		tiles[DecleminationId][TileID.Sand] = TileID.Sand;
		tiles[DecleminationId][TileID.HardenedSand] = TileID.HardenedSand;
		tiles[DecleminationId][TileID.Sandstone] = TileID.Sandstone;
		tiles[DecleminationId][TileID.JungleThorns] = TileID.JungleThorns;

		tiles[DecleminationId][TileID.CorruptThorns] = TileID.JungleThorns;
		tiles[DecleminationId][TileID.CrimsonThorns] = TileID.JungleThorns;

		// Mushroom
		tiles[DarkBlueSolutionId][TileID.JungleGrass] = TileID.MushroomGrass;
		tiles[DecleminationId][TileID.MushroomGrass] = TileID.JungleGrass;

		ForeachTileAltBiome();
	}

	private void ForeachTileAltBiome() {
		var union = UniteAltBiomes();
		foreach (IAltBiome u in union) {
			var data = u.DataHandler.Get<ConversionData>();
			for (int x = 0; x < TileLoader.TileCount; x++) {
				int t = 5 + u.Type;

				if (TileID.Sets.Conversion.GolfGrass[x] && data.MowedGrass != Keep && x != data.MowedGrass) {
					tiles[t][x] = data.MowedGrass;
				}
				else if (TileID.Sets.Conversion.Grass[x] && data.Grass != Keep && x != data.Grass) {
					tiles[t][x] = data.Grass;
				}
				else if (TileID.Sets.Conversion.JungleGrass[x] && data.JungleGrass != Keep && x != data.JungleGrass) {
					tiles[t][x] = data.JungleGrass;
				}
				else if ((Main.tileMoss[x] || TileID.Sets.Conversion.Stone[x]) && data.Stone != Keep && x != data.Stone) {
					tiles[t][x] = data.Stone;
				}
				else if (TileID.Sets.Conversion.Snow[x] && data.Snow != Keep && x != data.Snow) {
					tiles[t][x] = data.Snow;
				}
				else if (TileID.Sets.Conversion.Ice[x] && data.Ice != Keep && x != data.Ice) {
					tiles[t][x] = data.Ice;
				}
				else if (TileID.Sets.Conversion.Sandstone[x] && data.Sandstone != Keep && x != data.Sandstone) {
					tiles[t][x] = data.Sandstone;
				}
				else if (TileID.Sets.Conversion.HardenedSand[x] && data.HardSand != Keep && x != data.HardSand) {
					tiles[t][x] = data.HardSand;
				}
				else if (TileID.Sets.Conversion.Sand[x] && data.Sand != Keep && x != data.Sand) {
					tiles[t][x] = data.Sand;
				}
			}
		}
	}
}
