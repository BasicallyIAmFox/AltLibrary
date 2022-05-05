using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using AltLibrary.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ReLogic.Content;
using System;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;

namespace AltLibrary.Common.Hooks
{
    internal static class OuterVisual
    {
        public static void Init()
        {
            IL.Terraria.GameContent.UI.Elements.UIGenProgressBar.DrawSelf += UIGenProgressBar_DrawSelf;
        }

        private static void UIGenProgressBar_DrawSelf(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchLdcI4(-8131073)))
                return;
            if (!c.TryGotoNext(i => i.MatchCall(out _)))
                return;
            c.Index++;
            c.Emit(OpCodes.Ldloc, 5);
            c.EmitDelegate<Func<Color, Color>>((color) =>
            {
                int worldGenStep = 0;
                if (WorldGen.crimson) worldGenStep = 1;
                if (WorldBiomeManager.worldEvil != "") worldGenStep = ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).Type + 2;

                if (WorldGen.drunkWorldGen && Main.rand.NextBool(2)) worldGenStep = Main.rand.Next(AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Evil).ToList().Count + 2);

                Color expected = new(95, 242, 86);
                if (worldGenStep == 1) expected = new Color(255, 237, 131);
                foreach (AltBiome biome in AltLibrary.biomes)
                {
                    if (worldGenStep == biome.Type + 2 && biome.BiomeType == BiomeType.Evil)
                    {
                        expected = biome.OuterColor;
                    }
                }

                Color result = expected;
                return result;
            });
            c.Emit(OpCodes.Stloc, 5);
            if (!c.TryGotoNext(i => i.MatchLdfld<UIGenProgressBar>("_texOuterCorrupt")))
                return;
            c.Remove();
            c.EmitDelegate<Func<UIGenProgressBar, Asset<Texture2D>>>((unusedVariableLeftInForLoading) =>
            {
                int worldGenStep = 0;
                if (WorldGen.crimson) worldGenStep = 1;
                if (WorldBiomeManager.worldEvil != "") worldGenStep = ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).Type + 2;

                Asset<Texture2D> asset = ILHooks.EmptyAsset;
                return worldGenStep <= 1 ? (worldGenStep == 0 ?
                    Main.Assets.Request<Texture2D>("Images/UI/WorldGen/Outer_Corrupt") :
                    Main.Assets.Request<Texture2D>("Images/UI/WorldGen/Outer_Crimson")) : asset;
            });
            if (!c.TryGotoNext(i => i.MatchLdfld<UIGenProgressBar>("_texOuterCrimson")))
                return;
            c.Remove();
            c.EmitDelegate<Func<UIGenProgressBar, Asset<Texture2D>>>((unusedVariableLeftInForLoading) =>
            {
                int worldGenStep = 0;
                if (WorldGen.crimson) worldGenStep = 1;
                if (WorldBiomeManager.worldEvil != "") worldGenStep = ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).Type + 2;

                Asset<Texture2D> asset = ILHooks.EmptyAsset;
                return worldGenStep <= 1 ? (worldGenStep == 0 ?
                    Main.Assets.Request<Texture2D>("Images/UI/WorldGen/Outer_Corrupt") :
                    Main.Assets.Request<Texture2D>("Images/UI/WorldGen/Outer_Crimson")) : asset;
            });
            if (!c.TryGotoNext(i => i.MatchCallvirt(out _)))
                return;
            if (!c.TryGotoNext(i => i.MatchCallvirt(out _)))
                return;
            c.Index++;
            c.Emit(OpCodes.Ldarg, 1);
            c.Emit(OpCodes.Ldloc, 6);
            c.EmitDelegate<Action<SpriteBatch, Rectangle>>((spriteBatch, r) =>
            {
                int worldGenStep = 0;
                if (WorldGen.crimson) worldGenStep = 1;
                if (WorldBiomeManager.worldEvil != "") worldGenStep = ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).Type + 2;
                if (WorldGen.drunkWorldGen && Main.rand.NextBool(2)) worldGenStep = Main.rand.Next(AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Evil).ToList().Count + 2);
                Asset<Texture2D> asset = ILHooks.EmptyAsset;
                if (worldGenStep == 0) asset = Main.Assets.Request<Texture2D>("Images/UI/WorldGen/Outer_Corrupt");
                if (worldGenStep == 1) asset = Main.Assets.Request<Texture2D>("Images/UI/WorldGen/Outer_Crimson");
                foreach (AltBiome biome in AltLibrary.biomes)
                {
                    if (worldGenStep == biome.Type + 2 && biome.BiomeType == BiomeType.Evil)
                    {
                        asset = ModContent.Request<Texture2D>(ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).OuterTexture)
                            ?? ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/Outer Empty");
                    }
                }
                spriteBatch.Draw(asset.Value, r.TopLeft(), Color.White);
            });
        }
    }
}
