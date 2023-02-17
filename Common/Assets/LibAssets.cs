using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Attributes;
using AltLibrary.Common.Data;
using AltLibrary.Content.Groups;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using static AltLibrary.Common.Assets.AssetFactory;

namespace AltLibrary.Common.Assets;

[LoadableContent(ContentOrder.PostContent, nameof(Load))]
public static class LibAssets {
	#region Textures
	public static Asset<Texture2D> CloseButton;
	public static Asset<Texture2D> ShadowIcon;

	public static Asset<Texture2D> IconNormal_Base;
	public static Asset<Texture2D> IconDrunk_Base;
	public static Asset<Texture2D> IconForTheWorthy_Base;
	public static Asset<Texture2D> IconNotTheBees_Base;
	public static Asset<Texture2D> IconAnniversary_Base;
	public static Asset<Texture2D> IconDontStarve_Base;
	public static Asset<Texture2D> IconRemix_Base;
	public static Asset<Texture2D> IconNoTraps_Base;

	public static Asset<Texture2D>[] IconNormal_Evils;
	public static Asset<Texture2D>[] IconNormal_Goods;
	public static Asset<Texture2D>[] IconDrunk_Evils;
	public static Asset<Texture2D>[] IconDrunk_Goods;
	public static Asset<Texture2D>[] IconDrunkBase_Evils;
	public static Asset<Texture2D>[] IconDrunkBase_Goods;
	public static Asset<Texture2D>[] IconForTheWorthy_Evils;
	public static Asset<Texture2D>[] IconForTheWorthy_Goods;
	public static Asset<Texture2D>[] IconNotTheBees_Evils;
	public static Asset<Texture2D>[] IconNotTheBees_Goods;
	public static Asset<Texture2D>[] IconAnniversary_Evils;
	public static Asset<Texture2D>[] IconAnniversary_Goods;
	public static Asset<Texture2D>[] IconDontStarve_Evils;
	public static Asset<Texture2D>[] IconDontStarve_Goods;
	public static Asset<Texture2D>[] IconRemix_Evils;
	public static Asset<Texture2D>[] IconRemix_Goods;
	public static Asset<Texture2D>[] IconNoTraps_Evils;
	public static Asset<Texture2D>[] IconNoTraps_Goods;

	public static Asset<Texture2D>[] IconZenith_GoodsF;
	public static Asset<Texture2D>[] IconZenith_GoodsL;
	internal static List<int> ValidZenithEvils;
	internal static List<int> ValidZenithGoods;

	public static Asset<Texture2D>[,] PreviewIcons;
	#endregion

	#region Effects
	public static Effect ZenithShader;
	#endregion

	public static void Load() {
		if (Main.dedServ)
			return;

		#region Textures
		CloseButton = CreateSingle<Asset<Texture2D>>("AltLibrary/Assets/Menu/ButtonClose");
		ShadowIcon = CreateSingle<Asset<Texture2D>>("AltLibrary/Assets/WorldIcons/ShadowIcon");

		IconNormal_Base = CreateSingle<Asset<Texture2D>>("AltLibrary/Assets/WorldIcons/IconNormal");
		IconDrunk_Base = CreateSingle<Asset<Texture2D>>("AltLibrary/Assets/WorldIcons/IconDrunk");
		IconForTheWorthy_Base = CreateSingle<Asset<Texture2D>>("AltLibrary/Assets/WorldIcons/IconForTheWorthy");
		IconNotTheBees_Base = CreateSingle<Asset<Texture2D>>("AltLibrary/Assets/WorldIcons/IconNotTheBees");
		IconAnniversary_Base = CreateSingle<Asset<Texture2D>>("AltLibrary/Assets/WorldIcons/IconAnniversary");
		IconDontStarve_Base = CreateSingle<Asset<Texture2D>>("AltLibrary/Assets/WorldIcons/IconDontStarve");
		IconRemix_Base = CreateSingle<Asset<Texture2D>>("AltLibrary/Assets/WorldIcons/IconRemix");
		IconNoTraps_Base = CreateSingle<Asset<Texture2D>>("AltLibrary/Assets/WorldIcons/IconNoTraps");

		ValidZenithEvils = ValidZenithGoods = new();
		IconNormal_Evils = IconNormal_Goods =
			IconDrunk_Evils = IconDrunk_Goods =
			IconDrunkBase_Evils = IconDrunkBase_Goods =
			IconForTheWorthy_Evils = IconForTheWorthy_Goods =
			IconNotTheBees_Evils = IconNotTheBees_Goods =
			IconAnniversary_Evils = IconAnniversary_Goods =
			IconDontStarve_Evils = IconDontStarve_Goods =
			IconRemix_Evils = IconRemix_Goods =
			IconNoTraps_Evils = IconNoTraps_Goods =
			IconZenith_GoodsL = IconZenith_GoodsF = new Asset<Texture2D>[IAltBiome.altBiomes.Count];

		for (int i = 0; i < IAltBiome.altBiomes.Count; i++) {
			var biome = IAltBiome.altBiomes[i];
			if (biome is AltBiome<EvilBiomeGroup>) {
				var data = biome.DataHandler.Get<WorldIconData>();
				IconNormal_Evils[i] = CreateSingle<Asset<Texture2D>>(data.NormalWorldIcon);
				IconDrunk_Evils[i] = CreateSingle<Asset<Texture2D>>(data.DrunkWorldIcon);
				IconDrunkBase_Evils[i] = CreateSingle<Asset<Texture2D>>(data.DrunkBaseWorldIcon);
				IconForTheWorthy_Evils[i] = CreateSingle<Asset<Texture2D>>(data.ForTheWorthyWorldIcon);
				IconAnniversary_Evils[i] = CreateSingle<Asset<Texture2D>>(data.Celebrationmk10WorldIcon);
				IconDontStarve_Evils[i] = CreateSingle<Asset<Texture2D>>(data.TheConstantWorldIcon);
				IconRemix_Evils[i] = CreateSingle<Asset<Texture2D>>(data.DontDigUpWorldIcon);
				IconNoTraps_Evils[i] = CreateSingle<Asset<Texture2D>>(data.NoTrapsWorldIcon);
				ValidZenithEvils.Add(i);
			}
			else if (biome is AltBiome<GoodBiomeGroup>) {
				var data = biome.DataHandler.Get<WorldIconData>();
				IconNormal_Goods[i] = CreateSingle<Asset<Texture2D>>(data.NormalWorldIcon);
				IconDrunk_Goods[i] = CreateSingle<Asset<Texture2D>>(data.DrunkWorldIcon);
				IconForTheWorthy_Goods[i] = CreateSingle<Asset<Texture2D>>(data.ForTheWorthyWorldIcon);
				IconAnniversary_Goods[i] = CreateSingle<Asset<Texture2D>>(data.Celebrationmk10WorldIcon);
				IconDontStarve_Goods[i] = CreateSingle<Asset<Texture2D>>(data.TheConstantWorldIcon);
				IconRemix_Goods[i] = CreateSingle<Asset<Texture2D>>(data.DontDigUpWorldIcon);
				IconNoTraps_Goods[i] = CreateSingle<Asset<Texture2D>>(data.NoTrapsWorldIcon);
				IconZenith_GoodsL[i] = CreateSingle<Asset<Texture2D>>(data.GetFixedBoiLeftWorldIcon);
				IconZenith_GoodsF[i] = CreateSingle<Asset<Texture2D>>(data.GetFixedBoiFullWorldIcon);
				ValidZenithGoods.Add(i);
			}
		}

		PreviewIcons = CreateMultidimensional<Asset<Texture2D>>(i => {
			int x = i / 3;
			int y = i % 3;
			if (x == 0) {
				return y switch {
					0 => "Terraria/Images/UI/WorldCreation/PreviewSizeSmall",
					1 => "Terraria/Images/UI/WorldCreation/PreviewSizeMedium",
					2 => "Terraria/Images/UI/WorldCreation/PreviewSizeLarge",
					_ => throw new ArgumentException()
				};
			}
			return $"AltLibrary/Assets/WorldPreviews/Preview_{x - 1}_{y}";
		}, 8, 3) as Asset<Texture2D>[,];
		#endregion

		#region Effects
		ZenithShader = CreateSingle<Effect>("AltLibrary/Assets/Shaders/ZenithShader");
		#endregion
	}
}
