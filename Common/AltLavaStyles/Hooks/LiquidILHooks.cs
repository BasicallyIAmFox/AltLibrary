using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ReLogic.Content;
using System;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltLavaStyles.Hooks
{
    internal static class LiquidILHooks
    {
        internal static void Init()
        {
            IL.Terraria.Main.oldDrawWater += Main_oldDrawWater;
            IL.Terraria.GameContent.Liquid.LiquidRenderer.InternalDraw += LiquidRenderer_InternalDraw;
            On.Terraria.GameContent.Drawing.TileDrawing.DrawPartialLiquid += TileDrawing_DrawPartialLiquid;
            IL.Terraria.WaterfallManager.DrawWaterfall += WaterfallManager_DrawWaterfall;
        }

        private static void WaterfallManager_DrawWaterfall(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchLdcR4(900f)))
                return;
            for (int j = 0; j < 3; j++)
            {
                if (!c.TryGotoNext(i => i.MatchStloc(60 + j)))
                    return;
                c.Index++;
                c.Emit(OpCodes.Ldloc, 60 + j);
                c.Emit(OpCodes.Ldc_I4, j);
                c.EmitDelegate<Func<float, int, float>>((orig, j) =>
                {
                    foreach (AltLavaStyle lavaStyle in AltLibrary.LavaStyles)
                    {
                        if (lavaStyle.IsActive.Invoke())
                        {
                            return j switch
                            {
                                0 => lavaStyle.LiquidColor.R,
                                1 => lavaStyle.LiquidColor.G,
                                2 => lavaStyle.LiquidColor.B,
                                _ => orig
                            };
                        }
                    }
                    return orig;
                });
                c.Emit(OpCodes.Stloc, 60 + j);
            }

            while (c.TryGotoNext(
                i => i.MatchLdarg(1),
                i => i.MatchLdarg(0),
                i => i.MatchLdfld<WaterfallManager>("waterfallTexture") && i.Offset != 0,
                i => i.MatchLdloc(12) || i.MatchLdloc(20),
                i => i.MatchLdelemRef()))
            {
                c.Index += 5;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg, 0);
                c.Emit(OpCodes.Ldfld, typeof(WaterfallManager).GetField("waterfallTexture", BindingFlags.NonPublic | BindingFlags.Instance));
                c.Emit(OpCodes.Ldloc, 12);
                c.Emit(OpCodes.Ldloc, 20);
                c.EmitDelegate<Func<Asset<Texture2D>[], int, int, Asset<Texture2D>>>((waterfallTexture, num59, num109) =>
                {
                    if (num109 != num59)
                    {
                        foreach (AltLavaStyle lavaStyle in AltLibrary.LavaStyles)
                        {
                            if (lavaStyle.IsActive.Invoke() && num109 == 1)
                            {
                                return ModContent.Request<Texture2D>(lavaStyle.WaterfallTexture, AssetRequestMode.ImmediateLoad);
                            }
                        }
                        return waterfallTexture[num109];
                    }
                    else
                    {
                        foreach (AltLavaStyle lavaStyle in AltLibrary.LavaStyles)
                        {
                            if (lavaStyle.IsActive.Invoke() && num59 == 1)
                            {
                                return ModContent.Request<Texture2D>(lavaStyle.WaterfallTexture, AssetRequestMode.ImmediateLoad);
                            }
                        }
                        return waterfallTexture[num59];
                    }
                });
            }
        }

        private static void TileDrawing_DrawPartialLiquid(On.Terraria.GameContent.Drawing.TileDrawing.orig_DrawPartialLiquid orig, TileDrawing self, Tile tileCache, Vector2 position, Rectangle liquidSize, int liquidType, Color aColor)
        {
            orig(self, tileCache, position, liquidSize, liquidType, aColor);
            int num = (int)tileCache.Slope;
            if (TileID.Sets.BlocksWaterDrawingBehindSelf[tileCache.TileType] && num != 0)
            {
                liquidSize.X += 18 * (num - 1);
                foreach (AltLavaStyle lavaStyle in AltLibrary.LavaStyles)
                {
                    if (lavaStyle.IsActive.Invoke() && liquidType == 1)
                    {
                        if (tileCache.Slope == SlopeType.SlopeDownLeft)
                        {
                            Main.spriteBatch.Draw(ModContent.Request<Texture2D>(lavaStyle.SlopeTexture, AssetRequestMode.ImmediateLoad).Value, position, liquidSize, aColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        }
                        else if (tileCache.Slope == SlopeType.SlopeDownRight)
                        {
                            Main.spriteBatch.Draw(ModContent.Request<Texture2D>(lavaStyle.SlopeTexture, AssetRequestMode.ImmediateLoad).Value, position, liquidSize, aColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        }
                        else if (tileCache.Slope == SlopeType.SlopeUpLeft)
                        {
                            Main.spriteBatch.Draw(ModContent.Request<Texture2D>(lavaStyle.SlopeTexture, AssetRequestMode.ImmediateLoad).Value, position, liquidSize, aColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        }
                        else if (tileCache.Slope == SlopeType.SlopeUpRight)
                        {
                            Main.spriteBatch.Draw(ModContent.Request<Texture2D>(lavaStyle.SlopeTexture, AssetRequestMode.ImmediateLoad).Value, position, liquidSize, aColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        }
                    }
                }
            }
            else
            {
                foreach (AltLavaStyle lavaStyle in AltLibrary.LavaStyles)
                {
                    if (lavaStyle.IsActive.Invoke() && liquidType == 1)
                    {
                        Main.spriteBatch.Draw(ModContent.Request<Texture2D>(lavaStyle.Texture, AssetRequestMode.ImmediateLoad).Value, position, liquidSize, aColor, 0f, default, 1f, SpriteEffects.None, 0f);
                    }
                }
            }
        }

        private static void LiquidRenderer_InternalDraw(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchLdsfld<Main>(nameof(Main.tileBatch))))
                return;
            if (!c.TryGotoNext(i => i.MatchCallOrCallvirt<TileBatch>(nameof(TileBatch.Draw))))
                return;

            c.Index++;
            c.Emit(OpCodes.Ldloc, 8);
            c.Emit(OpCodes.Ldloc, 3);
            c.Emit(OpCodes.Ldloc, 4);
            c.Emit(OpCodes.Ldarg, 2);
            c.Emit(OpCodes.Ldloc, 6);
            c.Emit(OpCodes.Ldloc, 5);
            c.Emit(OpCodes.Ldloc, 9);
            c.EmitDelegate<Action<int, int, int, Vector2, Vector2, Rectangle, VertexColors>>((num2, j, i, drawOffset, liquidOffset, sourceRectangle, vertices) =>
            {
                foreach (AltLavaStyle lavaStyle in AltLibrary.LavaStyles)
                {
                    if (lavaStyle.IsActive.Invoke() && num2 == 1)
                    {
                        Main.tileBatch.Draw(ModContent.Request<Texture2D>(lavaStyle.Texture, AssetRequestMode.ImmediateLoad).Value,
                                        new Vector2(j << 4, i << 4) + drawOffset + liquidOffset,
                                        sourceRectangle,
                                        vertices,
                                        Vector2.Zero,
                                        1f,
                                        SpriteEffects.None);
                    }
                }
            });
        }

        private static void Main_oldDrawWater(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchCallOrCallvirt<SpriteBatch>(nameof(SpriteBatch.Draw))))
                return;

            c.Index++;
            c.Emit(OpCodes.Ldloc, 15);
            c.Emit(OpCodes.Ldloc, 17);
            c.Emit(OpCodes.Ldloc, 39);
            c.Emit(OpCodes.Ldloc, 40);
            c.Emit(OpCodes.Ldloc, 5);
            c.Emit(OpCodes.Ldloc, 18);
            c.Emit(OpCodes.Ldloc, 41);
            c.Emit(OpCodes.Ldloc, 42);
            c.Emit(OpCodes.Ldloc, 43);
            c.EmitDelegate<Action<int, Vector2, int, int, Vector2, Rectangle, int, int, Color>>((num57, value4, num38, num37, value5, value3, width, height, color3) =>
            {
                foreach (AltLavaStyle lavaStyle in AltLibrary.LavaStyles)
                {
                    if (lavaStyle.IsActive.Invoke() && num57 == 1)
                    {
                        Main.spriteBatch.Draw(ModContent.Request<Texture2D>(lavaStyle.Texture, AssetRequestMode.ImmediateLoad).Value,
                                              value4 - Main.screenPosition + new Vector2(num38, num37) + value5,
                                              new Rectangle(value3.X + num38, value3.Y + num37, width, height),
                                              color3,
                                              0f,
                                              default,
                                              1f,
                                              SpriteEffects.None,
                                              0f);
                    }
                }
            });

            if (!c.TryGotoNext(i => i.MatchCallOrCallvirt<SpriteBatch>(nameof(SpriteBatch.Draw))))
                return;

            c.Index++;
            c.Emit(OpCodes.Ldloc, 15);
            c.Emit(OpCodes.Ldloc, 17);
            c.Emit(OpCodes.Ldloc, 5);
            c.Emit(OpCodes.Ldloc, 18);
            c.Emit(OpCodes.Ldloc, 45);
            c.EmitDelegate<Action<int, Vector2, Vector2, Rectangle, Color>>((num57, value4, value5, value3, color) =>
            {
                foreach (AltLavaStyle lavaStyle in AltLibrary.LavaStyles)
                {
                    if (lavaStyle.IsActive.Invoke() && num57 == 1)
                    {
                        Main.spriteBatch.Draw(ModContent.Request<Texture2D>(lavaStyle.Texture, AssetRequestMode.ImmediateLoad).Value,
                                              value4 - Main.screenPosition + value5,
                                              value3,
                                              color,
                                              0f,
                                              default,
                                              1f,
                                              SpriteEffects.None,
                                              0f);
                    }
                }
            });

            if (!c.TryGotoNext(i => i.MatchCallOrCallvirt<SpriteBatch>(nameof(SpriteBatch.Draw))))
                return;

            c.Index++;
            c.Emit(OpCodes.Ldloc, 15);
            c.Emit(OpCodes.Ldloc, 17);
            c.Emit(OpCodes.Ldloc, 5);
            c.Emit(OpCodes.Ldloc, 18);
            c.Emit(OpCodes.Ldloc, 45);
            c.EmitDelegate<Action<int, Vector2, Vector2, Rectangle, Color>>((num57, value4, value5, value3, color) =>
            {
                foreach (AltLavaStyle lavaStyle in AltLibrary.LavaStyles)
                {
                    if (lavaStyle.IsActive.Invoke() && num57 == 1)
                    {
                        Main.spriteBatch.Draw(ModContent.Request<Texture2D>(lavaStyle.Texture, AssetRequestMode.ImmediateLoad).Value,
                                              value4 - Main.screenPosition + value5,
                                              value3,
                                              color,
                                              0f,
                                              default,
                                              1f,
                                              SpriteEffects.None,
                                              0f);
                    }
                }
            });

            if (!c.TryGotoNext(i => i.MatchCallOrCallvirt<SpriteBatch>(nameof(SpriteBatch.Draw))))
                return;

            c.Index++;
            c.Emit(OpCodes.Ldloc, 15);
            c.Emit(OpCodes.Ldloc, 17);
            c.Emit(OpCodes.Ldloc, 5);
            c.Emit(OpCodes.Ldloc, 45);
            c.EmitDelegate<Action<int, Vector2, Vector2, Color>>((num57, value4, value5, color) =>
            {
                foreach (AltLavaStyle lavaStyle in AltLibrary.LavaStyles)
                {
                    if (lavaStyle.IsActive.Invoke() && num57 == 1)
                    {
                        Main.spriteBatch.Draw(ModContent.Request<Texture2D>(lavaStyle.Texture, AssetRequestMode.ImmediateLoad).Value,
                                              value4 - Main.screenPosition + value5,
                                              new Rectangle(0, 4, 16, 8),
                                              color,
                                              0f,
                                              default,
                                              1f,
                                              SpriteEffects.None,
                                              0f);
                    }
                }
            });
        }
    }
}
