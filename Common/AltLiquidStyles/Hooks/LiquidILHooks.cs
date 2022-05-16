using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ReLogic.Content;
using System;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltLiquidStyles.Hooks
{
    internal static class LiquidILHooks
    {
        internal static void Init()
        {
            IL.Terraria.Main.oldDrawWater += Main_oldDrawWater;
            IL.Terraria.GameContent.Liquid.LiquidRenderer.InternalDraw += LiquidRenderer_InternalDraw;
            On.Terraria.GameContent.Drawing.TileDrawing.DrawPartialLiquid += TileDrawing_DrawPartialLiquid;
            IL.Terraria.WaterfallManager.DrawWaterfall += WaterfallManager_DrawWaterfall;
            On.Terraria.Graphics.Light.TileLightScanner.ApplyLavaLight += TileLightScanner_ApplyLavaLight;
            IL.Terraria.Player.Update += Player_Update;
            IL.Terraria.Main.DrawInterface_Resources_Breath += Main_DrawInterface_Resources_Breath;
            IL.Terraria.Liquid.LavaCheck += Liquid_LavaCheck;
            IL.Terraria.Liquid.HoneyCheck += Liquid_HoneyCheck;
            IL.Terraria.GameContent.Drawing.TileDrawing.EmitLiquidDrops += TileDrawing_EmitLiquidDrops;
            On.Terraria.Map.MapHelper.GetMapTileXnaColor += MapHelper_GetMapTileXnaColor;
            IL.Terraria.Player.ItemCheck_UseBuckets += Player_ItemCheck_UseBuckets;
        }

        private static void Player_ItemCheck_UseBuckets(ILContext il)
        {
            ALUtils.ReplaceIDs<int>(il,
                ItemID.LavaBucket,
                (orig) =>
                {
                    foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                    {
                        if (liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Lava)
                        {
                            return liquidStyle.BucketReplacement.Value;
                        }
                    }
                    return orig;
                },
                (orig) =>
                {
                    foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                    {
                        return liquidStyle.BucketReplacement.HasValue && liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Lava;
                    }
                    return false;
                });
            ALUtils.ReplaceIDs<int>(il,
                ItemID.HoneyBucket,
                (orig) =>
                {
                    foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                    {
                        if (liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Honey)
                        {
                            return liquidStyle.BucketReplacement.Value;
                        }
                    }
                    return orig;
                },
                (orig) =>
                {
                    foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                    {
                        return liquidStyle.BucketReplacement.HasValue && liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Honey;
                    }
                    return false;
                });
            ALUtils.ReplaceIDs<int>(il,
                ItemID.BottomlessLavaBucket,
                (orig) =>
                {
                    foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                    {
                        if (liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Lava)
                        {
                            return liquidStyle.BottomlessBucketReplacement.Value;
                        }
                    }
                    return orig;
                },
                (orig) =>
                {
                    foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                    {
                        return liquidStyle.BottomlessBucketReplacement.HasValue && liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Lava;
                    }
                    return false;
                });
        }

        private static Color MapHelper_GetMapTileXnaColor(On.Terraria.Map.MapHelper.orig_GetMapTileXnaColor orig, ref MapTile tile)
        {
            if (tile.Type == 1132)
            {
                foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                {
                    if (liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Lava)
                    {
                        return liquidStyle.MapColor;
                    }
                }
            }
            if (tile.Type == 1133)
            {
                foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                {
                    if (liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Honey)
                    {
                        return liquidStyle.MapColor;
                    }
                }
            }
            return orig(ref tile);
        }

        private static void TileDrawing_EmitLiquidDrops(ILContext il)
        {
            ALUtils.ReplaceIDs(il,
                GoreID.LavaDrip,
                (orig) =>
                {
                    foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                    {
                        if (liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Lava)
                        {
                            return liquidStyle.GoreDropletID.Value;
                        }
                    }
                    return orig;
                },
                (orig) =>
                {
                    foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                    {
                        return liquidStyle.GoreDropletID.HasValue && liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Lava;
                    }
                    return false;
                });
            ALUtils.ReplaceIDs(il,
                GoreID.HoneyDrip,
                (orig) =>
                {
                    foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                    {
                        if (liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Honey)
                        {
                            return liquidStyle.GoreDropletID.Value;
                        }
                    }
                    return orig;
                },
                (orig) =>
                {
                    foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                    {
                        return liquidStyle.GoreDropletID.HasValue && liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Honey;
                    }
                    return false;
                });
        }

        private static void Liquid_HoneyCheck(ILContext il)
        {
            ALUtils.ReplaceIDs<int>(il,
                TileID.HoneyBlock,
                (orig) =>
                {
                    foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                    {
                        if (liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Honey)
                        {
                            return liquidStyle.HoneyWater.Value;
                        }
                    }
                    return orig;
                },
                (orig) =>
                {
                    foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                    {
                        return liquidStyle.HoneyWater.HasValue && liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Honey;
                    }
                    return false;
                });
        }

        private static void Liquid_LavaCheck(ILContext il)
        {
            ILCursor c = new(il);
            ALUtils.ReplaceIDs<int>(il,
                TileID.Obsidian,
                (orig) =>
                {
                    foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                    {
                        if (liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Lava)
                        {
                            return liquidStyle.ObsidianWater.Value;
                        }
                    }
                    return orig;
                },
                (orig) =>
                {
                    foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                    {
                        return liquidStyle.ObsidianWater.HasValue && liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Lava;
                    }
                    return false;
                });
            ALUtils.ReplaceIDs<int>(il,
                TileID.CrispyHoneyBlock,
                (orig) =>
                {
                    foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                    {
                        if (liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Honey)
                        {
                            return liquidStyle.CrispyHoneyWater.Value;
                        }
                    }
                    return orig;
                },
                (orig) =>
                {
                    foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                    {
                        return liquidStyle.CrispyHoneyWater.HasValue && liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Honey;
                    }
                    return false;
                });
        }

        private static void Main_DrawInterface_Resources_Breath(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchLdsfld<Main>(nameof(Main.spriteBatch))))
                return;
            if (!c.TryGotoNext(i => i.MatchLdsfld<Main>(nameof(Main.spriteBatch))))
                return;
            if (!c.TryGotoNext(i => i.MatchLdsfld(out _)))
                return;

            c.Index++;
            c.Emit(OpCodes.Pop);
            c.EmitDelegate(() =>
            {
                foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                {
                    if (liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Lava)
                    {
                        return ModContent.Request<Texture2D>(liquidStyle.LavaImmuneTexture, AssetRequestMode.ImmediateLoad);
                    }
                }
                return TextureAssets.Flame;
            });
        }

        private static void Player_Update(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchLdloc(7),
                i => i.MatchBrfalse(out _),
                i => i.MatchLdarg(0),
                i => i.MatchLdcI4(48),
                i => i.MatchLdcI4(1800)))
                return;

            c.Index += 4;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                {
                    if (liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Honey && liquidStyle.LiquidBuff.HasValue)
                    {
                        return liquidStyle.LiquidBuff.Value;
                    }
                }
                return orig;
            });
            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                {
                    if (liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Honey && liquidStyle.LiquidBuffTime > 0)
                    {
                        return liquidStyle.LiquidBuffTime;
                    }
                }
                return orig;
            });

            if (!c.TryGotoPrev(i => i.MatchCallOrCallvirt<Player>(nameof(Player.AddBuff)) && i.Offset != 0))
                return;
            for (int j = 0; j < 2; j++)
            {
                if (!c.TryGotoPrev(i => i.MatchCall<PlayerDeathReason>(nameof(PlayerDeathReason.ByOther)) && i.Offset != 0))
                    return;

                c.Index++;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg, 0);
                c.EmitDelegate<Func<Player, PlayerDeathReason>>((player) =>
                {
                    foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                    {
                        if (liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Lava && liquidStyle.LavaDeathReason != null)
                        {
                            return liquidStyle.LavaDeathReason.Invoke(player);
                        }
                    }
                    return PlayerDeathReason.ByOther(2);
                });

                if (!c.TryGotoNext(i => i.MatchLdcI4(80) || i.MatchLdcI4(35)))
                    return;

                c.Index++;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldc_I4, j);
                c.EmitDelegate<Func<int, int>>((j) =>
                {
                    foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                    {
                        if (liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Lava && liquidStyle.LavaContactDamage > 0)
                        {
                            return liquidStyle.LavaContactDamage;
                        }
                    }
                    return (int)Math.Round(35f * (j == 0 ? 2.28571428571f : 1f));
                });

                if (!c.TryGotoNext(i => i.MatchLdcI4(24)))
                    return;

                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() =>
                {
                    foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                    {
                        if (liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Lava && liquidStyle.LiquidBuff.HasValue)
                        {
                            return liquidStyle.LiquidBuff.Value;
                        }
                    }
                    return 24;
                });

                if (!c.TryGotoNext(i => (i.MatchLdcI4(420) || i.MatchLdcI4(210)) && i.Offset != 0))
                    return;

                c.Index++;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldc_I4, j);
                c.EmitDelegate<Func<int, int>>((j) =>
                {
                    foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                    {
                        if (liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Lava && liquidStyle.LiquidBuffTime > 0)
                        {
                            return liquidStyle.LiquidBuffTime;
                        }
                    }
                    return 420 / (j + 1);
                });
            }
        }

        private static void TileLightScanner_ApplyLavaLight(On.Terraria.Graphics.Light.TileLightScanner.orig_ApplyLavaLight orig, Tile tile, ref Vector3 lightColor)
        {
            if (tile.LiquidType == LiquidID.Lava && tile.LiquidAmount > 0)
            {
                foreach (AltLiquidStyle liquidStyle in AltLibrary.LiquidStyles)
                {
                    if (!liquidStyle.IsActive.Invoke() && liquidStyle.LiquidStyle == LiquidStyle.Lava)
                    {
                        orig(tile, ref lightColor);
                    }
                    else
                    {
                        lightColor *= 0f;
                    }
                }
            }
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
                    foreach (AltLiquidStyle lavaStyle in AltLibrary.LiquidStyles)
                    {
                        if (lavaStyle.IsActive.Invoke() && lavaStyle.LiquidStyle == LiquidStyle.Lava)
                        {
                            return j switch
                            {
                                0 => lavaStyle.LavaColor.R,
                                1 => lavaStyle.LavaColor.G,
                                2 => lavaStyle.LavaColor.B,
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
                        foreach (AltLiquidStyle lavaStyle in AltLibrary.LiquidStyles)
                        {
                            if (lavaStyle.IsActive.Invoke() && (num59 == 1 && lavaStyle.LiquidStyle == LiquidStyle.Lava || num59 == 14 && lavaStyle.LiquidStyle == LiquidStyle.Honey))
                            {
                                return lavaStyle.GetTextures()[2];
                            }
                        }
                        return waterfallTexture[num59];
                    }
                    else
                    {
                        foreach (AltLiquidStyle lavaStyle in AltLibrary.LiquidStyles)
                        {
                            if (lavaStyle.IsActive.Invoke() && (num59 == 1 && lavaStyle.LiquidStyle == LiquidStyle.Lava || num59 == 14 && lavaStyle.LiquidStyle == LiquidStyle.Honey))
                            {
                                return lavaStyle.GetTextures()[2];
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
                foreach (AltLiquidStyle lavaStyle in AltLibrary.LiquidStyles)
                {
                    if (lavaStyle.IsActive.Invoke() && (liquidType == 1 && lavaStyle.LiquidStyle == LiquidStyle.Lava || liquidType == 11 && lavaStyle.LiquidStyle == LiquidStyle.Honey))
                    {
                        if (tileCache.Slope == SlopeType.SlopeDownLeft)
                        {
                            Main.spriteBatch.Draw(lavaStyle.GetTextures()[1].Value, position, liquidSize, aColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        }
                        else if (tileCache.Slope == SlopeType.SlopeDownRight)
                        {
                            Main.spriteBatch.Draw(lavaStyle.GetTextures()[1].Value, position, liquidSize, aColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        }
                        else if (tileCache.Slope == SlopeType.SlopeUpLeft)
                        {
                            Main.spriteBatch.Draw(lavaStyle.GetTextures()[1].Value, position, liquidSize, aColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        }
                        else if (tileCache.Slope == SlopeType.SlopeUpRight)
                        {
                            Main.spriteBatch.Draw(lavaStyle.GetTextures()[1].Value, position, liquidSize, aColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        }
                    }
                }
            }
            else
            {
                foreach (AltLiquidStyle lavaStyle in AltLibrary.LiquidStyles)
                {
                    if (lavaStyle.IsActive.Invoke() && (liquidType == 1 && lavaStyle.LiquidStyle == LiquidStyle.Lava || liquidType == 11 && lavaStyle.LiquidStyle == LiquidStyle.Honey))
                    {
                        Main.spriteBatch.Draw(lavaStyle.GetTextures()[3].Value, position, liquidSize, aColor, 0f, default, 1f, SpriteEffects.None, 0f);
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
                foreach (AltLiquidStyle lavaStyle in AltLibrary.LiquidStyles)
                {
                    if (lavaStyle.IsActive.Invoke() && (num2 == 1 && lavaStyle.LiquidStyle == LiquidStyle.Lava || num2 == 11 && lavaStyle.LiquidStyle == LiquidStyle.Honey))
                    {
                        Main.tileBatch.Draw(lavaStyle.GetTextures()[0].Value,
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
                foreach (AltLiquidStyle lavaStyle in AltLibrary.LiquidStyles)
                {
                    if (lavaStyle.IsActive.Invoke() && (num57 == 1 && lavaStyle.LiquidStyle == LiquidStyle.Lava || num57 == 11 && lavaStyle.LiquidStyle == LiquidStyle.Honey))
                    {
                        Main.spriteBatch.Draw(lavaStyle.GetTextures()[3].Value,
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
                foreach (AltLiquidStyle lavaStyle in AltLibrary.LiquidStyles)
                {
                    if (lavaStyle.IsActive.Invoke() && (num57 == 1 && lavaStyle.LiquidStyle == LiquidStyle.Lava || num57 == 11 && lavaStyle.LiquidStyle == LiquidStyle.Honey))
                    {
                        Main.spriteBatch.Draw(lavaStyle.GetTextures()[3].Value,
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
                foreach (AltLiquidStyle lavaStyle in AltLibrary.LiquidStyles)
                {
                    if (lavaStyle.IsActive.Invoke() && (num57 == 1 && lavaStyle.LiquidStyle == LiquidStyle.Lava || num57 == 11 && lavaStyle.LiquidStyle == LiquidStyle.Honey))
                    {
                        Main.spriteBatch.Draw(lavaStyle.GetTextures()[3].Value,
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
                foreach (AltLiquidStyle lavaStyle in AltLibrary.LiquidStyles)
                {
                    if (lavaStyle.IsActive.Invoke() && (num57 == 1 && lavaStyle.LiquidStyle == LiquidStyle.Lava || num57 == 11 && lavaStyle.LiquidStyle == LiquidStyle.Honey))
                    {
                        Main.spriteBatch.Draw(lavaStyle.GetTextures()[3].Value,
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
