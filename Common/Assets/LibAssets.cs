using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Attributes;
using AltLibrary.Common.Data;
using AltLibrary.Content.Groups;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
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

	public static Asset<Texture2D>[] IconZenith_Left;
	public static Asset<Texture2D>[] IconZenith_Right;

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
		IconNormal_Evils = CreateMultiple<AltBiome<EvilBiomeGroup>, Asset<Texture2D>>(x => x.DataHandler.Get<WorldIconData>().NormalWorldIcon);
		IconNormal_Goods = CreateMultiple<AltBiome<GoodBiomeGroup>, Asset<Texture2D>>(x => x.DataHandler.Get<WorldIconData>().NormalWorldIcon);
		IconDrunk_Base = CreateSingle<Asset<Texture2D>>("AltLibrary/Assets/WorldIcons/IconDrunk");
		IconDrunk_Evils = CreateMultiple<AltBiome<EvilBiomeGroup>, Asset<Texture2D>>(x => x.DataHandler.Get<WorldIconData>().DrunkWorldIcon);
		IconDrunk_Goods = CreateMultiple<AltBiome<GoodBiomeGroup>, Asset<Texture2D>>(x => x.DataHandler.Get<WorldIconData>().DrunkWorldIcon);
		IconDrunkBase_Evils = CreateMultiple<AltBiome<EvilBiomeGroup>, Asset<Texture2D>>(x => x.DataHandler.Get<WorldIconData>().DrunkBaseWorldIcon);
		IconDrunkBase_Goods = CreateMultiple<AltBiome<GoodBiomeGroup>, Asset<Texture2D>>(x => x.DataHandler.Get<WorldIconData>().DrunkBaseWorldIcon);
		IconForTheWorthy_Base = CreateSingle<Asset<Texture2D>>("AltLibrary/Assets/WorldIcons/IconForTheWorthy");
		IconForTheWorthy_Evils = CreateMultiple<AltBiome<EvilBiomeGroup>, Asset<Texture2D>>(x => x.DataHandler.Get<WorldIconData>().ForTheWorthyWorldIcon);
		IconForTheWorthy_Goods = CreateMultiple<AltBiome<GoodBiomeGroup>, Asset<Texture2D>>(x => x.DataHandler.Get<WorldIconData>().ForTheWorthyWorldIcon);
		IconNotTheBees_Base = CreateSingle<Asset<Texture2D>>("AltLibrary/Assets/WorldIcons/IconNotTheBees");
		IconNotTheBees_Evils = CreateMultiple<AltBiome<EvilBiomeGroup>, Asset<Texture2D>>(x => x.DataHandler.Get<WorldIconData>().NotTheBeesWorldIcon);
		IconNotTheBees_Goods = CreateMultiple<AltBiome<GoodBiomeGroup>, Asset<Texture2D>>(x => x.DataHandler.Get<WorldIconData>().NotTheBeesWorldIcon);
		IconAnniversary_Base = CreateSingle<Asset<Texture2D>>("AltLibrary/Assets/WorldIcons/IconAnniversary");
		IconAnniversary_Evils = CreateMultiple<AltBiome<EvilBiomeGroup>, Asset<Texture2D>>(x => x.DataHandler.Get<WorldIconData>().Celebrationmk10WorldIcon);
		IconAnniversary_Goods = CreateMultiple<AltBiome<GoodBiomeGroup>, Asset<Texture2D>>(x => x.DataHandler.Get<WorldIconData>().Celebrationmk10WorldIcon);
		IconDontStarve_Base = CreateSingle<Asset<Texture2D>>("AltLibrary/Assets/WorldIcons/IconDontStarve");
		IconDontStarve_Evils = CreateMultiple<AltBiome<EvilBiomeGroup>, Asset<Texture2D>>(x => x.DataHandler.Get<WorldIconData>().TheConstantWorldIcon);
		IconDontStarve_Goods = CreateMultiple<AltBiome<GoodBiomeGroup>, Asset<Texture2D>>(x => x.DataHandler.Get<WorldIconData>().TheConstantWorldIcon);
		IconRemix_Base = CreateSingle<Asset<Texture2D>>("AltLibrary/Assets/WorldIcons/IconRemix");
		IconRemix_Evils = CreateMultiple<AltBiome<EvilBiomeGroup>, Asset<Texture2D>>(x => x.DataHandler.Get<WorldIconData>().DontDigUpWorldIcon);
		IconRemix_Goods = CreateMultiple<AltBiome<GoodBiomeGroup>, Asset<Texture2D>>(x => x.DataHandler.Get<WorldIconData>().DontDigUpWorldIcon);
		IconNoTraps_Base = CreateSingle<Asset<Texture2D>>("AltLibrary/Assets/WorldIcons/IconNoTraps");
		IconNoTraps_Evils = CreateMultiple<AltBiome<EvilBiomeGroup>, Asset<Texture2D>>(x => x.DataHandler.Get<WorldIconData>().NoTrapsWorldIcon);
		IconNoTraps_Goods = CreateMultiple<AltBiome<GoodBiomeGroup>, Asset<Texture2D>>(x => x.DataHandler.Get<WorldIconData>().NoTrapsWorldIcon);

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
