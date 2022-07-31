using AltLibrary.Common;
using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.AltOres;
using AltLibrary.Common.Hooks;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static AltLibrary.Common.UIWorldCreationEdits;

namespace AltLibrary.Core.Baking
{
	internal class ALWorldCreationLists
	{
		internal static OreCreationList prehmOreData;
		internal static BiomeCreationList biomeData;

		internal class ALWorldCreationLists_Loader : ILoadable
		{
			public void Load(Mod mod)
			{
				prehmOreData = new();
				biomeData = new();

				List<int> ores = new()
				{
					ItemID.CopperOre, ItemID.TinOre,
					ItemID.IronOre, ItemID.LeadOre,
					ItemID.SilverOre, ItemID.TungstenOre,
					ItemID.GoldOre, ItemID.PlatinumOre
				};

				for (int i = 0; i < prehmOreData.Types.FindIndex(x => x.ore == ItemID.CobaltOre); i++)
				{
					ores.Add(prehmOreData.Types[i].ore);
				}

				AltOreInsideBodies.ores = ores;
			}

			public void Unload()
			{
				prehmOreData = null;
				biomeData = null;

				AltOreInsideBodies.ores = null;
			}
		}

		public static void FillData()
		{
			prehmOreData.Initialize();
			biomeData.Initialize();
		}

		internal class BiomeCreationList : WorldCreationList<AltBiome>
		{
			internal List<ALDrawingStruct<AltBiome>> Quenes;

			public override void Initialize()
			{
				List<ALDrawingStruct<AltBiome>> quene = new()
				{
#region Hallow
                    new("Terraria/Hallow", (value) =>
			{
				return AltHallowBiomeChosenType >= 0
					? ModContent.Request<Texture2D>(AltLibrary.Biomes[AltHallowBiomeChosenType].IconSmall
													?? "AltLibrary/Assets/Menu/ButtonHallow", AssetRequestMode.ImmediateLoad)
					: value;
			},
			() =>
			{
				return AltHallowBiomeChosenType switch
				{
					< 0 => new(30, 30, 30, 30),
					_ => null
				};
			},
			() =>
			{
				return AltHallowBiomeChosenType < 0
					? Language.GetTextValue("Mods.AltLibrary.AltBiomeName.HallowBiome")
					: AltLibrary.Biomes[AltHallowBiomeChosenType].Name;
			},
			(mod) =>
			{
				return AltHallowBiomeChosenType switch
				{
					>= 0 => AltLibrary.Biomes[AltHallowBiomeChosenType].Mod.Name,
					_ => mod
				};
			},
			() =>
			{
				bool isAlt = AltLibraryConfig.Config.VanillaShowUpIfOnlyAltVarExist == true;
				if (isAlt) isAlt &= AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Hallow && x.Selectable).Any();
				if (AltLibraryConfig.Config.VanillaShowUpIfOnlyAltVarExist == false)
				{
					isAlt = true;
				}
				return (chosenOption == CurrentAltOption.Biome || AltLibraryConfig.Config.BiomeIconsVisibleOutsideBiomeUI) && isAlt;
			}),
                    #endregion
                    #region Evil
                    new("Terraria/Evil", (value) =>
			{
				return AltEvilBiomeChosenType >= 0
					? ModContent.Request<Texture2D>(AltLibrary.Biomes[AltEvilBiomeChosenType].IconSmall
													?? "AltLibrary/Assets/Menu/ButtonCorrupt", AssetRequestMode.ImmediateLoad)
					: value;
			}, () =>
			{
				return AltEvilBiomeChosenType switch
				{
					> -666 and <= -333 => new(210, 0, 30, 30),
					<= -666 => new(360, 0, 30, 30),
					_ => null
				};
			}, () =>
			{
				if (AltEvilBiomeChosenType == -333) return Language.GetTextValue("Mods.AltLibrary.AltBiomeName.CorruptBiome");
				if (AltEvilBiomeChosenType == -666) return Language.GetTextValue("Mods.AltLibrary.AltBiomeName.CrimsonBiome");
				return AltLibrary.Biomes[AltEvilBiomeChosenType].Name;
			}, (mod) =>
			{
				return AltEvilBiomeChosenType switch
				{
					>= 0 => AltLibrary.Biomes[AltEvilBiomeChosenType].Mod.Name,
					_ => mod
				};
			},
			() => chosenOption == CurrentAltOption.Biome || AltLibraryConfig.Config.BiomeIconsVisibleOutsideBiomeUI),
                    #endregion
                    #region Underworld
                    new("Terraria/Underworld",
					(value) =>
			{
				return AltHellBiomeChosenType >= 0
					? ModContent.Request<Texture2D>(AltLibrary.Biomes[AltHellBiomeChosenType].IconSmall
													?? "AltLibrary/Assets/Menu/ButtonHell", AssetRequestMode.ImmediateLoad)
					: value;
			}, () =>
			{
				return AltHellBiomeChosenType switch
				{
					< 0 => new(30, 60, 30, 30),
					_ => null
				};
			}, () =>
			{
				return AltHellBiomeChosenType < 0
					? Language.GetTextValue("Mods.AltLibrary.AltBiomeName.UnderworldBiome")
					: AltLibrary.Biomes[AltHellBiomeChosenType].Name;
			}, (mod) =>
			{
				return AltHellBiomeChosenType switch
				{
					>= 0 => AltLibrary.Biomes[AltHellBiomeChosenType].Mod.Name,
					_ => mod
				};
			},
			() =>
			{
				bool isAlt = AltLibraryConfig.Config.VanillaShowUpIfOnlyAltVarExist == true;
				if (isAlt) isAlt &= AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Hell && x.Selectable).Any();
				if (AltLibraryConfig.Config.VanillaShowUpIfOnlyAltVarExist == false)
				{
					isAlt = true;
				}
				return (chosenOption == CurrentAltOption.Biome || AltLibraryConfig.Config.BiomeIconsVisibleOutsideBiomeUI) && isAlt;
			}),
                    #endregion
                    #region Jungle
                    new("Terraria/Jungle",
					(value) =>
			{
				return AltJungleBiomeChosenType >= 0
					? ModContent.Request<Texture2D>(AltLibrary.Biomes[AltJungleBiomeChosenType].IconSmall
													?? "AltLibrary/Assets/Menu/ButtonJungle", AssetRequestMode.ImmediateLoad)
					: value;
			}, () =>
			{
				return AltJungleBiomeChosenType switch
				{
					< 0 => new(180, 30, 30, 30),
					_ => null
				};
			}, () =>
			{
				return AltJungleBiomeChosenType < 0
					? Language.GetTextValue("Mods.AltLibrary.AltBiomeName.JungleBiome")
					: AltLibrary.Biomes[AltJungleBiomeChosenType].Name;
			}, (mod) =>
			{
				return AltJungleBiomeChosenType switch
				{
					>= 0 => AltLibrary.Biomes[AltJungleBiomeChosenType].Mod.Name,
					_ => mod
				};
			},
					() =>
			{
				bool isAlt = AltLibraryConfig.Config.VanillaShowUpIfOnlyAltVarExist == true;
				if (isAlt) isAlt &= AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Jungle && x.Selectable).Any();
				if (AltLibraryConfig.Config.VanillaShowUpIfOnlyAltVarExist == false)
				{
					isAlt = true;
				}
				return (chosenOption == CurrentAltOption.Biome || AltLibraryConfig.Config.BiomeIconsVisibleOutsideBiomeUI) && isAlt;
			}),
#endregion
                };
				foreach (AltBiome ore in AltLibrary.Biomes)
				{
					ore.AddBiomeOnScreenIcon(quene);
				}
				Quenes = quene;
			}
		}

		internal class OreCreationList : WorldCreationList<AltOre>
		{
			internal List<ALDrawingStruct<AltOre>> Quenes;

			public override void Initialize()
			{
				List<AltOre> preOrder = new()
			{
				new VanillaOre("Copper", "Copper", -1, TileID.Copper, ItemID.CopperBar, OreType.Copper),
				new VanillaOre("Tin", "Tin", -2, TileID.Tin, ItemID.TinBar, OreType.Copper),
				new VanillaOre("Iron", "Iron", -3, TileID.Iron, ItemID.IronBar, OreType.Iron),
				new VanillaOre("Lead", "Lead", -4, TileID.Lead, ItemID.LeadBar, OreType.Iron),
				new VanillaOre("Silver", "Silver", -5, TileID.Silver, ItemID.SilverBar, OreType.Silver),
				new VanillaOre("Tungsten", "Tungsten", -6, TileID.Tungsten, ItemID.TungstenBar, OreType.Silver),
				new VanillaOre("Gold", "Gold", -7, TileID.Gold, ItemID.GoldBar, OreType.Gold),
				new VanillaOre("Platinum", "Platinum", -8, TileID.Platinum, ItemID.PlatinumBar, OreType.Gold),
				new VanillaOre("Cobalt", "Cobalt", -9, TileID.Cobalt, ItemID.CobaltBar, OreType.Cobalt),
				new VanillaOre("Palladium", "Palladium", -10, TileID.Palladium, ItemID.PalladiumBar, OreType.Cobalt),
				new VanillaOre("Mythril", "Mythril", -11, TileID.Mythril, ItemID.MythrilBar, OreType.Mythril),
				new VanillaOre("Orichalcum", "Orichalcum", -12, TileID.Orichalcum, ItemID.OrichalcumBar, OreType.Mythril),
				new VanillaOre("Adamantite", "Adamantite", -13, TileID.Adamantite, ItemID.AdamantiteBar, OreType.Adamantite),
				new VanillaOre("Titanium", "Titanium", -14, TileID.Titanium, ItemID.TitaniumBar, OreType.Adamantite)
			};
				foreach (AltOre ore in AltLibrary.Ores)
				{
					ore.CustomSelection(preOrder);
				}
				Types = preOrder;

				List<ALDrawingStruct<AltOre>> quene = new()
			{
                #region Pre-HM Ores
#region Copper
                new("Terraria/Copper", (value) =>
			{
				return Copper switch
				{
					>= 0 => ModContent.Request<Texture2D>(AltLibrary.Ores[Copper - 1].Texture),
					_ => value
				};
			}, () =>
			{
				return Copper switch
				{
					-1 => new(0, 0, 30, 30),
					-2 => new(30, 0, 30, 30),
					_ => null,
				};
			}, () =>
			{
				return Copper switch
				{
					-1 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Copper"),
					-2 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Tin"),
					_ => AltLibrary.Ores[Copper - 1].Name
				};
			}, (mod) =>
			{
				return Copper switch
				{
					>= 0 => AltLibrary.Ores[Copper - 1].Mod.Name,
					_ => mod
				};
			}),
                #endregion
#region Iron
                new("Terraria/Iron",
				(value) =>
				{
return Iron switch
{
						>= 0 => ModContent.Request<Texture2D>(AltLibrary.Ores[Iron - 1].Texture),
						_ => value
					};
				}, () =>
{
					return Iron switch
					{
						-3 => new(60, 0, 30, 30),
						-4 => new(90, 0, 30, 30),
						_ => null,
					};
				}, () =>
{
					return Iron switch
					{
						-3 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Iron"),
						-4 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Lead"),
_ => AltLibrary.Ores[Iron - 1].Name
					};
				}, (mod) =>
				{
					return Iron switch
{
						>= 0 => AltLibrary.Ores[Iron - 1].Mod.Name,
_ => mod
					};
				}),
                #endregion
#region Silver
                new("Terraria/Silver",
				(value) =>
				{
					return Silver switch
					{
						>= 0 => ModContent.Request<Texture2D>(AltLibrary.Ores[Silver - 1].Texture),
						_ => value
					};
				}, () =>
				{
					return Silver switch
					{
						-5 => new(120, 0, 30, 30),
						-6 => new(150, 0, 30, 30),
						_ => null,
					};
				}, () =>
				{
					return Silver switch
					{
						-5 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Silver"),
						-6 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Tungsten"),
						_ => AltLibrary.Ores[Silver - 1].Name
					};
				}, (mod) =>
				{
					return Silver switch
					{
						>= 0 => AltLibrary.Ores[Silver - 1].Mod.Name,
						_ => mod
					};
				}),
                #endregion
#region Gold
                new("Terraria/Gold",
				(value) =>
				{
					return Gold switch
					{
						>= 0 => ModContent.Request<Texture2D>(AltLibrary.Ores[Gold - 1].Texture),
						_ => value
					};
				}, () =>
				{
					return Gold switch
					{
						-7 => new(180, 0, 30, 30),
						-8 => new(210, 0, 30, 30),
						_ => null,
					};
				}, () =>
				{
					return Gold switch
					{
						-7 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Gold"),
						-8 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Platinum"),
						_ => AltLibrary.Ores[Gold - 1].Name
					};
				}, (mod) =>
				{
					return Gold switch
					{
						>= 0 => AltLibrary.Ores[Gold - 1].Mod.Name,
						_ => mod
					};
				}),
                #endregion
                #endregion
                #region HM Ores
#region Cobalt
                new("Terraria/Cobalt",
				(value) =>
				{
					return Cobalt switch
					{
						>= 0 => ModContent.Request<Texture2D>(AltLibrary.Ores[Cobalt - 1].Texture),
						_ => value
					};
				}, () =>
				{
					return Cobalt switch
					{
						-9 => new(0, 30, 30, 30),
						-10 => new(30, 30, 30, 30),
						_ => null,
					};
				}, () =>
				{
					return Cobalt switch
					{
						-9 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Cobalt"),
						-10 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Palladium"),
						_ => AltLibrary.Ores[Cobalt - 1].Name
					};
				}, (mod) =>
				{
					return Cobalt switch
					{
						>= 0 => AltLibrary.Ores[Cobalt - 1].Mod.Name,
						_ => mod
					};
				}),
                #endregion
#region Mythril
                new("Terraria/Mythril",
				(value) =>
				{
					return Mythril switch
					{
						>= 0 => ModContent.Request<Texture2D>(AltLibrary.Ores[Mythril - 1].Texture),
						_ => value
					};
				}, () =>
				{
					return Mythril switch
					{
						-11 => new(60, 30, 30, 30),
						-12 => new(90, 30, 30, 30),
						_ => null,
					};
				}, () =>
				{
					return Mythril switch
					{
						-11 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Mythril"),
						-12 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Orichalcum"),
						_ => AltLibrary.Ores[Mythril - 1].Name
					};
				}, (mod) =>
				{
					return Mythril switch
					{
						>= 0 => AltLibrary.Ores[Mythril - 1].Mod.Name,
						_ => mod
					};
				}),
                #endregion
#region Adamantite
                new("Terraria/Adamantite",
				(value) =>
				{
					return Adamantite switch
					{
						>= 0 => ModContent.Request<Texture2D>(AltLibrary.Ores[Adamantite - 1].Texture),
						_ => value
					};
				}, () =>
				{
					return Adamantite switch
					{
						-13 => new(120, 30, 30, 30),
						-14 => new(150, 30, 30, 30),
						_ => null,
					};
				}, () =>
				{
					return Adamantite switch
					{
						-13 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Adamantite"),
						-14 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Titanium"),
						_ => AltLibrary.Ores[Adamantite - 1].Name
					};
				}, (mod) =>
				{
					return Adamantite switch
					{
						>= 0 => AltLibrary.Ores[Adamantite - 1].Mod.Name,
						_ => mod
					};
				})
#endregion
                #endregion
            };
				foreach (AltOre ore in AltLibrary.Ores)
				{
					ore.AddOreOnScreenIcon(quene);
				}
				Quenes = quene;
			}
		}

		internal abstract class WorldCreationList<T>
		{
			public List<T> Types = new();

			public abstract void Initialize();
		}
	}
}
