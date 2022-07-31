using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ReLogic.Content;
using System;
using System.Linq;
using System.Reflection;
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

		public static void Unload()
		{
			IL.Terraria.GameContent.UI.Elements.UIGenProgressBar.DrawSelf -= UIGenProgressBar_DrawSelf;
		}

		private static void UIGenProgressBar_DrawSelf(ILContext il)
		{
			ILCursor c = new(il);
			if (!c.TryGotoNext(i => i.MatchLdcI4(-8131073)))
			{
				AltLibrary.Instance.Logger.Info("m $ 1");
				return;
			}
			if (!c.TryGotoNext(i => i.MatchCall(out _)))
			{
				AltLibrary.Instance.Logger.Info("m $ 2");
				return;
			}
			c.Index++;
			c.Emit(OpCodes.Ldloc, 5);
			c.EmitDelegate<Func<Color, Color>>((color) =>
			{
				int worldGenStep = 0;
				if (WorldGen.crimson) worldGenStep = 1;
				if (WorldBiomeManager.WorldEvil != "") worldGenStep = ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).Type + 2;

				if (WorldGen.drunkWorldGen && Main.rand.NextBool(2)) worldGenStep = Main.rand.Next(AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Evil).ToList().Count + 2);

				Color expected = new(95, 242, 86);
				if (worldGenStep == 1) expected = new Color(255, 237, 131);
				foreach (AltBiome biome in AltLibrary.Biomes)
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
			{
				AltLibrary.Instance.Logger.Info("m $ 3");
				return;
			}
			c.Index++;
			c.Emit(OpCodes.Pop);
			c.Emit(OpCodes.Ldarg, 0);
			c.Emit(OpCodes.Ldfld, typeof(UIGenProgressBar).GetField("_texOuterCorrupt", BindingFlags.Instance | BindingFlags.NonPublic));
			c.Emit(OpCodes.Ldarg, 0);
			c.Emit(OpCodes.Ldfld, typeof(UIGenProgressBar).GetField("_texOuterCrimson", BindingFlags.Instance | BindingFlags.NonPublic));
			c.EmitDelegate<Func<Asset<Texture2D>, Asset<Texture2D>, Asset<Texture2D>>>((corrupt, crimson) =>
			{
				int worldGenStep = 0;
				if (WorldGen.crimson) worldGenStep = 1;
				if (WorldBiomeManager.WorldEvil != "") worldGenStep = ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).Type + 2;
				Asset<Texture2D> asset = ALTextureAssets.OuterTexture;
				return worldGenStep <= 1 ? (worldGenStep == 0 ? corrupt : crimson) : asset;
			});
			if (!c.TryGotoNext(i => i.MatchLdfld<UIGenProgressBar>("_texOuterCrimson")))
			{
				AltLibrary.Instance.Logger.Info("m $ 4");
				return;
			}
			c.Index++;
			c.Emit(OpCodes.Pop);
			c.Emit(OpCodes.Ldarg, 0);
			c.Emit(OpCodes.Ldfld, typeof(UIGenProgressBar).GetField("_texOuterCorrupt", BindingFlags.Instance | BindingFlags.NonPublic));
			c.Emit(OpCodes.Ldarg, 0);
			c.Emit(OpCodes.Ldfld, typeof(UIGenProgressBar).GetField("_texOuterCrimson", BindingFlags.Instance | BindingFlags.NonPublic));
			c.EmitDelegate<Func<Asset<Texture2D>, Asset<Texture2D>, Asset<Texture2D>>>((corrupt, crimson) =>
			{
				int worldGenStep = 0;
				if (WorldGen.crimson) worldGenStep = 1;
				if (WorldBiomeManager.WorldEvil != "") worldGenStep = ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).Type + 2;
				Asset<Texture2D> asset = ALTextureAssets.OuterTexture;
				return worldGenStep <= 1 ? (worldGenStep == 0 ? corrupt : crimson) : asset;
			});
			if (!c.TryGotoNext(i => i.MatchCallvirt(out _)))
			{
				AltLibrary.Instance.Logger.Info("m $ 5");
				return;
			}
			if (!c.TryGotoNext(i => i.MatchCallvirt(out _)))
			{
				AltLibrary.Instance.Logger.Info("m $ 6");
				return;
			}
			c.Index++;
			c.Emit(OpCodes.Ldarg, 1);
			c.Emit(OpCodes.Ldloc, 6);
			c.Emit(OpCodes.Ldarg, 0);
			c.Emit(OpCodes.Ldfld, typeof(UIGenProgressBar).GetField("_texOuterCorrupt", BindingFlags.Instance | BindingFlags.NonPublic));
			c.Emit(OpCodes.Ldarg, 0);
			c.Emit(OpCodes.Ldfld, typeof(UIGenProgressBar).GetField("_texOuterCrimson", BindingFlags.Instance | BindingFlags.NonPublic));
			c.EmitDelegate<Action<SpriteBatch, Rectangle, Asset<Texture2D>, Asset<Texture2D>>>((spriteBatch, r, corrupt, crimson) =>
			{
				int worldGenStep = 0;
				if (WorldGen.crimson) worldGenStep = 1;
				if (WorldBiomeManager.WorldEvil != "") worldGenStep = ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).Type + 2;
				if (WorldGen.drunkWorldGen && Main.rand.NextBool(2)) worldGenStep = Main.rand.Next(AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Evil).ToList().Count + 2);
				Asset<Texture2D> asset = ALTextureAssets.OuterTexture;
				if (worldGenStep == 0) asset = corrupt;
				if (worldGenStep == 1) asset = crimson;
				foreach (AltBiome biome in AltLibrary.Biomes)
				{
					if (worldGenStep == biome.Type + 2 && biome.BiomeType == BiomeType.Evil)
					{
						asset = ALTextureAssets.BiomeOuter[biome.Type - 1];
					}
				}
				spriteBatch.Draw(asset.Value, r.TopLeft(), Color.White);
			});
			if (!c.TryGotoNext(i => i.MatchLdfld<UIGenProgressBar>("_texOuterLower")))
			{
				AltLibrary.Instance.Logger.Info("m $ 7");
				return;
			}
			c.Index++;
			c.Emit(OpCodes.Pop);
			c.Emit(OpCodes.Ldarg, 0);
			c.Emit(OpCodes.Ldfld, typeof(UIGenProgressBar).GetField("_texOuterLower", BindingFlags.Instance | BindingFlags.NonPublic));
			c.EmitDelegate<Func<Asset<Texture2D>, Asset<Texture2D>>>((lower) =>
			{
				int worldGenStep = 0;
				if (WorldBiomeManager.WorldHell != "") worldGenStep = ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).Type + 1;
				Asset<Texture2D> asset = ALTextureAssets.OuterLowerTexture;
				return worldGenStep <= 0 ? lower : asset;
			});
			if (!c.TryGotoNext(i => i.MatchCallvirt(out _)))
			{
				AltLibrary.Instance.Logger.Info("m $ 8");
				return;
			}
			if (!c.TryGotoNext(i => i.MatchCallvirt(out _)))
			{
				AltLibrary.Instance.Logger.Info("m $ 9");
				return;
			}
			c.Index++;
			c.Emit(OpCodes.Ldarg, 1);
			c.Emit(OpCodes.Ldloc, 6);
			c.Emit(OpCodes.Ldarg, 0);
			c.Emit(OpCodes.Ldfld, typeof(UIGenProgressBar).GetField("_texOuterLower", BindingFlags.Instance | BindingFlags.NonPublic));
			c.EmitDelegate<Action<SpriteBatch, Rectangle, Asset<Texture2D>>>((spriteBatch, r, lower) =>
			{
				int worldGenStep = 0;
				if (WorldBiomeManager.WorldHell != "") worldGenStep = ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).Type + 1;
				if (WorldGen.drunkWorldGen && Main.rand.NextBool(2)) worldGenStep = Main.rand.Next(AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Hell).ToList().Count + 1);
				Asset<Texture2D> asset = ALTextureAssets.OuterLowerTexture;
				if (worldGenStep == 0) asset = lower;
				foreach (AltBiome biome in AltLibrary.Biomes)
				{
					if (worldGenStep == biome.Type + 1 && biome.BiomeType == BiomeType.Hell)
					{
						asset = ALTextureAssets.BiomeLower[biome.Type - 1];
					}
				}
				spriteBatch.Draw(asset.Value, r.TopLeft() + new Vector2(44f, 60f), Color.White);
			});
		}
	}
}
