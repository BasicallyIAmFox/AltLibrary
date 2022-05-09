using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace AltLibrary.Common.Hooks
{
    internal class DungeonChests
    {
        public static void Init()
        {
            IL.Terraria.WorldGen.MakeDungeon += WorldGen_MakeDungeon;
        }

        private static void WorldGen_MakeDungeon(ILContext il)
        {
            ILCursor c = new(il);
            int hellChestIndex = int.MinValue;
            if (!c.TryGotoNext(i => i.MatchCall<WorldGen>(nameof(WorldGen.AddBuriedChest))))
                return;
            if (!c.TryGotoPrev(i => i.MatchStloc(15)))
                return;

            c.Index++;
            c.Emit(OpCodes.Ldloc, 15);
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldBiomeManager.worldHell != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHell).BiomeChestTile.HasValue)
                {
                    orig++;
                    hellChestIndex = orig;
                }
                return orig;
            });
            c.Emit(OpCodes.Stloc, 15);

            if (!c.TryGotoNext(i => i.MatchCall<WorldGen>(nameof(WorldGen.AddBuriedChest))))
                return;
            if (!c.TryGotoNext(i => i.MatchLdloc(98)))
                return;

            c.Index++;
            c.Emit(OpCodes.Ldloc, 93);
            c.Emit(OpCodes.Ldc_I4, hellChestIndex);
            c.EmitDelegate<Func<int, int, int, int>>((contain, chests, hellChestIndex) =>
            {
                if (chests == 0 && WorldBiomeManager.worldJungle != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldJungle).BiomeChestItem.HasValue)
                {
                    return ModContent.Find<AltBiome>(WorldBiomeManager.worldJungle).BiomeChestItem.Value;
                }
                if (chests == 2 && WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeChestItem.HasValue)
                {
                    return ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeChestItem.Value;
                }
                if ((chests == 1 || chests == 5) && WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeChestItem.HasValue)
                {
                    return ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeChestItem.Value;
                }
                if (chests == hellChestIndex && WorldBiomeManager.worldHell != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHell).BiomeChestItem.HasValue)
                {
                    return ModContent.Find<AltBiome>(WorldBiomeManager.worldHell).BiomeChestItem.Value;
                }
                return contain;
            });

            if (!c.TryGotoNext(i => i.MatchLdloc(99)))
                return;

            c.Index++;
            c.Emit(OpCodes.Ldloc, 93);
            c.Emit(OpCodes.Ldc_I4, hellChestIndex);
            c.EmitDelegate<Func<int, int, int, int>>((style, chests, hellChestIndex) =>
            {
                if (chests == 0 && WorldBiomeManager.worldJungle != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldJungle).BiomeChestTileStyle.HasValue)
                {
                    style = ModContent.Find<AltBiome>(WorldBiomeManager.worldJungle).BiomeChestTileStyle.Value;
                }
                if (chests == 2 && WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeChestTileStyle.HasValue)
                {
                    style = ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeChestTileStyle.Value;
                }
                if ((chests == 1 || chests == 5) && WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeChestTileStyle.HasValue)
                {
                    style = ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeChestTileStyle.Value;
                }
                if (chests == hellChestIndex && WorldBiomeManager.worldHell != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHell).BiomeChestTileStyle.HasValue)
                {
                    return ModContent.Find<AltBiome>(WorldBiomeManager.worldHell).BiomeChestTileStyle.Value;
                }
                return style;
            });

            if (!c.TryGotoNext(i => i.MatchLdloc(97)))
                return;

            c.Index++;
            c.Emit(OpCodes.Ldloc, 93);
            c.Emit(OpCodes.Ldc_I4, hellChestIndex);
            c.EmitDelegate<Func<int, int, int, int>>((chestTileType, chests, hellChestIndex) =>
            {
                if (chests == 0 && WorldBiomeManager.worldJungle != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldJungle).BiomeChestTile.HasValue)
                {
                    return ModContent.Find<AltBiome>(WorldBiomeManager.worldJungle).BiomeChestTile.Value;
                }
                if (chests == 2 && WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeChestTile.HasValue)
                {
                    return ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeChestTile.Value;
                }
                if ((chests == 1 || chests == 5) && WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeChestTile.HasValue)
                {
                    return ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeChestTile.Value;
                }
                if (chests == hellChestIndex && WorldBiomeManager.worldHell != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHell).BiomeChestTile.HasValue)
                {
                    return ModContent.Find<AltBiome>(WorldBiomeManager.worldHell).BiomeChestTile.Value;
                }
                return chestTileType;
            });

            foreach (Instruction instruction in c.Instrs)
            {
                object obj = instruction.Operand == null ? "" : instruction.Operand.ToString();
                AltLibrary.Instance.Logger.Debug($"{instruction.Offset} | {instruction.OpCode} | {obj}");
            }
        }
    }
}
