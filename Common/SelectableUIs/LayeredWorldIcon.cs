using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Assets;
using AltLibrary.Common.IO;
using AltLibrary.Common.OrderGroups;
using AltLibrary.Content.Groups;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace AltLibrary.Common.SelectableUIs;

public sealed class LayeredWorldIconElement : UIImage {
	private readonly List<Asset<Texture2D>> assets = new(8);
	private readonly bool zenith;
	private readonly SpriteEffects effects;
	private int _glitchFrame;
	private int _glitchFrameCounter;
	private int[] _glitchVariation;

	public LayeredWorldIconElement(WorldFileData data, TagCompound tagCompound) : base(Asset<Texture2D>.Empty) {
		Asset<Texture2D> treeType = LibAssets.IconNormal_Base;

		_glitchVariation = new int[3];
		_glitchVariation[0] = -1;
		if (data.ZenithWorld) {
			zenith = true;
			assets.Add(treeType);
			OnUpdate += ZenithGlitch;
			return;
		}
		else if (data.DrunkWorld && data.RemixWorld) {
			effects = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
		}
		else if (data.DrunkWorld) {
			treeType = LibAssets.IconDrunk_Base;
		}
		else if (data.ForTheWorthy) {
			treeType = LibAssets.IconForTheWorthy_Base;
		}
		else if (data.NotTheBees) {
			treeType = LibAssets.IconNotTheBees_Base;
		}
		else if (data.Anniversary) {
			treeType = LibAssets.IconAnniversary_Base;
		}
		else if (data.DontStarve) {
			treeType = LibAssets.IconDontStarve_Base;
		}
		else if (data.RemixWorld) {
			treeType = LibAssets.IconRemix_Base;
		}
		else if (data.NoTrapsWorld) {
			treeType = LibAssets.IconNoTraps_Base;
		}
		assets.Add(treeType);

		if (tagCompound.TryGet(WorldDataManager.BiomeDataKey, out Dictionary<BiomeGroup, ModTypeData<IAltBiome>> biomeGroups)) {
			if (biomeGroups.TryGetValue(ModContent.GetInstance<EvilBiomeGroup>(), out var evilAlt)) {
				var biome = evilAlt.FullName.To<IAltBiome>();
				if (biome == null) {
					goto skip;
				}

				var type = biome.Type;
				if (data.DrunkWorld) {
					assets.Add(LibAssets.IconDrunkBase_Evils[type]);
					assets.Add(LibAssets.IconDrunk_Evils[type]);
				}
				else if (data.ForTheWorthy) {
					assets.Add(LibAssets.IconForTheWorthy_Evils[type]);
				}
				else if (data.NotTheBees) {
					assets.Add(LibAssets.IconNotTheBees_Evils[type]);
				}
				else if (data.Anniversary) {
					assets.Add(LibAssets.IconAnniversary_Evils[type]);
				}
				else if (data.DontStarve) {
					assets.Add(LibAssets.IconDontStarve_Evils[type]);
				}
				else if (data.RemixWorld) {
					assets.Add(LibAssets.IconRemix_Evils[type]);
				}
				else if (data.NoTrapsWorld) {
					assets.Add(LibAssets.IconNoTraps_Evils[type]);
				}
				else {
					assets.Add(LibAssets.IconNormal_Evils[type]);
				}

			skip:
				_ = 0;
			}

			if (data.IsHardMode && biomeGroups.TryGetValue(ModContent.GetInstance<GoodBiomeGroup>(), out var goodAlt)) {
				var biome = goodAlt.FullName.To<IAltBiome>();
				if (biome == null) {
					goto skip;
				}

				var type = biome.Type;
				if (data.DrunkWorld) {
					assets.Add(LibAssets.IconDrunkBase_Goods[type]);
					assets.Add(LibAssets.IconDrunk_Goods[type]);
				}
				else if (data.ForTheWorthy) {
					assets.Add(LibAssets.IconForTheWorthy_Goods[type]);
				}
				else if (data.NotTheBees) {
					assets.Add(LibAssets.IconNotTheBees_Goods[type]);
				}
				else if (data.Anniversary) {
					assets.Add(LibAssets.IconAnniversary_Goods[type]);
				}
				else if (data.DontStarve) {
					assets.Add(LibAssets.IconDontStarve_Goods[type]);
				}
				else if (data.RemixWorld) {
					assets.Add(LibAssets.IconRemix_Goods[type]);
				}
				else if (data.NoTrapsWorld) {
					assets.Add(LibAssets.IconNoTraps_Goods[type]);
				}
				else {
					assets.Add(LibAssets.IconNormal_Goods[type]);
				}

			skip:
				_ = 0;
			}
		}
	}

	protected override void DrawSelf(SpriteBatch spriteBatch) {
		CalculatedStyle dimensions = GetDimensions();
		Vector2 vector = assets[0].Size();
		Vector2 vector2 = dimensions.Position() + vector * (1f - ImageScale) / 2f + vector * NormalizedOrigin;
		if (RemoveFloatingPointsFromDrawPosition) {
			vector2 = vector2.Floor();
		}

		if (zenith) {
			//var sortMode = spriteBatch.GetData().SortMode;
			//spriteBatch.GetData().SortMode = SpriteSortMode.Immediate;

			var shader = LibAssets.ZenithShader;
			shader.Parameters["frame"].SetValue(_glitchFrame);
			shader.CurrentTechnique.Passes[0].Apply();

			void Draw(Asset<Texture2D> asset) {
				if (!ScaleToFit)
					spriteBatch.Draw(asset.Value, new Vector2(dimensions.X, dimensions.Y), null, Color, Rotation, vector * NormalizedOrigin, new Vector2(dimensions.Width, dimensions.Height), effects, 0f);
				else
					spriteBatch.Draw(asset.Value, vector2, null, Color, Rotation, vector * NormalizedOrigin, ImageScale, effects, 0f);
			}

			Draw(assets[0]);
			switch (_glitchVariation[0]) {
				case 0:
					Draw(LibAssets.IconNormal_Evils[_glitchVariation[1]]);
					break;
				case 1:
					Draw(LibAssets.IconDrunkBase_Evils[_glitchVariation[2]]);
					Draw(LibAssets.IconNormal_Evils[_glitchVariation[1]]);
					break;
				case 2:
					Draw(LibAssets.IconZenith_GoodsL[_glitchVariation[1]]);
					break;
				case 3:
					Draw(LibAssets.IconZenith_GoodsF[_glitchVariation[1]]);
					Draw(LibAssets.IconZenith_GoodsL[_glitchVariation[2]]);
					break;
				case 4:
					Draw(LibAssets.IconZenith_GoodsF[_glitchVariation[1]]);
					Draw(LibAssets.IconNormal_Evils[_glitchVariation[2]]);
					break;
			}

			spriteBatch.End();
			//spriteBatch.GetData().SortMode = sortMode;
			return;
		}

		if (ScaleToFit) {
			foreach (var b in assets) {
				spriteBatch.Draw(b.Value, new Vector2(dimensions.X, dimensions.Y), null, Color, Rotation, vector * NormalizedOrigin, new Vector2(dimensions.Width, dimensions.Height), effects, 0f);
			}
		}

		foreach (var b in assets) {
			spriteBatch.Draw(b.Value, vector2, null, Color, Rotation, vector * NormalizedOrigin, ImageScale, effects, 0f);
		}
	}

	private void ZenithGlitch(UIElement affectedElement) {
		int minValue = 3;
		int maxValue = 3;
		if (_glitchFrame == 0) {
			minValue = 15;
			maxValue = 120;
		}

		if (++_glitchFrameCounter < Main.rand.Next(minValue, maxValue + 1)) {
			return;
		}

		_glitchFrameCounter = 0;
		_glitchFrame = (_glitchFrame + 1) % 16;

		if ((_glitchFrame == 4 || _glitchFrame == 8 || _glitchFrame == 12) && Main.rand.NextBool(3)) {
			var id = Main.rand.Next(5);

			_glitchVariation[0] = id;
			if (id == 0) {
				_glitchVariation[1] = Main.rand.Next(LibAssets.ValidZenithEvils.Count);
			}
			else if (id == 1) {
				_glitchVariation[2] = Main.rand.Next(LibAssets.ValidZenithEvils.Count);
				_glitchVariation[1] = Main.rand.Next(LibAssets.ValidZenithEvils.Count);
			}
			else if (id == 2) {
				_glitchVariation[1] = Main.rand.Next(LibAssets.ValidZenithGoods.Count);
			}
			else if (id == 3) {
				_glitchVariation[2] = Main.rand.Next(LibAssets.ValidZenithGoods.Count);
				_glitchVariation[1] = Main.rand.Next(LibAssets.ValidZenithGoods.Count);
			}
			else if (id == 4) {
				_glitchVariation[2] = Main.rand.Next(LibAssets.ValidZenithEvils.Count);
				_glitchVariation[1] = Main.rand.Next(LibAssets.ValidZenithGoods.Count);
			}
		}
	}
}
