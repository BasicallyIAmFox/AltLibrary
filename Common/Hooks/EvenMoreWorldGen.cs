using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ModLoader;

namespace AltLibrary.Common.Hooks
{
    internal static class EvenMoreWorldGen
    {
        public static void Init()
        {
            IL.Terraria.WorldGen.GenerateWorld += GenPasses.ILGenerateWorld;
            GenPasses.HookGenPassReset += GenPasses_HookGenPassReset;
            GenPasses.HookGenPassShinies += GenPasses_HookGenPassShinies;
            GenPasses.HookGenPassAltars += ILGenPassAltars;
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
            if (!c.TryGotoNext(i => i.MatchStsfld<WorldGen>(nameof(WorldGen.crimson))))
                return;
            if (!c.TryGotoNext(i => i.MatchLdfld(out _)))
                return;
            /*c.EmitDelegate(() =>
            {
                if (WorldBiomeManager.worldEvil == "")
                {
                    WorldGen.crimson = true;
                }
                else
                {
                    WorldGen.crimson = false;
                }
            });*/
            if (!c.TryGotoNext(i => i.MatchRet()))
                return;
            if (!c.TryGotoPrev(i => i.MatchBneUn(out _)))
                return;
            if (!c.TryGotoPrev(i => i.MatchLdcI4(-1)))
                return;
            c.EmitDelegate<Func<int, int>>(dungeonSide =>
            {
                WorldBiomeGeneration.dungeonLocation = dungeonSide;
                return dungeonSide;
            });
            for (int i = 0; i < 2; i++)
            {
                if (!c.TryGotoNext(i => i.MatchRet()))
                    return;
                c.Index--;
                c.EmitDelegate<Func<int, int>>(dungeonLocation =>
                {
                    WorldBiomeGeneration.dungeonLocation = dungeonLocation;
                    return dungeonLocation;
                });
            }
        }

        private static void GenPasses_HookGenPassShinies(ILContext il)
        {
            ILCursor c = new(il);
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
            c.EmitDelegate(() => !WorldGen.crimson && WorldBiomeManager.worldEvil == "");
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
