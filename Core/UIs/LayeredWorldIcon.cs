using AltLibrary.Common.AltBiomes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.UI;

namespace AltLibrary.Core.UIs {
	internal class LayeredWorldIcon : UIImage
	{
		private readonly List<Asset<Texture2D>> assets = new(8);
		private readonly bool zenith;
		private int _glitchFrame;
		private int _glitchFrameCounter;
		private int _glitchVariation;

		internal LayeredWorldIcon(WorldFileData data, AltLibraryConfig.WorldDataValues worldDataValues) : base(Asset<Texture2D>.Empty)
		{
			Asset<Texture2D> treeType = ALTextureAssets.WorldIconNormal;
			string path = "AltLibrary/Assets/WorldIcons/";
			string extra = data.DrunkWorld ? "Drunk/" : "Normal/";
			if (data.ZenithWorld)
			{
				zenith = true;
				assets.Add(treeType);
				OnUpdate += ZenithGlitch;
				return;
			}
			else if (data.ForTheWorthy)
			{
				treeType = ALTextureAssets.WorldIconForTheWorthy;
				extra = "ForTheWorthy/";
			}
			else if (data.NotTheBees)
			{
				treeType = ALTextureAssets.WorldIconNotTheBees;
				extra = "NotTheBees/";
			}
			else if (data.Anniversary)
			{
				treeType = ALTextureAssets.WorldIconAnniversary;
				extra = "Anniversary/";
			}
			else if (data.DontStarve)
			{
				treeType = ALTextureAssets.WorldIconDontStarve;
				extra = "DontStarve/";
			}
			else if (data.RemixWorld) {
				treeType = ALTextureAssets.WorldIconRemixWorld;
				extra = "Remix/";
			}
			else if (data.NoTrapsWorld)
			{
				treeType = ALTextureAssets.WorldIconNoTrapsWorld;
				extra = "Traps/";
			}

			Asset<Texture2D> FindOrReplace(string fullname, Asset<Texture2D> nullAsset)
			{
				if (ModContent.TryFind(fullname, out AltBiome biome))
					return ModContent.Request<Texture2D>(biome.WorldIcon + extra[..^1]);
				return nullAsset;
			}

			assets.Add(treeType);

			if (data.DrunkWorld)
			{
				extra = "DrunkBase/";

				if (worldDataValues.drunkEvil != null && worldDataValues.drunkEvil != string.Empty)
					assets.Add(FindOrReplace(worldDataValues.drunkEvil, ModContent.Request<Texture2D>(path + "NullBiome/NullBiomeDrunkBase")));
				else if (data.HasCorruption)
					assets.Add(ModContent.Request<Texture2D>(path + extra + "Corrupt"));
				else
					assets.Add(ModContent.Request<Texture2D>(path + extra + "Crimson"));

				extra = "Drunk/";
			}

			if (worldDataValues.worldEvil != null && worldDataValues.worldEvil != string.Empty)
				assets.Add(FindOrReplace(worldDataValues.worldEvil, ModContent.Request<Texture2D>($"{path}NullBiome/NullBiome{extra[..^1]}")));
			else if (data.HasCorruption)
				assets.Add(ModContent.Request<Texture2D>(path + extra + "Corrupt"));
			else
				assets.Add(ModContent.Request<Texture2D>(path + extra + "Crimson"));

			if (data.IsHardMode)
			{
				if (worldDataValues.worldHallow != null && worldDataValues.worldHallow != string.Empty)
					assets.Add(FindOrReplace(worldDataValues.worldHallow, ModContent.Request<Texture2D>(path + "NullBiome/NullBiomeDrunkBase")));
				else
					assets.Add(ModContent.Request<Texture2D>(path + extra + "Hallow"));
			}
		}

		private void ZenithGlitch(UIElement affectedElement)
		{
			int minValue = 3;
			int num = 3;
			if (_glitchFrame == 0)
			{
				minValue = 15;
				num = 120;
			}
			int num2 = _glitchFrameCounter + 1;
			_glitchFrameCounter = num2;
			if (num2 >= Main.rand.Next(minValue, num + 1))
			{
				_glitchFrameCounter = 0;
				_glitchFrame = (_glitchFrame + 1) % 16;

				if ((_glitchFrame == 4 || _glitchFrame == 8 || _glitchFrame == 12) && Main.rand.NextBool(3))
				{
					_glitchVariation = Main.rand.Next(ALTextureAssets.WorldZenith.Length);
				}
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			var a = assets[0];
			Vector2 vector = a.Size();
			Vector2 vector2 = dimensions.Position() + vector * (1f - ImageScale) / 2f + vector * NormalizedOrigin;
			if (RemoveFloatingPointsFromDrawPosition)
			{
				vector2 = vector2.Floor();
			}

			if (zenith)
			{
				spriteBatch.GetParameters(out SpriteSortMode sortMode, out BlendState blendState, out SamplerState samplerState, out DepthStencilState depthStencilState, out RasterizerState rasterizerState, out Effect effect, out Matrix transformationMatrix);
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, blendState, samplerState, depthStencilState, rasterizerState, effect, transformationMatrix);
				Effect shader = AltLibrary.GetEffect();
				shader.Parameters["frame"].SetValue(_glitchFrame);
				shader.CurrentTechnique.Passes[0].Apply();

				void Draw(Asset<Texture2D> asset)
				{
					if (!ScaleToFit)
						spriteBatch.Draw(asset.Value, dimensions.ToRectangle(), Color);
					else
						spriteBatch.Draw(asset.Value, vector2, null, Color, Rotation, vector * NormalizedOrigin, ImageScale, SpriteEffects.None, 0f);
				}

				Draw(a);
				Draw(ALTextureAssets.WorldZenith[_glitchVariation]);
				Draw(ALTextureAssets.WorldZenith2[_glitchVariation]);

				spriteBatch.End();
				spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformationMatrix);
				return;
			}

			if (ScaleToFit)
			{
				foreach (var b in assets)
				{
					spriteBatch.Draw(b.Value, dimensions.ToRectangle(), Color);
				}
			}
			else
			{
				foreach (var b in assets)
				{
					spriteBatch.Draw(b.Value, vector2, null, Color, Rotation, vector * NormalizedOrigin, ImageScale, SpriteEffects.None, 0f);
				}
			}
		}
	}
}
