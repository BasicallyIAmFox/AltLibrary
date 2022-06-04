using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AltLibrary.Common.Hooks
{
    [Autoload(Side = ModSide.Both)]
    internal static class HardmodeWorldGen
    {
        private static string evilAlt;
        private static string hallowAlt;

        public static void Init()
        {
            evilAlt = "";
            hallowAlt = "";
            IL.Terraria.WorldGen.smCallBack += GenPasses.ILSMCallBack;
            IL.Terraria.WorldGen.GERunner += WorldGen_GERunner;
            On.Terraria.WorldGen.GERunner += WorldGen_GERunner1;
            GenPasses.HookGenPassHardmodeWalls += GenPasses_HookGenPassHardmodeWalls;
        }

        public static void Unload()
        {
            evilAlt = null;
            hallowAlt = null;
            IL.Terraria.WorldGen.smCallBack -= GenPasses.ILSMCallBack;
            IL.Terraria.WorldGen.GERunner -= WorldGen_GERunner;
            On.Terraria.WorldGen.GERunner -= WorldGen_GERunner1;
            GenPasses.HookGenPassHardmodeWalls -= GenPasses_HookGenPassHardmodeWalls;
        }

        private static void WorldGen_GERunner1(On.Terraria.WorldGen.orig_GERunner orig, int i, int j, float speedX, float speedY, bool good)
        {
            if (Main.drunkWorld && WorldBiomeGeneration.WofKilledTimes > 1)
            {
                List<string> possibles = new();
                AltLibrary.Biomes.FindAll(x => x.BiomeType == BiomeType.Hallow).ForEach(x => possibles.Add(x.FullName));
                possibles.Add("");
                hallowAlt = Main.rand.Next(possibles);
                possibles = new();
                AltLibrary.Biomes.FindAll(x => x.BiomeType == BiomeType.Evil).ForEach(x => possibles.Add(x.FullName));
                possibles.Add("Terraria/Corruption");
                possibles.Add("Terraria/Crimson");
                evilAlt = Main.rand.Next(possibles);

                int addX = WorldGen.genRand.Next(300, 400) * WorldBiomeGeneration.WofKilledTimes;
                if (!good) addX *= -1;
                i += addX;
                if (i < 0)
                {
                    i *= -1;
                }
                if (i > Main.maxTilesX)
                {
                    i %= Main.maxTilesX;
                }
            }
            orig(i, j, speedX, speedY, good);
        }

        private static void GenPasses_HookGenPassHardmodeWalls(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchStloc(8)))
            {
                AltLibrary.Instance.Logger.Info("h $ 1");
                return;
            }
            if (!c.TryGotoNext(i => i.MatchLdloc(4)))
            {
                AltLibrary.Instance.Logger.Info("h $ 2");
                return;
            }
            c.Index++;
            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldloc, 7);
            c.Emit(OpCodes.Ldloc, 5);
            c.EmitDelegate<Func<int, Tile, int>>((orig, tile) =>
            {
                if (WorldBiomeManager.WorldHallow != "" && WorldBiomeGeneration.WofKilledTimes <= 1 &&
                    Find<AltBiome>(WorldBiomeManager.WorldHallow).HardmodeWalls.Count > 0 &&
                    ((Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeGrass.HasValue &&
                        tile.TileType == Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeGrass.Value) ||
                    (Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeStone.HasValue &&
                        tile.TileType == Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeStone.Value) ||
                    (Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeSand.HasValue &&
                        tile.TileType == Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeSand.Value) ||
                    (Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeIce.HasValue &&
                        tile.TileType == Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeIce.Value) ||
                    (Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeHardenedSand.HasValue &&
                        tile.TileType == Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeHardenedSand.Value) ||
                    (Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeSandstone.HasValue &&
                        tile.TileType == Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeSandstone.Value))
                    // Vine here!
                    )
                {
                    orig = WorldGen.genRand.Next(Find<AltBiome>(WorldBiomeManager.WorldHallow).HardmodeWalls);
                }
                if (WorldBiomeManager.WorldEvil != "" && WorldBiomeGeneration.WofKilledTimes <= 1 &&
                    Find<AltBiome>(WorldBiomeManager.WorldEvil).HardmodeWalls.Count > 0 &&
                    ((Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeGrass.HasValue &&
                        tile.TileType == Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeGrass.Value) ||
                    (Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeStone.HasValue &&
                        tile.TileType == Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeStone.Value) ||
                    (Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSand.HasValue &&
                        tile.TileType == Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSand.Value) ||
                    (Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeIce.HasValue &&
                        tile.TileType == Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeIce.Value) ||
                    (Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeHardenedSand.HasValue &&
                        tile.TileType == Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeHardenedSand.Value) ||
                    (Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSandstone.HasValue &&
                        tile.TileType == Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSandstone.Value))
                    // Vine here!
                    )
                {
                    orig = WorldGen.genRand.Next(Find<AltBiome>(WorldBiomeManager.WorldEvil).HardmodeWalls);
                }

                if (hallowAlt != "" && WorldBiomeGeneration.WofKilledTimes > 1 &&
                    Find<AltBiome>(hallowAlt).HardmodeWalls.Count > 0 &&
                    ((Find<AltBiome>(hallowAlt).BiomeGrass.HasValue &&
                        tile.TileType == Find<AltBiome>(hallowAlt).BiomeGrass.Value) ||
                    (Find<AltBiome>(hallowAlt).BiomeStone.HasValue &&
                        tile.TileType == Find<AltBiome>(hallowAlt).BiomeStone.Value) ||
                    (Find<AltBiome>(hallowAlt).BiomeSand.HasValue &&
                        tile.TileType == Find<AltBiome>(hallowAlt).BiomeSand.Value) ||
                    (Find<AltBiome>(hallowAlt).BiomeIce.HasValue &&
                        tile.TileType == Find<AltBiome>(hallowAlt).BiomeIce.Value) ||
                    (Find<AltBiome>(hallowAlt).BiomeHardenedSand.HasValue &&
                        tile.TileType == Find<AltBiome>(hallowAlt).BiomeHardenedSand.Value) ||
                    (Find<AltBiome>(hallowAlt).BiomeSandstone.HasValue &&
                        tile.TileType == Find<AltBiome>(hallowAlt).BiomeSandstone.Value))
                    // Vine here!
                    )
                {
                    orig = WorldGen.genRand.Next(Find<AltBiome>(WorldBiomeManager.WorldHallow).HardmodeWalls);
                }
                if (!evilAlt.StartsWith("Terraria/") && WorldBiomeGeneration.WofKilledTimes > 1 &&
                    Find<AltBiome>(evilAlt).HardmodeWalls.Count > 0 &&
                    ((Find<AltBiome>(evilAlt).BiomeGrass.HasValue &&
                        tile.TileType == Find<AltBiome>(evilAlt).BiomeGrass.Value) ||
                    (Find<AltBiome>(evilAlt).BiomeStone.HasValue &&
                        tile.TileType == Find<AltBiome>(evilAlt).BiomeStone.Value) ||
                    (Find<AltBiome>(evilAlt).BiomeSand.HasValue &&
                        tile.TileType == Find<AltBiome>(evilAlt).BiomeSand.Value) ||
                    (Find<AltBiome>(evilAlt).BiomeIce.HasValue &&
                        tile.TileType == Find<AltBiome>(evilAlt).BiomeIce.Value) ||
                    (Find<AltBiome>(evilAlt).BiomeHardenedSand.HasValue &&
                        tile.TileType == Find<AltBiome>(evilAlt).BiomeHardenedSand.Value) ||
                    (Find<AltBiome>(evilAlt).BiomeSandstone.HasValue &&
                        tile.TileType == Find<AltBiome>(evilAlt).BiomeSandstone.Value))
                    // Vine here!
                    )
                {
                    orig = WorldGen.genRand.Next(Find<AltBiome>(evilAlt).HardmodeWalls);
                }
                return orig;
            });
            c.Emit(OpCodes.Stloc, 7);
            c.Emit(OpCodes.Ldloc, 4);
        }

        private static void WorldGen_GERunner(ILContext il)
        {
            ILCursor c = new(il);
            int good = 0;
            if (!c.TryGotoNext(i => i.MatchBrfalse(out _)))
            {
                AltLibrary.Instance.Logger.Info("i $ 1");
                return;
            }
            if (!c.TryGotoNext(i => i.MatchBrfalse(out _)))
            {
                AltLibrary.Instance.Logger.Info("i $ 2");
                return;
            }
            if (!c.TryGotoPrev(i => i.MatchLdarg(out good)))
            {
                AltLibrary.Instance.Logger.Info("i $ 3");
                return;
            }
            if (!c.TryGotoPrev(i => i.MatchBgeUn(out _)))
            {
                AltLibrary.Instance.Logger.Info("i $ 4");
                return;
            }

            c.Index++;
            c.Emit(OpCodes.Ldarg, good);
            c.Emit(OpCodes.Ldloc, 15);
            c.Emit(OpCodes.Ldloc, 16);
            c.EmitDelegate<Action<bool, int, int>>((good, m, l) =>
            {
                if (!good)
                {
                    Tile tile = Main.tile[m, l];
                    if (WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldEvil != "")
                    {
                        foreach (KeyValuePair<int, int> entry in Find<AltBiome>(WorldBiomeManager.WorldEvil).SpecialConversion)
                        {
                            if (tile.TileType == entry.Key)
                            {
                                tile = Main.tile[m, l];
                                tile.TileType = (ushort)entry.Value;
                                WorldGen.SquareTileFrame(m, l, true);
                            }
                        }
                        foreach (KeyValuePair<ushort, ushort> entry in Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement)
                        {
                            if (tile.WallType == entry.Key)
                            {
                                tile.WallType = entry.Value;
                            }
                        }
                    }
                }
            });

            if (!c.TryGotoNext(i => i.MatchBrfalse(out _)))
            {
                AltLibrary.Instance.Logger.Info("i $ 5");
                return;
            }

            c.Index++;
            c.Emit(OpCodes.Ldloc, 15);
            c.Emit(OpCodes.Ldloc, 16);
            c.EmitDelegate<Action<int, int>>((m, l) =>
            {
                Tile tile = Main.tile[m, l];
                if (WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldHallow != "")
                {
                    foreach (KeyValuePair<int, int> entry in Find<AltBiome>(WorldBiomeManager.WorldHallow).SpecialConversion)
                    {
                        if (tile.TileType == entry.Key)
                        {
                            tile.TileType = (ushort)entry.Value;
                            WorldGen.SquareTileFrame(m, l, true);
                        }
                    }
                    foreach (KeyValuePair<ushort, ushort> entry in Find<AltBiome>(WorldBiomeManager.WorldHallow).WallContext.wallsReplacement)
                    {
                        if (tile.WallType == entry.Key)
                        {
                            tile.WallType = entry.Value;
                        }
                    }
                }
            });

            ALUtils.ReplaceIDs(il,
                WallID.HallowedGrassUnsafe,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldHallow).WallContext.wallsReplacement.TryGetValue(orig, out ushort value) ? value : WallID.None,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldHallow != "" && Find<AltBiome>(WorldBiomeManager.WorldHallow).WallContext.wallsReplacement.ContainsKey(orig),
                (orig) =>
                {
                    if (WorldBiomeGeneration.WofKilledTimes > 1 && hallowAlt != "" && Find<AltBiome>(hallowAlt).WallContext.wallsReplacement.ContainsKey(orig))
                    {
                        Find<AltBiome>(hallowAlt).WallContext.wallsReplacement.TryGetValue(orig, out ushort value);
                        return value;
                    }
                    return orig;
                });
            ALUtils.ReplaceIDs(il,
                WallID.HallowHardenedSand,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldHallow).WallContext.wallsReplacement.TryGetValue(orig, out ushort value) ? value : WallID.None,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldHallow != "" && Find<AltBiome>(WorldBiomeManager.WorldHallow).WallContext.wallsReplacement.ContainsKey(orig),
                (orig) =>
                {
                    if (WorldBiomeGeneration.WofKilledTimes > 1 && hallowAlt != "" && Find<AltBiome>(hallowAlt).WallContext.wallsReplacement.ContainsKey(orig))
                    {
                        Find<AltBiome>(hallowAlt).WallContext.wallsReplacement.TryGetValue(orig, out ushort value);
                        return value;
                    }
                    return orig;
                });
            ALUtils.ReplaceIDs(il,
                WallID.HallowSandstone,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldHallow).WallContext.wallsReplacement.TryGetValue(orig, out ushort value) ? value : WallID.None,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldHallow != "" && Find<AltBiome>(WorldBiomeManager.WorldHallow).WallContext.wallsReplacement.ContainsKey(orig),
                (orig) =>
                {
                    if (WorldBiomeGeneration.WofKilledTimes > 1 && hallowAlt != "" && Find<AltBiome>(hallowAlt).WallContext.wallsReplacement.ContainsKey(orig))
                    {
                        Find<AltBiome>(hallowAlt).WallContext.wallsReplacement.TryGetValue(orig, out ushort value);
                        return value;
                    }
                    return orig;
                });
            ALUtils.ReplaceIDs(il,
                WallID.PearlstoneBrickUnsafe,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldHallow).WallContext.wallsReplacement.TryGetValue(orig, out ushort value) ? value : WallID.None,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldHallow != "" && Find<AltBiome>(WorldBiomeManager.WorldHallow).WallContext.wallsReplacement.ContainsKey(orig),
                (orig) =>
                {
                    if (WorldBiomeGeneration.WofKilledTimes > 1 && hallowAlt != "" && Find<AltBiome>(hallowAlt).WallContext.wallsReplacement.ContainsKey(orig))
                    {
                        Find<AltBiome>(hallowAlt).WallContext.wallsReplacement.TryGetValue(orig, out ushort value);
                        return value;
                    }
                    return orig;
                });
            ALUtils.ReplaceIDs(il,
                WallID.CorruptGrassUnsafe,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.TryGetValue(orig, out ushort value) ? value : WallID.None,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.ContainsKey(orig),
                (orig) =>
                {
                    if (WorldBiomeGeneration.WofKilledTimes > 1 && !evilAlt.StartsWith("Terraria/") && Find<AltBiome>(evilAlt).WallContext.wallsReplacement.ContainsKey(orig))
                    {
                        Find<AltBiome>(evilAlt).WallContext.wallsReplacement.TryGetValue(orig, out ushort value);
                        return value;
                    }
                    return orig;
                });
            ALUtils.ReplaceIDs(il,
                WallID.CorruptHardenedSand,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.TryGetValue(orig, out ushort value) ? value : WallID.None,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.ContainsKey(orig),
                (orig) =>
                {
                    if (WorldBiomeGeneration.WofKilledTimes > 1 && !evilAlt.StartsWith("Terraria/") && Find<AltBiome>(evilAlt).WallContext.wallsReplacement.ContainsKey(orig))
                    {
                        Find<AltBiome>(evilAlt).WallContext.wallsReplacement.TryGetValue(orig, out ushort value);
                        return value;
                    }
                    return orig;
                });
            ALUtils.ReplaceIDs(il,
                WallID.CorruptSandstone,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.TryGetValue(orig, out ushort value) ? value : WallID.None,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.ContainsKey(orig),
                (orig) =>
                {
                    if (WorldBiomeGeneration.WofKilledTimes > 1 && !evilAlt.StartsWith("Terraria/") && Find<AltBiome>(evilAlt).WallContext.wallsReplacement.ContainsKey(orig))
                    {
                        Find<AltBiome>(evilAlt).WallContext.wallsReplacement.TryGetValue(orig, out ushort value);
                        return value;
                    }
                    return orig;
                });
            ALUtils.ReplaceIDs(il,
                WallID.CrimsonGrassUnsafe,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.TryGetValue(orig, out ushort value) ? value : WallID.None,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.ContainsKey(69),
                (orig) =>
                {
                    if (WorldBiomeGeneration.WofKilledTimes > 1 && !evilAlt.StartsWith("Terraria/") && Find<AltBiome>(evilAlt).WallContext.wallsReplacement.ContainsKey(69))
                    {
                        Find<AltBiome>(evilAlt).WallContext.wallsReplacement.TryGetValue(69, out ushort value);
                        return value;
                    }
                    return orig;
                });
            ALUtils.ReplaceIDs(il,
                WallID.CrimsonHardenedSand,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.TryGetValue(orig, out ushort value) ? value : WallID.None,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.ContainsKey(217),
                (orig) =>
                {
                    if (WorldBiomeGeneration.WofKilledTimes > 1 && !evilAlt.StartsWith("Terraria/") && Find<AltBiome>(evilAlt).WallContext.wallsReplacement.ContainsKey(217))
                    {
                        Find<AltBiome>(evilAlt).WallContext.wallsReplacement.TryGetValue(217, out ushort value);
                        return value;
                    }
                    return orig;
                });
            ALUtils.ReplaceIDs(il,
                WallID.CrimsonSandstone,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.TryGetValue(orig, out ushort value) ? value : WallID.None,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.ContainsKey(220),
                (orig) =>
                {
                    if (WorldBiomeGeneration.WofKilledTimes > 1 && !evilAlt.StartsWith("Terraria/") && Find<AltBiome>(evilAlt).WallContext.wallsReplacement.ContainsKey(220))
                    {
                        Find<AltBiome>(evilAlt).WallContext.wallsReplacement.TryGetValue(220, out ushort value);
                        return value;
                    }
                    return orig;
                });

            ALUtils.ReplaceIDs<int>(il,
                TileID.Pearlstone,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeStone ?? orig,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldHallow != "" && Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeStone.HasValue,
                (orig) =>
                {
                    return WorldBiomeGeneration.WofKilledTimes > 1 && hallowAlt != "" && Find<AltBiome>(hallowAlt).BiomeStone.HasValue
                        ? Find<AltBiome>(hallowAlt).BiomeStone.Value
                        : orig;
                });
            ALUtils.ReplaceIDs<int>(il,
                TileID.HallowHardenedSand,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeHardenedSand ?? orig,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldHallow != "" && Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeHardenedSand.HasValue,
                (orig) =>
                {
                    return WorldBiomeGeneration.WofKilledTimes > 1 && hallowAlt != "" && Find<AltBiome>(hallowAlt).BiomeHardenedSand.HasValue
                        ? Find<AltBiome>(hallowAlt).BiomeHardenedSand.Value
                        : orig;
                });
            ALUtils.ReplaceIDs<int>(il,
                TileID.HallowedGrass,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeGrass ?? orig,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldHallow != "" && Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeGrass.HasValue,
                (orig) =>
                {
                    return WorldBiomeGeneration.WofKilledTimes > 1 && hallowAlt != "" && Find<AltBiome>(hallowAlt).BiomeGrass.HasValue
                        ? Find<AltBiome>(hallowAlt).BiomeGrass.Value
                        : orig;
                });
            ALUtils.ReplaceIDs<int>(il,
                TileID.Pearlsand,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeSand ?? orig,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldHallow != "" && Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeSand.HasValue,
                (orig) =>
                {
                    return WorldBiomeGeneration.WofKilledTimes > 1 && hallowAlt != "" && Find<AltBiome>(hallowAlt).BiomeSand.HasValue
                        ? Find<AltBiome>(hallowAlt).BiomeSand.Value
                        : orig;
                });
            ALUtils.ReplaceIDs<int>(il,
                TileID.HallowedIce,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeIce ?? orig,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldHallow != "" && Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeIce.HasValue,
                (orig) =>
                {
                    return WorldBiomeGeneration.WofKilledTimes > 1 && hallowAlt != "" && Find<AltBiome>(hallowAlt).BiomeIce.HasValue
                        ? Find<AltBiome>(hallowAlt).BiomeIce.Value
                        : orig;
                });
            ALUtils.ReplaceIDs<int>(il,
                TileID.HallowSandstone,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeSandstone ?? orig,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldHallow != "" && Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeSandstone.HasValue,
                (orig) =>
                {
                    return WorldBiomeGeneration.WofKilledTimes > 1 && hallowAlt != "" && Find<AltBiome>(hallowAlt).BiomeSandstone.HasValue
                        ? Find<AltBiome>(hallowAlt).BiomeSandstone.Value
                        : orig;
                });

            ALUtils.ReplaceIDs<int>(il,
                TileID.Ebonstone,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeStone ?? orig,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeStone.HasValue,
                (orig) =>
                {
                    if (WorldBiomeGeneration.WofKilledTimes > 1 && evilAlt.EndsWith("Crimson")) return TileID.Crimstone;
                    return WorldBiomeGeneration.WofKilledTimes > 1 && !evilAlt.StartsWith("Terraria/") && Find<AltBiome>(evilAlt).BiomeStone.HasValue
                        ? Find<AltBiome>(evilAlt).BiomeStone.Value
                        : orig;
                });
            ALUtils.ReplaceIDs<int>(il,
                TileID.CorruptHardenedSand,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeHardenedSand ?? orig,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeHardenedSand.HasValue,
                (orig) =>
                {
                    if (WorldBiomeGeneration.WofKilledTimes > 1 && evilAlt.EndsWith("Crimson")) return TileID.CrimsonHardenedSand;
                    return WorldBiomeGeneration.WofKilledTimes > 1 && !evilAlt.StartsWith("Terraria/") && Find<AltBiome>(evilAlt).BiomeHardenedSand.HasValue
                        ? Find<AltBiome>(evilAlt).BiomeHardenedSand.Value
                        : orig;
                });
            ALUtils.ReplaceIDs<int>(il,
                TileID.CorruptGrass,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeGrass ?? orig,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeGrass.HasValue,
                (orig) =>
                {
                    if (WorldBiomeGeneration.WofKilledTimes > 1 && evilAlt.EndsWith("Crimson")) return TileID.CrimsonGrass;
                    return WorldBiomeGeneration.WofKilledTimes > 1 && !evilAlt.StartsWith("Terraria/") && Find<AltBiome>(evilAlt).BiomeGrass.HasValue
                        ? Find<AltBiome>(evilAlt).BiomeGrass.Value
                        : orig;
                });
            ALUtils.ReplaceIDs<int>(il,
                TileID.Ebonsand,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSand ?? orig,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSand.HasValue,
                (orig) =>
                {
                    if (WorldBiomeGeneration.WofKilledTimes > 1 && evilAlt.EndsWith("Crimson")) return TileID.Crimsand;
                    return WorldBiomeGeneration.WofKilledTimes > 1 && !evilAlt.StartsWith("Terraria/") && Find<AltBiome>(evilAlt).BiomeSand.HasValue
                        ? Find<AltBiome>(evilAlt).BiomeSand.Value
                        : orig;
                });
            ALUtils.ReplaceIDs<int>(il,
                TileID.CorruptIce,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeIce ?? orig,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeIce.HasValue,
                (orig) =>
                {
                    if (WorldBiomeGeneration.WofKilledTimes > 1 && evilAlt.EndsWith("Crimson")) return TileID.FleshIce;
                    return WorldBiomeGeneration.WofKilledTimes > 1 && !evilAlt.StartsWith("Terraria/") && Find<AltBiome>(evilAlt).BiomeIce.HasValue
                        ? Find<AltBiome>(evilAlt).BiomeIce.Value
                        : orig;
                });
            ALUtils.ReplaceIDs<int>(il,
                TileID.CorruptSandstone,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSandstone ?? orig,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSandstone.HasValue,
                (orig) =>
                {
                    if (WorldBiomeGeneration.WofKilledTimes > 1 && evilAlt.EndsWith("Crimson")) return TileID.CrimsonSandstone;
                    return WorldBiomeGeneration.WofKilledTimes > 1 && !evilAlt.StartsWith("Terraria/") && Find<AltBiome>(evilAlt).BiomeSandstone.HasValue
                        ? Find<AltBiome>(evilAlt).BiomeSandstone.Value
                        : orig;
                });

            ALUtils.ReplaceIDs<int>(il,
                TileID.Crimstone,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeStone ?? orig,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeStone.HasValue,
                (orig) =>
                {
                    if (WorldBiomeGeneration.WofKilledTimes > 1 && evilAlt.EndsWith("Corruption")) return TileID.Ebonsand;
                    return WorldBiomeGeneration.WofKilledTimes > 1 && !evilAlt.StartsWith("Terraria/") && Find<AltBiome>(evilAlt).BiomeStone.HasValue
                        ? Find<AltBiome>(evilAlt).BiomeStone.Value
                        : orig;
                });
            ALUtils.ReplaceIDs<int>(il,
                TileID.CrimsonHardenedSand,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeHardenedSand ?? orig,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeHardenedSand.HasValue,
                (orig) =>
                {
                    if (WorldBiomeGeneration.WofKilledTimes > 1 && evilAlt.EndsWith("Corruption")) return TileID.CorruptHardenedSand;
                    return WorldBiomeGeneration.WofKilledTimes > 1 && !evilAlt.StartsWith("Terraria/") && Find<AltBiome>(evilAlt).BiomeHardenedSand.HasValue
                        ? Find<AltBiome>(evilAlt).BiomeHardenedSand.Value
                        : orig;
                });
            ALUtils.ReplaceIDs<int>(il,
                TileID.CrimsonGrass,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeGrass ?? orig,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeGrass.HasValue,
                (orig) =>
                {
                    if (WorldBiomeGeneration.WofKilledTimes > 1 && evilAlt.EndsWith("Corruption")) return TileID.CorruptGrass;
                    return WorldBiomeGeneration.WofKilledTimes > 1 && !evilAlt.StartsWith("Terraria/") && Find<AltBiome>(evilAlt).BiomeGrass.HasValue
                        ? Find<AltBiome>(evilAlt).BiomeGrass.Value
                        : orig;
                });
            ALUtils.ReplaceIDs<int>(il,
                TileID.Crimsand,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSand ?? orig,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSand.HasValue,
                (orig) =>
                {
                    if (WorldBiomeGeneration.WofKilledTimes > 1 && evilAlt.EndsWith("Corruption")) return TileID.Ebonsand;
                    return WorldBiomeGeneration.WofKilledTimes > 1 && !evilAlt.StartsWith("Terraria/") && Find<AltBiome>(evilAlt).BiomeSand.HasValue
                        ? Find<AltBiome>(evilAlt).BiomeSand.Value
                        : orig;
                });
            ALUtils.ReplaceIDs<int>(il,
                TileID.FleshIce,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeIce ?? orig,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeIce.HasValue,
                (orig) =>
                {
                    if (WorldBiomeGeneration.WofKilledTimes > 1 && evilAlt.EndsWith("Corruption")) return TileID.CorruptIce;
                    return WorldBiomeGeneration.WofKilledTimes > 1 && !evilAlt.StartsWith("Terraria/") && Find<AltBiome>(evilAlt).BiomeIce.HasValue
                        ? Find<AltBiome>(evilAlt).BiomeIce.Value
                        : orig;
                });
            ALUtils.ReplaceIDs<int>(il,
                TileID.CrimsonSandstone,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSandstone ?? orig,
                (orig) => WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSandstone.HasValue,
                (orig) =>
                {
                    if (WorldBiomeGeneration.WofKilledTimes > 1 && evilAlt.EndsWith("Corruption")) return TileID.CorruptSandstone;
                    return WorldBiomeGeneration.WofKilledTimes > 1 && !evilAlt.StartsWith("Terraria/") && Find<AltBiome>(evilAlt).BiomeSandstone.HasValue
                        ? Find<AltBiome>(evilAlt).BiomeSandstone.Value
                        : orig;
                });
        }
    }
}
