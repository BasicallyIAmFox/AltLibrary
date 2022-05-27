using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AltLibrary.Common.Hooks
{
    [Autoload(Side = ModSide.Both)]
    internal static class HardmodeWorldGen
    {
        public static void Init()
        {
            IL.Terraria.WorldGen.smCallBack += GenPasses.ILSMCallBack;
            IL.Terraria.WorldGen.GERunner += WorldGen_GERunner;
            GenPasses.HookGenPassHardmodeWalls += GenPasses_HookGenPassHardmodeWalls;
        }

        public static void Unload()
        {
            IL.Terraria.WorldGen.smCallBack -= GenPasses.ILSMCallBack;
            IL.Terraria.WorldGen.GERunner -= WorldGen_GERunner;
            GenPasses.HookGenPassHardmodeWalls -= GenPasses_HookGenPassHardmodeWalls;
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
                if (WorldBiomeManager.WorldHallow != "" && Find<AltBiome>(WorldBiomeManager.WorldHallow).HardmodeWalls.Count > 0 &&
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
                    orig = Find<AltBiome>(WorldBiomeManager.WorldHallow).HardmodeWalls[WorldGen.genRand.Next(Find<AltBiome>(WorldBiomeManager.WorldHallow).HardmodeWalls.Count)];
                }
                if (WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).HardmodeWalls.Count > 0 &&
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
                    orig = Find<AltBiome>(WorldBiomeManager.WorldEvil).HardmodeWalls[WorldGen.genRand.Next(Find<AltBiome>(WorldBiomeManager.WorldEvil).HardmodeWalls.Count)];
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
                    if (WorldBiomeManager.WorldEvil != "")
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
                if (WorldBiomeManager.WorldHallow != "")
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
                (orig) => WorldBiomeManager.WorldHallow != "" && Find<AltBiome>(WorldBiomeManager.WorldHallow).WallContext.wallsReplacement.ContainsKey(orig));
            ALUtils.ReplaceIDs(il,
                WallID.HallowHardenedSand,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldHallow).WallContext.wallsReplacement.TryGetValue(orig, out ushort value) ? value : WallID.None,
                (orig) => WorldBiomeManager.WorldHallow != "" && Find<AltBiome>(WorldBiomeManager.WorldHallow).WallContext.wallsReplacement.ContainsKey(orig));
            ALUtils.ReplaceIDs(il,
                WallID.HallowSandstone,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldHallow).WallContext.wallsReplacement.TryGetValue(orig, out ushort value) ? value : WallID.None,
                (orig) => WorldBiomeManager.WorldHallow != "" && Find<AltBiome>(WorldBiomeManager.WorldHallow).WallContext.wallsReplacement.ContainsKey(orig));
            ALUtils.ReplaceIDs(il,
                WallID.PearlstoneBrickUnsafe,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldHallow).WallContext.wallsReplacement.TryGetValue(orig, out ushort value) ? value : WallID.None,
                (orig) => WorldBiomeManager.WorldHallow != "" && Find<AltBiome>(WorldBiomeManager.WorldHallow).WallContext.wallsReplacement.ContainsKey(orig));
            ALUtils.ReplaceIDs(il,
                WallID.CorruptGrassUnsafe,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.TryGetValue(orig, out ushort value) ? value : WallID.None,
                (orig) => WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.ContainsKey(orig));
            ALUtils.ReplaceIDs(il,
                WallID.CorruptHardenedSand,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.TryGetValue(orig, out ushort value) ? value : WallID.None,
                (orig) => WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.ContainsKey(orig));
            ALUtils.ReplaceIDs(il,
                WallID.CorruptSandstone,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.TryGetValue(orig, out ushort value) ? value : WallID.None,
                (orig) => WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.ContainsKey(orig));

            ALUtils.ReplaceIDs<int>(il,
                TileID.Pearlstone,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeStone ?? orig,
                (orig) => WorldBiomeManager.WorldHallow != "" && Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeStone.HasValue);
            ALUtils.ReplaceIDs<int>(il,
                TileID.HallowHardenedSand,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeHardenedSand ?? orig,
                (orig) => WorldBiomeManager.WorldHallow != "" && Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeHardenedSand.HasValue);
            ALUtils.ReplaceIDs<int>(il,
                TileID.HallowedGrass,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeGrass ?? orig,
                (orig) => WorldBiomeManager.WorldHallow != "" && Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeGrass.HasValue);
            ALUtils.ReplaceIDs<int>(il,
                TileID.Pearlsand,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeSand ?? orig,
                (orig) => WorldBiomeManager.WorldHallow != "" && Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeSand.HasValue);
            ALUtils.ReplaceIDs<int>(il,
                TileID.HallowedIce,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeIce ?? orig,
                (orig) => WorldBiomeManager.WorldHallow != "" && Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeIce.HasValue);
            ALUtils.ReplaceIDs<int>(il,
                TileID.HallowSandstone,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeSandstone ?? orig,
                (orig) => WorldBiomeManager.WorldHallow != "" && Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeSandstone.HasValue);

            ALUtils.ReplaceIDs<int>(il,
                TileID.Ebonstone,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeStone ?? orig,
                (orig) => WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeStone.HasValue);
            ALUtils.ReplaceIDs<int>(il,
                TileID.CorruptHardenedSand,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeHardenedSand ?? orig,
                (orig) => WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeHardenedSand.HasValue);
            ALUtils.ReplaceIDs<int>(il,
                TileID.CorruptGrass,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeGrass ?? orig,
                (orig) => WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeGrass.HasValue);
            ALUtils.ReplaceIDs<int>(il,
                TileID.Ebonsand,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSand ?? orig,
                (orig) => WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSand.HasValue);
            ALUtils.ReplaceIDs<int>(il,
                TileID.CorruptIce,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeIce ?? orig,
                (orig) => WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeIce.HasValue);
            ALUtils.ReplaceIDs<int>(il,
                TileID.CorruptSandstone,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSandstone ?? orig,
                (orig) => WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSandstone.HasValue);
        }
    }
}
