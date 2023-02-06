using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ModLoader;
using static AltLibrary.Common.Assets.AssetFactory;

namespace AltLibrary.Common.Assets;

public class LibAssets : IPostContent {
	#region Textures
	public static Asset<Texture2D> CloseButton;
	public static Asset<Texture2D> ShadowIcon;

	public static Asset<Texture2D>[,] PreviewIcons;
	#endregion

	#region Effects
	public static Effect ZenithShader;
	#endregion

	public void Load(Mod mod) {
		if (Main.dedServ)
			return;

		#region Textures
		CloseButton = CreateSingle<Asset<Texture2D>>("AltLibrary/Assets/Menu/ButtonClose");
		ShadowIcon = CreateSingle<Asset<Texture2D>>("AltLibrary/Assets/WorldIcons/ShadowIcon");

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

	public void Unload() {
	}
}
