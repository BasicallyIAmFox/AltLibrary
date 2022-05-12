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
            GenPasses.HookGenPassCorruption += GenPasses_HookGenPassCorruption;
            GenPasses.HookGenPassAltars += ILGenPassAltars;
            GenPasses.HookGenPassMicroBiomes += GenPasses_HookGenPassMicroBiomes;
        }

        private static void GenPasses_HookGenPassMicroBiomes(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchLdstr("LivingTreeCount")))
                return;
            if (!c.TryGotoPrev(i => i.MatchCallvirt<GenerationProgress>("Set")))
                return;

            var label = il.DefineLabel();

            c.Index++;
            c.Emit(OpCodes.Ldsfld, typeof(WorldBiomeManager).GetField(nameof(WorldBiomeManager.worldJungle), BindingFlags.Public | BindingFlags.Static));
            c.Emit(OpCodes.Ldstr, "");
            c.Emit(OpCodes.Call, typeof(string).GetMethod("op_Equality", new Type[] { typeof(string), typeof(string) }));
            c.Emit(OpCodes.Brfalse_S, label);

            if (!c.TryGotoNext(i => i.MatchLdstr("..Long Minecart Tracks")))
                return;
            if (!c.TryGotoPrev(i => i.MatchLdarg(1)))
                return;

            c.MarkLabel(label);
        }

        private static void GenPasses_HookGenPassCorruption(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchCall<WorldGen>(nameof(WorldGen.CrimStart))))
                return;

            #region Crimson
            if (!c.TryGotoNext(i => i.MatchLdcI4(60)))
                return;

            c.Index++;
            c.Emit(OpCodes.Pop);
            c.EmitDelegate(() =>
            {
                foreach (AltBiome biome in AltLibrary.Biomes)
                {
                    if (WorldBiomeManager.worldJungle == biome.FullName && biome.BiomeType == BiomeType.Jungle && biome.BiomeGrass.HasValue)
                        return biome.BiomeGrass.Value;
                }
                return 60;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(234)))
                return;

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.Crimsand;
                    int value2 = TileID.Ebonsand;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeSand ?? TileID.Sand) : TileID.Sand;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(199)))
                return;

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.CrimsonGrass;
                    int value2 = TileID.CorruptGrass;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeGrass ?? TileID.Grass) : TileID.Grass;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(203)))
                return;

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.Crimstone;
                    int value2 = TileID.Ebonstone;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeStone ?? TileID.Stone) : TileID.Stone;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(199)))
                return;

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.CrimsonGrass;
                    int value2 = TileID.CorruptGrass;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeGrass ?? TileID.Grass) : TileID.Grass;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(200)))
                return;

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = orig;
                    int value2 = TileID.CorruptIce;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeIce ?? TileID.IceBlock) : TileID.IceBlock;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(401)))
                return;

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.CrimsonSandstone;
                    int value2 = TileID.CorruptSandstone;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeSandstone ?? TileID.Sandstone) : TileID.Sandstone;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(399)))
                return;

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.CrimsonHardenedSand;
                    int value2 = TileID.CorruptHardenedSand;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeHardenedSand ?? TileID.HardenedSand) : TileID.HardenedSand;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });
            #endregion

            #region Corrupt
            if (!c.TryGotoNext(i => i.MatchLdcI4(60)))
                return;

            c.Index++;
            c.Emit(OpCodes.Pop);
            c.EmitDelegate(() =>
            {
                foreach (AltBiome biome in AltLibrary.Biomes)
                {
                    if (WorldBiomeManager.worldJungle == biome.FullName && biome.BiomeType == BiomeType.Jungle && biome.BiomeGrass.HasValue)
                        return biome.BiomeGrass.Value;
                }
                return 60;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(112)))
                return;

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.Crimsand;
                    int value2 = TileID.Ebonsand;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeSand ?? TileID.Sand) : TileID.Sand;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(23)))
                return;

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.CrimsonGrass;
                    int value2 = TileID.CorruptGrass;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeGrass ?? TileID.Grass) : TileID.Grass;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(25)))
                return;

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.Crimstone;
                    int value2 = TileID.Ebonstone;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeStone ?? TileID.Stone) : TileID.Stone;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(23)))
                return;

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.CrimsonGrass;
                    int value2 = TileID.CorruptGrass;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeGrass ?? TileID.Grass) : TileID.Grass;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(163)))
                return;

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.FleshIce;
                    int value2 = TileID.CorruptIce;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeIce ?? TileID.IceBlock) : TileID.IceBlock;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(400)))
                return;

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.CrimsonSandstone;
                    int value2 = TileID.CorruptSandstone;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeSand ?? TileID.Sandstone) : TileID.Sandstone;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(398)))
                return;

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.CrimsonHardenedSand;
                    int value2 = TileID.CorruptHardenedSand;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeHardenedSand ?? TileID.HardenedSand) : TileID.HardenedSand;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });
            #endregion
        }

        public static void ILGenPassAltars(ILContext il)
        {
            ILCursor c = new(il);
            ILLabel endNormalAltar = c.DefineLabel();
            ILLabel startNormalAltar = c.DefineLabel();
            if (!c.TryGotoNext(i => i.MatchLdsfld<WorldGen>(nameof(WorldGen.crimson))))
                return;
            c.EmitDelegate(() => WorldGen.crimson || WorldBiomeManager.worldEvil != "");
            c.Emit(OpCodes.Brfalse, startNormalAltar);
            c.Emit(OpCodes.Ldloc, 3);
            c.Emit(OpCodes.Ldloc, 4);
            c.EmitDelegate((int x, int y) =>
            {
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).AltarTile.HasValue)
                {
                    if (!WorldGen.IsTileNearby(x, y, ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).AltarTile.Value, 3))
                    {
                        WorldGen.Place3x2(x, y, (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).AltarTile.Value);
                    }
                }
            });
            c.Emit(OpCodes.Br, endNormalAltar);
            c.MarkLabel(startNormalAltar);
            if (!c.TryGotoNext(i => i.MatchLdloc(5)))
                return;
            if (!c.TryGotoNext(i => i.MatchLdsflda(out _)))
                return;
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
                return;
            if (!c.TryGotoPrev(i => i.MatchLdcI4(166)))
                return;
            if (!c.TryGotoPrev(i => i.MatchLdcI4(166)))
                return;
            if (!c.TryGotoNext(i => i.MatchStfld(out copper)))
                return;
            if (!c.TryGotoNext(i => i.MatchLdcI4(167)))
                return;
            if (!c.TryGotoNext(i => i.MatchStfld(out iron)))
                return;
            if (!c.TryGotoNext(i => i.MatchLdcI4(168)))
                return;
            if (!c.TryGotoNext(i => i.MatchStfld(out silver)))
                return;
            if (!c.TryGotoNext(i => i.MatchLdcI4(169)))
                return;
            if (!c.TryGotoNext(i => i.MatchStfld(out gold)))
                return;
            if (!c.TryGotoNext(i => i.MatchStsfld(typeof(WorldGen).GetField(nameof(WorldGen.crimson), BindingFlags.Public | BindingFlags.Static))))
                return;
            if (!c.TryGotoNext(i => i.MatchRet()))
                return;
            if (!c.TryGotoPrev(i => i.MatchBneUn(out _)))
                return;
            if (!c.TryGotoPrev(i => i.MatchLdcI4(-1)))
                return;

            c.EmitDelegate<Func<int, int>>(dungeonSide =>
            {
                WorldBiomeGeneration.dungeonSide = dungeonSide;
                return dungeonSide;
            });

            replaceValues(nameof(WorldBiomeManager.Copper), (-1, TileID.Copper), (-2, TileID.Tin), copper);
            replaceValues(nameof(WorldBiomeManager.Iron), (-3, TileID.Iron), (-4, TileID.Lead), iron);
            replaceValues(nameof(WorldBiomeManager.Silver), (-5, TileID.Silver), (-6, TileID.Tungsten), silver);
            replaceValues(nameof(WorldBiomeManager.Gold), (-7, TileID.Gold), (-8, TileID.Platinum), gold);

            void replaceValues(string type, ValueTuple<int, int> value1, ValueTuple<int, int> value2, FieldReference field)
            {
                var label = c.DefineLabel();
                var label2 = c.DefineLabel();
                var label3 = c.DefineLabel();
                c.Emit(OpCodes.Ldsfld, typeof(WorldBiomeManager).GetField(type, BindingFlags.Public | BindingFlags.Static));
                c.Emit(OpCodes.Ldc_I4, value1.Item1);
                c.Emit(OpCodes.Bne_Un_S, label);
                c.Emit(OpCodes.Ldarg, 0);
                c.EmitDelegate(() => value1.Item2);
                c.Emit(OpCodes.Stfld, field);
                c.Emit(OpCodes.Br_S, label3);
                c.MarkLabel(label);
                c.Emit(OpCodes.Ldsfld, typeof(WorldBiomeManager).GetField(type, BindingFlags.Public | BindingFlags.Static));
                c.Emit(OpCodes.Ldc_I4, value2.Item1);
                c.Emit(OpCodes.Bne_Un_S, label2);
                c.Emit(OpCodes.Ldarg, 0);
                c.EmitDelegate(() => value2.Item2);
                c.Emit(OpCodes.Stfld, field);
                c.Emit(OpCodes.Br_S, label3);
                c.MarkLabel(label2);
                c.Emit(OpCodes.Ldarg, 0);
                c.EmitDelegate(() => AltLibrary.Ores[(int)typeof(WorldBiomeManager).GetField(type, BindingFlags.Public | BindingFlags.Static).GetValue(null) - 1].ore);
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
                    return;
                if (!c.TryGotoNext(i => i.MatchLdcI4(2)))
                    return;
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldc_I4, 1);
                if (!c.TryGotoNext(i => i.MatchLdcI4(7)))
                    return;
                c.Index++;
                c.EmitDelegate<Func<int, int>>((value) =>
                {
                    List<int> list = new();
                    list.Add(7);
                    list.Add(166);
                    AltLibrary.Ores.Where(x => x.OreType == OreType.Copper)
                                   .ToList()
                                   .ForEach(x => list.Add(x.ore));
                    return list[WorldGen.genRand.Next(list.Count)];
                });
            }

            for (int j = 0; j < 3; j++)
            {
                if (!c.TryGotoNext(i => i.MatchLdsfld<WorldGen>(nameof(WorldGen.drunkWorldGen))))
                    return;
                if (!c.TryGotoNext(i => i.MatchLdcI4(2)))
                    return;
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldc_I4, 1);
                if (!c.TryGotoNext(i => i.MatchLdcI4(6)))
                    return;
                c.Index++;
                c.EmitDelegate<Func<int, int>>((value) =>
                {
                    List<int> list = new();
                    list.Add(6);
                    list.Add(167);
                    AltLibrary.Ores.Where(x => x.OreType == OreType.Iron)
                                   .ToList()
                                   .ForEach(x => list.Add(x.ore));
                    return list[WorldGen.genRand.Next(list.Count)];
                });
            }

            for (int j = 0; j < 3; j++)
            {
                if (!c.TryGotoNext(i => i.MatchLdsfld<WorldGen>(nameof(WorldGen.drunkWorldGen))))
                    return;
                if (!c.TryGotoNext(i => i.MatchLdcI4(2)))
                    return;
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldc_I4, 1);
                if (!c.TryGotoNext(i => i.MatchLdcI4(9)))
                    return;
                c.Index++;
                c.EmitDelegate<Func<int, int>>((value) =>
                {
                    List<int> list = new();
                    list.Add(9);
                    list.Add(168);
                    AltLibrary.Ores.Where(x => x.OreType == OreType.Silver)
                                   .ToList()
                                   .ForEach(x => list.Add(x.ore));
                    return list[WorldGen.genRand.Next(list.Count)];
                });
            }

            for (int j = 0; j < 2; j++)
            {
                if (!c.TryGotoNext(i => i.MatchLdsfld<WorldGen>(nameof(WorldGen.drunkWorldGen))))
                    return;
                if (!c.TryGotoNext(i => i.MatchLdcI4(2)))
                    return;
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldc_I4, 1);
                if (!c.TryGotoNext(i => i.MatchLdcI4(8)))
                    return;
                c.Index++;
                c.EmitDelegate<Func<int, int>>((value) =>
                {
                    List<int> list = new();
                    list.Add(8);
                    list.Add(169);
                    AltLibrary.Ores.Where(x => x.OreType == OreType.Gold)
                                   .ToList()
                                   .ForEach(x => list.Add(x.ore));
                    return list[WorldGen.genRand.Next(list.Count)];
                });
            }

            if (!c.TryGotoNext(i => i.MatchRet()))
                return;
            if (!c.TryGotoPrev(i => i.MatchLdsfld<WorldGen>(nameof(WorldGen.drunkWorldGen))))
                return;
            if (!c.TryGotoNext(i => i.MatchBrfalse(out _)))
                return;
            c.Index++;
            c.EmitDelegate(() =>
            {
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeOre.HasValue)
                {
                    for (int i = 0; i < (int)(Main.maxTilesX * Main.maxTilesY * 2.25E-05 / 2.0); i++)
                    {
                        WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX),
                            WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY), WorldGen.genRand.Next(3, 6),
                            WorldGen.genRand.Next(4, 8), ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeOre.Value);
                    }
                }
            });
            if (!c.TryGotoNext(i => i.MatchRet()))
                return;
            if (!c.TryGotoNext(i => i.MatchBr(out _)))
                return;
            ILLabel startCorruptionGen = c.DefineLabel();
            c.EmitDelegate(() => !WorldGen.crimson && WorldBiomeManager.worldEvil != "");
            c.Emit(OpCodes.Brfalse, startCorruptionGen);
            c.EmitDelegate(() =>
            {
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeOre.HasValue)
                {
                    for (int i = 0; i < (int)(Main.maxTilesX * Main.maxTilesY * 2.25E-05); i++)
                    {
                        WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX),
                            WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY), WorldGen.genRand.Next(3, 6),
                            WorldGen.genRand.Next(4, 8), ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeOre.Value);
                    }
                }
            });
            c.Emit(OpCodes.Ret);
            c.MarkLabel(startCorruptionGen);
        }
    }
}
