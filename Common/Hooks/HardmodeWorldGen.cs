using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace AltLibrary.Common.Hooks
{
    internal static class HardmodeWorldGen
    {
        public static void Init()
        {
            IL.Terraria.WorldGen.smCallBack += GenPasses.ILSMCallBack;
            IL.Terraria.WorldGen.GERunner += WorldGen_GERunner;
            GenPasses.HookGenPassHardmodeWalls += GenPasses_HookGenPassHardmodeWalls;
        }

        private static void GenPasses_HookGenPassHardmodeWalls(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchStloc(8)))
                return;
            if (!c.TryGotoNext(i => i.MatchLdloc(4)))
                return;
            c.Index++;
            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldloc, 7);
            c.Emit(OpCodes.Ldloc, 5);
            c.EmitDelegate<Func<int, Tile, int>>((orig, tile) =>
            {
                if (WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).HardmodeWalls.Count > 0 &&
                    ((Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeGrass.HasValue &&
                        tile.TileType == Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeGrass.Value) ||
                    (Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.HasValue &&
                        tile.TileType == Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.Value) ||
                    (Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSand.HasValue &&
                        tile.TileType == Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSand.Value) ||
                    (Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeIce.HasValue &&
                        tile.TileType == Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeIce.Value) ||
                    (Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeHardenedSand.HasValue &&
                        tile.TileType == Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeHardenedSand.Value) ||
                    (Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSandstone.HasValue &&
                        tile.TileType == Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSandstone.Value))
                    // Vine here!
                    )
                {
                    orig = Find<AltBiome>(WorldBiomeManager.worldHallow).HardmodeWalls[WorldGen.genRand.Next(Find<AltBiome>(WorldBiomeManager.worldHallow).HardmodeWalls.Count)];
                }
                if (WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).HardmodeWalls.Count > 0 &&
                    ((Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass.HasValue &&
                        tile.TileType == Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass.Value) ||
                    (Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.HasValue &&
                        tile.TileType == Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.Value) ||
                    (Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSand.HasValue &&
                        tile.TileType == Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSand.Value) ||
                    (Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeIce.HasValue &&
                        tile.TileType == Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeIce.Value) ||
                    (Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeHardenedSand.HasValue &&
                        tile.TileType == Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeHardenedSand.Value) ||
                    (Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSandstone.HasValue &&
                        tile.TileType == Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSandstone.Value))
                    // Vine here!
                    )
                {
                    orig = Find<AltBiome>(WorldBiomeManager.worldEvil).HardmodeWalls[WorldGen.genRand.Next(Find<AltBiome>(WorldBiomeManager.worldEvil).HardmodeWalls.Count)];
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
                return;
            if (!c.TryGotoNext(i => i.MatchBrfalse(out _)))
                return;
            if (!c.TryGotoPrev(i => i.MatchLdarg(out good)))
                return;
            if (!c.TryGotoPrev(i => i.MatchBgeUn(out _)))
                return;

            c.Index++;
            c.Emit(OpCodes.Ldarg, good);
            c.Emit(OpCodes.Ldloc, 15);
            c.Emit(OpCodes.Ldloc, 16);
            c.EmitDelegate<Action<bool, int, int>>((good, m, l) =>
            {
                if (!good)
                {
                    Tile tile = Main.tile[m, l];
                    if (WorldBiomeManager.worldEvil != "")
                    {
                        foreach (KeyValuePair<int, int> entry in Find<AltBiome>(WorldBiomeManager.worldEvil).SpecialConversion)
                        {
                            if (tile.TileType == entry.Key)
                            {
                                tile = Main.tile[m, l];
                                tile.TileType = (ushort)entry.Value;
                                WorldGen.SquareTileFrame(m, l, true);
                            }
                        }
                    }
                }
            });

            if (!c.TryGotoNext(i => i.MatchBrfalse(out _)))
                return;

            c.Index++;
            c.Emit(OpCodes.Ldloc, 15);
            c.Emit(OpCodes.Ldloc, 16);
            c.EmitDelegate<Action<int, int>>((m, l) =>
            {
                Tile tile = Main.tile[m, l];
                if (WorldBiomeManager.worldHallow != "")
                {
                    foreach (KeyValuePair<int, int> entry in Find<AltBiome>(WorldBiomeManager.worldHallow).SpecialConversion)
                    {
                        if (tile.TileType == entry.Key)
                        {
                            tile = Main.tile[m, l];
                            tile.TileType = (ushort)entry.Value;
                            WorldGen.SquareTileFrame(m, l, true);
                        }
                    }
                }
            });

            ALUtils.ReplaceIDs(il,
                WallID.HallowedGrassUnsafe,
                (orig) => Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.TryGetValue(orig, out ushort value) ? value : WallID.None,
                (orig) => WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.ContainsKey(orig));
            ALUtils.ReplaceIDs(il,
                WallID.HallowHardenedSand,
                (orig) => Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.TryGetValue(orig, out ushort value) ? value : WallID.None,
                (orig) => WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.ContainsKey(orig));
            ALUtils.ReplaceIDs(il,
                WallID.HallowSandstone,
                (orig) => Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.TryGetValue(orig, out ushort value) ? value : WallID.None,
                (orig) => WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.ContainsKey(orig));
            ALUtils.ReplaceIDs(il,
                WallID.PearlstoneBrickUnsafe,
                (orig) => Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.TryGetValue(orig, out ushort value) ? value : WallID.None,
                (orig) => WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.ContainsKey(orig));
            ALUtils.ReplaceIDs(il,
                WallID.CorruptGrassUnsafe,
                (orig) => Find<AltBiome>(WorldBiomeManager.worldEvil).WallContext.wallsReplacement.TryGetValue(orig, out ushort value) ? value : WallID.None,
                (orig) => WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).WallContext.wallsReplacement.ContainsKey(orig));
            ALUtils.ReplaceIDs(il,
                WallID.CorruptHardenedSand,
                (orig) => Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.TryGetValue(orig, out ushort value) ? value : WallID.None,
                (orig) => WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).WallContext.wallsReplacement.ContainsKey(orig));
            ALUtils.ReplaceIDs(il,
                WallID.CorruptSandstone,
                (orig) => Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.TryGetValue(orig, out ushort value) ? value : WallID.None,
                (orig) => WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).WallContext.wallsReplacement.ContainsKey(orig));

            ALUtils.ReplaceIDs<int>(il,
                TileID.Pearlstone,
                (orig) => Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone ?? orig,
                (orig) => WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.HasValue);
            ALUtils.ReplaceIDs<int>(il,
                TileID.HallowHardenedSand,
                (orig) => Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeHardenedSand ?? orig,
                (orig) => WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeHardenedSand.HasValue);
            ALUtils.ReplaceIDs<int>(il,
                TileID.HallowedGrass,
                (orig) => Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeGrass ?? orig,
                (orig) => WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeGrass.HasValue);
            ALUtils.ReplaceIDs<int>(il,
                TileID.Pearlsand,
                (orig) => Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSand ?? orig,
                (orig) => WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSand.HasValue);
            ALUtils.ReplaceIDs<int>(il,
                TileID.HallowedIce,
                (orig) => Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeIce ?? orig,
                (orig) => WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeIce.HasValue);
            ALUtils.ReplaceIDs<int>(il,
                TileID.HallowSandstone,
                (orig) => Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSandstone ?? orig,
                (orig) => WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSandstone.HasValue);

            ALUtils.ReplaceIDs<int>(il,
                TileID.Ebonstone,
                (orig) => Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone ?? orig,
                (orig) => WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.HasValue);
            ALUtils.ReplaceIDs<int>(il,
                TileID.CorruptHardenedSand,
                (orig) => Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeHardenedSand ?? orig,
                (orig) => WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeHardenedSand.HasValue);
            ALUtils.ReplaceIDs<int>(il,
                TileID.CorruptGrass,
                (orig) => Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass ?? orig,
                (orig) => WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass.HasValue);
            ALUtils.ReplaceIDs<int>(il,
                TileID.Ebonsand,
                (orig) => Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSand ?? orig,
                (orig) => WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSand.HasValue);
            ALUtils.ReplaceIDs<int>(il,
                TileID.CorruptIce,
                (orig) => Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeIce ?? orig,
                (orig) => WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeIce.HasValue);
            ALUtils.ReplaceIDs<int>(il,
                TileID.CorruptSandstone,
                (orig) => Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSandstone ?? orig,
                (orig) => WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSandstone.HasValue);

            /*
            #region Hallow
            if (!c.TryGotoNext(i => i.MatchLdcI4(70)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.ContainsKey(70))
                {
                    Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.TryGetValue(70, out value);
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(219)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.ContainsKey(219))
                {
                    Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.TryGetValue(219, out value);
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(222)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.ContainsKey(222))
                {
                    Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.TryGetValue(222, out value);
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(28)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.ContainsKey(28))
                {
                    Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.TryGetValue(28, out value);
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(117)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(402)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeHardenedSand.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeHardenedSand.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(109)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeGrass.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeGrass.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(117)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(116)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSand.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSand.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(23)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(199)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(109)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeGrass.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeGrass.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(25)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(203)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(117)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(112)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSand.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSand.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(234)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSand.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSand.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(116)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSand.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSand.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(163)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeIce.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeIce.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(200)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeIce.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeIce.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(164)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeIce.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeIce.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(403)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSandstone.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSandstone.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(402)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeHardenedSand.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeHardenedSand.Value;
                }
                return value;
            });
            #endregion

            #region Crimson
            if (!c.TryGotoNext(i => i.MatchLdcI4(109)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeGrass.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeGrass.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(117)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(116)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSand.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSand.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(164)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeIce.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeIce.Value;
                }
                return value;
            });
            #endregion

            #region Corruption (and custom)
            if (!c.TryGotoNext(i => i.MatchLdcI4(69)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).WallContext.wallsReplacement.ContainsKey(69))
                {
                    Find<AltBiome>(WorldBiomeManager.worldEvil).WallContext.wallsReplacement.TryGetValue(69, out value);
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(217)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).WallContext.wallsReplacement.ContainsKey(217))
                {
                    Find<AltBiome>(WorldBiomeManager.worldEvil).WallContext.wallsReplacement.TryGetValue(217, out value);
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(220)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).WallContext.wallsReplacement.ContainsKey(217))
                {
                    Find<AltBiome>(WorldBiomeManager.worldEvil).WallContext.wallsReplacement.TryGetValue(220, out value);
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(25)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(398)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeHardenedSand.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeHardenedSand.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(23)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(25)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(112)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSand.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSand.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(109)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeGrass.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeGrass.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(23)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(117)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(25)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(116)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSand.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSand.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(112)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSand.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSand.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(164)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeIce.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeIce.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(163)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeIce.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeIce.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(400)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSandstone.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSandstone.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(398)))
                return;
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeHardenedSand.HasValue)
                {
                    value = (ushort)Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeHardenedSand.Value;
                }
                return value;
            });
            #endregion
            */
        }
    }
}
