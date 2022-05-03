using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ModLoader;

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
            c.Remove();
            c.Emit(OpCodes.Ldloc, 7);
            c.Emit(OpCodes.Ldloc, 5);
            c.EmitDelegate<Func<int, Tile, int>>((orig, tile) =>
            {
                if (WorldBiomeManager.worldHallow != "" &&
                    ((ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeGrass.HasValue &&
                        tile.TileType == ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeGrass.Value) ||
                    (ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.HasValue &&
                        tile.TileType == ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.Value) ||
                    (ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSand.HasValue &&
                        tile.TileType == ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSand.Value) ||
                    (ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeIce.HasValue &&
                        tile.TileType == ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeIce.Value) ||
                    (ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeHardenedSand.HasValue &&
                        tile.TileType == ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeHardenedSand.Value) ||
                    (ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSandstone.HasValue &&
                        tile.TileType == ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSandstone.Value))
                    // Vine here!
                    )
                {
                    orig = (ushort)(168 + WorldGen.genRand.Next(4));
                }
                if (WorldBiomeManager.worldEvil != "" &&
                    ((ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass.HasValue &&
                        tile.TileType == ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass.Value) ||
                    (ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.HasValue &&
                        tile.TileType == ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.Value) ||
                    (ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSand.HasValue &&
                        tile.TileType == ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSand.Value) ||
                    (ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeIce.HasValue &&
                        tile.TileType == ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeIce.Value) ||
                    (ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeHardenedSand.HasValue &&
                        tile.TileType == ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeHardenedSand.Value) ||
                    (ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSandstone.HasValue &&
                        tile.TileType == ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSandstone.Value))
                    // Vine here!
                    )
                {
                    orig = (ushort)(153 + WorldGen.genRand.Next(4));
                }
                return orig;
            });
            c.Emit(OpCodes.Stloc, 7);
            c.Emit(OpCodes.Ldloc, 4);
        }

        private static void WorldGen_GERunner(ILContext il)
        {
            ILCursor c = new(il);

            #region Hallow
            if (!c.TryGotoNext(i => i.MatchLdcI4(70))) { AltLibrary.Instance.Logger.Info("Error here! $1"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.ContainsKey(70))
                {
                    ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.TryGetValue(70, out value);
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(219))) { AltLibrary.Instance.Logger.Info("Error here! $2"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.ContainsKey(219))
                {
                    ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.TryGetValue(219, out value);
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(222))) { AltLibrary.Instance.Logger.Info("Error here! $3"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.ContainsKey(222))
                {
                    ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.TryGetValue(222, out value);
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(28))) { AltLibrary.Instance.Logger.Info("Error here! $4"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.ContainsKey(28))
                {
                    ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.TryGetValue(28, out value);
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(117))) { AltLibrary.Instance.Logger.Info("Error here! $5"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(402))) { AltLibrary.Instance.Logger.Info("Error here! $6"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeHardenedSand.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeHardenedSand.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(109))) { AltLibrary.Instance.Logger.Info("Error here! $7"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeGrass.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeGrass.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(117))) { AltLibrary.Instance.Logger.Info("Error here! $8"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(116))) { AltLibrary.Instance.Logger.Info("Error here! $9"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSand.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSand.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(23))) { AltLibrary.Instance.Logger.Info("Error here! $10"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(199))) { AltLibrary.Instance.Logger.Info("Error here! $11"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(109))) { AltLibrary.Instance.Logger.Info("Error here! $12"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeGrass.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeGrass.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(25))) { AltLibrary.Instance.Logger.Info("Error here! $13"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(203))) { AltLibrary.Instance.Logger.Info("Error here! $14"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(117))) { AltLibrary.Instance.Logger.Info("Error here! $15"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(112))) { AltLibrary.Instance.Logger.Info("Error here! $16"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSand.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSand.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(234))) { AltLibrary.Instance.Logger.Info("Error here! $17"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSand.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSand.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(116))) { AltLibrary.Instance.Logger.Info("Error here! $18"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSand.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSand.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(163))) { AltLibrary.Instance.Logger.Info("Error here! $19"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeIce.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeIce.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(200))) { AltLibrary.Instance.Logger.Info("Error here! $20"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeIce.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeIce.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(164))) { AltLibrary.Instance.Logger.Info("Error here! $21"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeIce.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeIce.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(403))) { AltLibrary.Instance.Logger.Info("Error here! $22"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSandstone.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSandstone.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(402))) { AltLibrary.Instance.Logger.Info("Error here! $23"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeHardenedSand.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeHardenedSand.Value;
                }
                return value;
            });
            #endregion

            #region Crimson
            if (!c.TryGotoNext(i => i.MatchLdcI4(109))) { AltLibrary.Instance.Logger.Info("Error here! $24"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeGrass.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeGrass.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(117))) { AltLibrary.Instance.Logger.Info("Error here! $25"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(116))) { AltLibrary.Instance.Logger.Info("Error here! $26"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSand.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSand.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(164))) { AltLibrary.Instance.Logger.Info("Error here! $27"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeIce.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeIce.Value;
                }
                return value;
            });
            #endregion

            #region Corruption (and custom)
            if (!c.TryGotoNext(i => i.MatchLdcI4(69))) { AltLibrary.Instance.Logger.Info("Error here! $28"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).WallContext.wallsReplacement.ContainsKey(69))
                {
                    ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).WallContext.wallsReplacement.TryGetValue(69, out value);
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(217))) { AltLibrary.Instance.Logger.Info("Error here! $29"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).WallContext.wallsReplacement.ContainsKey(217))
                {
                    ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).WallContext.wallsReplacement.TryGetValue(217, out value);
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(220))) { AltLibrary.Instance.Logger.Info("Error here! $30"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).WallContext.wallsReplacement.ContainsKey(217))
                {
                    ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).WallContext.wallsReplacement.TryGetValue(220, out value);
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(25))) { AltLibrary.Instance.Logger.Info("Error here! $31"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(398))) { AltLibrary.Instance.Logger.Info("Error here! $32"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeHardenedSand.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeHardenedSand.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(23))) { AltLibrary.Instance.Logger.Info("Error here! $33"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(25))) { AltLibrary.Instance.Logger.Info("Error here! $34"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(112))) { AltLibrary.Instance.Logger.Info("Error here! $35"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSand.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSand.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(109))) { AltLibrary.Instance.Logger.Info("Error here! $36"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeGrass.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeGrass.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(23))) { AltLibrary.Instance.Logger.Info("Error here! $37"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(117))) { AltLibrary.Instance.Logger.Info("Error here! $38"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(25))) { AltLibrary.Instance.Logger.Info("Error here! $39"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(116))) { AltLibrary.Instance.Logger.Info("Error here! $40"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSand.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeSand.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(112))) { AltLibrary.Instance.Logger.Info("Error here! $41"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSand.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSand.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(164))) { AltLibrary.Instance.Logger.Info("Error here! $42"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeIce.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeIce.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(163))) { AltLibrary.Instance.Logger.Info("Error here! $43"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeIce.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeIce.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(400))) { AltLibrary.Instance.Logger.Info("Error here! $44"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSandstone.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSandstone.Value;
                }
                return value;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(398))) { AltLibrary.Instance.Logger.Info("Error here! $45"); return; }
            c.Index++;
            c.EmitDelegate<Func<ushort, ushort>>((orig) =>
            {
                ushort value = orig;
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeHardenedSand.HasValue)
                {
                    value = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeHardenedSand.Value;
                }
                return value;
            });
            #endregion
        }
    }
}
