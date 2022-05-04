using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
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
            if (!c.TryGotoNext(i => i.MatchCall<WorldGen>(nameof(WorldGen.AddBuriedChest))))
                return;
            if (!c.TryGotoPrev(i => i.MatchLdloc(98)))
                return;

            c.Index++;
            c.Emit(OpCodes.Ldloc, 93);
            c.EmitDelegate<Func<int, int, int>>((contain, chests) =>
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
                return contain;
            });

            if (!c.TryGotoPrev(i => i.MatchLdloc(99)))
                return;

            c.Index++;
            c.Emit(OpCodes.Ldloc, 93);
            c.EmitDelegate<Func<int, int, int>>((style, chests) =>
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
                return style;
            });

            if (!c.TryGotoPrev(i => i.MatchLdloc(97)))
                return;

            c.Index++;
            c.Emit(OpCodes.Ldloc, 93);
            c.EmitDelegate<Func<int, int, int>>((contain, chests) =>
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
                return contain;
            });
        }
    }
}
