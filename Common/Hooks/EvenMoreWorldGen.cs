using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace AltLibrary.Common.Hooks
{
	internal static class EvenMoreWorldGen
	{
		public static void Init()
		{
			IL.Terraria.WorldGen.GenerateWorld += GenPasses.ILGenerateWorld;
			GenPasses.HookGenPassReset += GenPasses_HookGenPassReset;
			GenPasses.HookGenPassShinies += GenPasses_HookGenPassShinies;
			GenPasses.HookGenPassUnderworld += GenPasses_HookGenPassUnderworld;
			GenPasses.HookGenPassAltars += ILGenPassAltars;
			GenPasses.HookGenPassMicroBiomes += GenPasses_HookGenPassMicroBiomes;
			IL.Terraria.GameContent.Biomes.MiningExplosivesBiome.Place += MiningExplosivesBiome_Place;
		}

		public static void Unload()
		{
			IL.Terraria.WorldGen.GenerateWorld -= GenPasses.ILGenerateWorld;
			GenPasses.HookGenPassReset -= GenPasses_HookGenPassReset;
			GenPasses.HookGenPassShinies -= GenPasses_HookGenPassShinies;
			GenPasses.HookGenPassUnderworld -= GenPasses_HookGenPassUnderworld;
			GenPasses.HookGenPassAltars -= ILGenPassAltars;
			GenPasses.HookGenPassMicroBiomes -= GenPasses_HookGenPassMicroBiomes;
			IL.Terraria.GameContent.Biomes.MiningExplosivesBiome.Place -= MiningExplosivesBiome_Place;
		}

		private static void MiningExplosivesBiome_Place(ILContext il)
		{
			ILCursor c = new(il);

			if (!c.TryGotoNext(i => i.MatchStloc(0)))
			{
				AltLibrary.Instance.Logger.Info("12 $ 1");
				return;
			}

			c.Index++;
			c.Emit(OpCodes.Ldloc, 0);
			c.EmitDelegate<Func<ushort, ushort>>((type) =>
			{
				type = Utils.SelectRandom(WorldGen.genRand, new ushort[]
				{
					(ushort)WorldGen.SavedOreTiers.Gold,
					(ushort)WorldGen.SavedOreTiers.Silver,
					(ushort)WorldGen.SavedOreTiers.Iron,
					(ushort)WorldGen.SavedOreTiers.Copper,
				});
				return type;
			});
			c.Emit(OpCodes.Stloc, 0);
		}

		private static void GenPasses_HookGenPassUnderworld(ILContext il)
		{
			ILCursor c = new(il);

			if (!c.TryGotoNext(i => i.MatchCallvirt<GenerationProgress>("set_Message")))
			{
				AltLibrary.Instance.Logger.Info("d $ 1");
				return;
			}

			c.Index++;
			c.Emit(OpCodes.Ldarg, 1);
			c.EmitDelegate<Action<GenerationProgress>>((progress) =>
			{
				if (WorldBiomeManager.WorldHell != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).GenPassName != null)
				{
					progress.Message = ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).GenPassName.GetTranslation(Language.ActiveCulture);
				}
			});

			ALUtils.ReplaceIDs(il, TileID.Ash,
				(orig) => (ushort?)ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).BiomeStone ?? orig,
				(orig) => WorldBiomeManager.WorldHell != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).BiomeStone.HasValue);
			ALUtils.ReplaceIDs(il, TileID.Hellstone,
				(orig) => (ushort?)ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).BiomeOre ?? orig,
				(orig) => WorldBiomeManager.WorldHell != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).BiomeOre.HasValue);

			if (!c.TryGotoNext(i => i.MatchCall<WorldGen>(nameof(WorldGen.AddHellHouses))))
			{
				AltLibrary.Instance.Logger.Info("d $ 2");
				return;
			}

			ILLabel label = c.DefineLabel();
			c.EmitDelegate(() => WorldBiomeManager.WorldHell == "");
			c.Emit(OpCodes.Brfalse_S, label);
			c.Index++;
			c.MarkLabel(label);
		}

		private static void GenPasses_HookGenPassMicroBiomes(ILContext il)
		{
			ILCursor c = new(il);
			if (!c.TryGotoNext(i => i.MatchLdstr("LivingTreeCount")))
			{
				AltLibrary.Instance.Logger.Info("c $ 1");
				return;
			}
			if (!c.TryGotoPrev(i => i.MatchCallvirt<GenerationProgress>("Set")))
			{
				AltLibrary.Instance.Logger.Info("c $ 2");
				return;
			}

			var label = il.DefineLabel();

			c.Index++;
			c.EmitDelegate(() => WorldBiomeManager.WorldJungle == "");
			c.Emit(OpCodes.Brfalse_S, label);

			if (!c.TryGotoNext(i => i.MatchLdstr("..Long Minecart Tracks")))
			{
				AltLibrary.Instance.Logger.Info("c $ 3");
				return;
			}
			if (!c.TryGotoPrev(i => i.MatchLdarg(1)))
			{
				AltLibrary.Instance.Logger.Info("c $ 4");
				return;
			}

			c.MarkLabel(label);
		}

		public static void ILGenPassAltars(ILContext il)
		{
			ILCursor c = new(il);
			ILLabel endNormalAltar = c.DefineLabel();
			ILLabel startNormalAltar = c.DefineLabel();
			if (!c.TryGotoNext(i => i.MatchLdsfld<WorldGen>(nameof(WorldGen.crimson))))
			{
				AltLibrary.Instance.Logger.Info("e $ 1");
				return;
			}
			c.EmitDelegate(() => WorldGen.crimson || WorldBiomeManager.WorldEvil != "");
			c.Emit(OpCodes.Brfalse, startNormalAltar);
			c.Emit(OpCodes.Ldloc, 3);
			c.Emit(OpCodes.Ldloc, 4);
			c.EmitDelegate((int x, int y) =>
			{
				if (WorldBiomeManager.WorldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).AltarTile.HasValue)
				{
					if (!WorldGen.IsTileNearby(x, y, ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).AltarTile.Value, 3))
					{
						WorldGen.Place3x2(x, y, (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).AltarTile.Value);
					}
				}
			});
			c.Emit(OpCodes.Br, endNormalAltar);
			c.MarkLabel(startNormalAltar);
			if (!c.TryGotoNext(i => i.MatchLdloc(5)))
			{
				AltLibrary.Instance.Logger.Info("e $ 2");
				return;
			}
			if (!c.TryGotoNext(i => i.MatchLdsflda(out _)))
			{
				AltLibrary.Instance.Logger.Info("e $ 3");
				return;
			}
			c.MarkLabel(endNormalAltar);
		}

		private static void GenPasses_HookGenPassReset(ILContext il)
		{
			ILCursor c = new(il);
			FieldReference copper = null;
			FieldReference iron = null;
			FieldReference silver = null;
			FieldReference gold = null;

			if (!c.TryGotoNext(i => i.MatchStsfld(typeof(WorldGen).GetField(nameof(WorldGen.crimson), BindingFlags.Public | BindingFlags.Static))))
			{
				AltLibrary.Instance.Logger.Info("f $ 1");
				return;
			}
			if (!c.TryGotoPrev(i => i.MatchLdcI4(166)))
			{
				AltLibrary.Instance.Logger.Info("f $ 2");
				return;
			}
			if (!c.TryGotoPrev(i => i.MatchLdcI4(166)))
			{
				AltLibrary.Instance.Logger.Info("f $ 3");
				return;
			}
			if (!c.TryGotoNext(i => i.MatchStsfld(out copper)))
			{
				AltLibrary.Instance.Logger.Info("f $ 4");
				return;
			}
			if (!c.TryGotoNext(i => i.MatchLdcI4(167)))
			{
				AltLibrary.Instance.Logger.Info("f $ 5");
				return;
			}
			if (!c.TryGotoNext(i => i.MatchStsfld(out iron)))
			{
				AltLibrary.Instance.Logger.Info("f $ 6");
				return;
			}
			if (!c.TryGotoNext(i => i.MatchLdcI4(168)))
			{
				AltLibrary.Instance.Logger.Info("f $ 7");
				return;
			}
			if (!c.TryGotoNext(i => i.MatchStsfld(out silver)))
			{
				AltLibrary.Instance.Logger.Info("f $ 8");
				return;
			}
			if (!c.TryGotoNext(i => i.MatchLdcI4(169)))
			{
				AltLibrary.Instance.Logger.Info("f $ 9");
				return;
			}
			if (!c.TryGotoNext(i => i.MatchStsfld(out gold)))
			{
				AltLibrary.Instance.Logger.Info("f $ 10");
				return;
			}
			if (!c.TryGotoNext(i => i.MatchStsfld(typeof(WorldGen).GetField(nameof(WorldGen.crimson), BindingFlags.Public | BindingFlags.Static))))
			{
				AltLibrary.Instance.Logger.Info("f $ 11");
				return;
			}
			if (!c.TryGotoNext(i => i.MatchRet()))
			{
				AltLibrary.Instance.Logger.Info("f $ 12");
				return;
			}
			if (!c.TryGotoPrev(i => i.MatchBneUn(out _)))
			{
				AltLibrary.Instance.Logger.Info("f $ 13");
				return;
			}
			if (!c.TryGotoPrev(i => i.MatchLdcI4(-1)))
			{
				AltLibrary.Instance.Logger.Info("f $ 14");
				return;
			}

			c.EmitDelegate<Func<int, int>>(dungeonSide =>
			{
				WorldBiomeGeneration.DungeonSide = dungeonSide;
				return dungeonSide;
			});

			replaceValues(() => WorldBiomeManager.Copper, (-1, TileID.Copper), (-2, TileID.Tin), copper);
			replaceValues(() => WorldBiomeManager.Iron, (-3, TileID.Iron), (-4, TileID.Lead), iron);
			replaceValues(() => WorldBiomeManager.Silver, (-5, TileID.Silver), (-6, TileID.Tungsten), silver);
			replaceValues(() => WorldBiomeManager.Gold, (-7, TileID.Gold), (-8, TileID.Platinum), gold);

			for (int i = 0; i < 2; i++)
			{
				if (!c.TryGotoNext(i => i.MatchRet()))
				{
					AltLibrary.Instance.Logger.Info("f $ 15 " + i);
					return;
				}
				c.Index--;
				c.EmitDelegate<Func<int, int>>(dungeonLocation =>
				{
					WorldBiomeGeneration.DungeonLocation = dungeonLocation;
					return dungeonLocation;
				});
				c.Index += 2;
			}

			void replaceValues(Func<int> type, ValueTuple<int, int> value1, ValueTuple<int, int> value2, FieldReference field)
			{
				var label = c.DefineLabel();
				var label2 = c.DefineLabel();
				var label3 = c.DefineLabel();
				c.EmitDelegate(type.Invoke);
				c.Emit(OpCodes.Ldc_I4, value1.Item1);
				c.Emit(OpCodes.Bne_Un_S, label);
				c.Emit(OpCodes.Ldarg, 0);
				c.EmitDelegate(() => value1.Item2);
				c.Emit(OpCodes.Stfld, field);
				c.Emit(OpCodes.Br_S, label3);
				c.MarkLabel(label);
				c.EmitDelegate(type.Invoke);
				c.Emit(OpCodes.Ldc_I4, value2.Item1);
				c.Emit(OpCodes.Bne_Un_S, label2);
				c.Emit(OpCodes.Ldarg, 0);
				c.EmitDelegate(() => value2.Item2);
				c.Emit(OpCodes.Stfld, field);
				c.Emit(OpCodes.Br_S, label3);
				c.MarkLabel(label2);
				c.Emit(OpCodes.Ldarg, 0);
				c.EmitDelegate(() => AltLibrary.Ores[type.Invoke() - 1].ore);
				c.Emit(OpCodes.Stfld, field);
				c.MarkLabel(label3);
			}
		}

		private static void GenPasses_HookGenPassShinies(ILContext il)
		{
			ILCursor c = new(il);
			for (int j = 0; j < 3; j++)
			{
				if (!c.TryGotoNext(i => i.MatchLdsfld<WorldGen>(nameof(WorldGen.drunkWorldGen))))
				{
					AltLibrary.Instance.Logger.Info("g $ 1 " + j);
					return;
				}
				if (!c.TryGotoNext(i => i.MatchLdcI4(2)))
				{
					AltLibrary.Instance.Logger.Info("g $ 2 " + j);
					return;
				}
				c.Index++;
				c.Emit(OpCodes.Pop);
				c.Emit(OpCodes.Ldc_I4, 1);
				if (!c.TryGotoNext(i => i.MatchLdcI4(7)))
				{
					AltLibrary.Instance.Logger.Info("g $ 3 " + j);
					return;
				}
				c.Index++;
				c.EmitDelegate<Func<int, int>>((value) =>
				{
					List<int> list = new()
					{
						7,
						166
					};
					AltLibrary.Ores.Where(x => x.OreType == OreType.Copper)
								   .ToList()
								   .ForEach(x => list.Add(x.ore));
					return WorldGen.genRand.Next(list);
				});
			}

			for (int j = 0; j < 3; j++)
			{
				if (!c.TryGotoNext(i => i.MatchLdsfld<WorldGen>(nameof(WorldGen.drunkWorldGen))))
				{
					AltLibrary.Instance.Logger.Info("g $ 4 " + j);
					return;
				}
				if (!c.TryGotoNext(i => i.MatchLdcI4(2)))
				{
					AltLibrary.Instance.Logger.Info("g $ 5 " + j);
					return;
				}
				c.Index++;
				c.Emit(OpCodes.Pop);
				c.Emit(OpCodes.Ldc_I4, 1);
				if (!c.TryGotoNext(i => i.MatchLdcI4(6)))
				{
					AltLibrary.Instance.Logger.Info("g $ 6 " + j);
					return;
				}
				c.Index++;
				c.EmitDelegate<Func<int, int>>((value) =>
				{
					List<int> list = new()
					{
						6,
						167
					};
					AltLibrary.Ores.Where(x => x.OreType == OreType.Iron)
								   .ToList()
								   .ForEach(x => list.Add(x.ore));
					return WorldGen.genRand.Next(list);
				});
			}

			for (int j = 0; j < 3; j++)
			{
				if (!c.TryGotoNext(i => i.MatchLdsfld<WorldGen>(nameof(WorldGen.drunkWorldGen))))
				{
					AltLibrary.Instance.Logger.Info("g $ 7 " + j);
					return;
				}
				if (!c.TryGotoNext(i => i.MatchLdcI4(2)))
				{
					AltLibrary.Instance.Logger.Info("g $ 8 " + j);
					return;
				}
				c.Index++;
				c.Emit(OpCodes.Pop);
				c.Emit(OpCodes.Ldc_I4, 1);
				if (!c.TryGotoNext(i => i.MatchLdcI4(9)))
				{
					AltLibrary.Instance.Logger.Info("g $ 9 " + j);
					return;
				}
				c.Index++;
				c.EmitDelegate<Func<int, int>>((value) =>
				{
					List<int> list = new()
					{
						9,
						168
					};
					AltLibrary.Ores.Where(x => x.OreType == OreType.Silver)
								   .ToList()
								   .ForEach(x => list.Add(x.ore));
					return WorldGen.genRand.Next(list);
				});
			}

			for (int j = 0; j < 2; j++)
			{
				if (!c.TryGotoNext(i => i.MatchLdsfld<WorldGen>(nameof(WorldGen.drunkWorldGen))))
				{
					AltLibrary.Instance.Logger.Info("g $ 10 " + j);
					return;
				}
				if (!c.TryGotoNext(i => i.MatchLdcI4(2)))
				{
					AltLibrary.Instance.Logger.Info("g $ 11 " + j);
					return;
				}
				c.Index++;
				c.Emit(OpCodes.Pop);
				c.Emit(OpCodes.Ldc_I4, 1);
				if (!c.TryGotoNext(i => i.MatchLdcI4(8)))
				{
					AltLibrary.Instance.Logger.Info("g $ 12 " + j);
					return;
				}
				c.Index++;
				c.EmitDelegate<Func<int, int>>((value) =>
				{
					List<int> list = new()
					{
						8,
						169
					};
					AltLibrary.Ores.Where(x => x.OreType == OreType.Gold)
								   .ToList()
								   .ForEach(x => list.Add(x.ore));
					return WorldGen.genRand.Next(list);
				});
			}

			if (!c.TryGotoNext(i => i.MatchRet()))
			{
				AltLibrary.Instance.Logger.Info("g $ 13");
				return;
			}
			if (!c.TryGotoPrev(i => i.MatchLdsfld<WorldGen>(nameof(WorldGen.drunkWorldGen))))
			{
				AltLibrary.Instance.Logger.Info("g $ 14");
				return;
			}
			if (!c.TryGotoNext(i => i.MatchBrfalse(out _)))
			{
				AltLibrary.Instance.Logger.Info("g $ 15");
				return;
			}
			c.Index++;
			c.EmitDelegate(() =>
			{
				if (WorldBiomeManager.WorldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeOre.HasValue)
				{
					for (int i = 0; i < (int)(Main.maxTilesX * Main.maxTilesY * 2.25E-05 / 2.0); i++)
					{
						WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX),
							WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY), WorldGen.genRand.Next(3, 6),
							WorldGen.genRand.Next(4, 8), ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeOre.Value);
					}
				}
			});
			if (!c.TryGotoNext(i => i.MatchRet()))
			{
				AltLibrary.Instance.Logger.Info("g $ 16");
				return;
			}
			if (!c.TryGotoNext(i => i.MatchBr(out _)))
			{
				AltLibrary.Instance.Logger.Info("g $ 17");
				return;
			}
			ILLabel startCorruptionGen = c.DefineLabel();
			c.EmitDelegate(() => !WorldGen.crimson && WorldBiomeManager.WorldEvil != "");
			c.Emit(OpCodes.Brfalse, startCorruptionGen);
			c.EmitDelegate(() =>
			{
				if (WorldBiomeManager.WorldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeOre.HasValue)
				{
					for (int i = 0; i < (int)(Main.maxTilesX * Main.maxTilesY * 2.25E-05); i++)
					{
						WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX),
							WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY), WorldGen.genRand.Next(3, 6),
							WorldGen.genRand.Next(4, 8), ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeOre.Value);
					}
				}
			});
			c.Emit(OpCodes.Ret);
			c.MarkLabel(startCorruptionGen);
		}
	}
}
