using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria.ModLoader;
using static AltLibrary.Common.Assets.TextureFactory;

namespace AltLibrary.Common.Assets;

public class LibAssets : IPostContent {
	public static Asset<Texture2D> ShadowIcon;

	public static Asset<Texture2D>[,] PreviewIcons;

	public void Load(Mod mod) {
		ShadowIcon = CreateSingle("AltLibrary/Assets/WorldIcons/ShadowIcon");

		PreviewIcons = CreateMultidimensional(i => {
			int x = i / 3;
			int y = i % 3;
			if (x == 0) {
				return y switch {
					0 => $"Terraria/Images/UI/WorldCreation/PreviewSizeSmall",
					1 => $"Terraria/Images/UI/WorldCreation/PreviewSizeMedium",
					2 => $"Terraria/Images/UI/WorldCreation/PreviewSizeLarge",
					_ => throw new ArgumentException()
				};
			}
			return $"AltLibrary/Assets/WorldPreviews/Preview_{x - 1}_{y}";
		}, 8, 3) as Asset<Texture2D>[,];
	}

	public void Unload() {
	}
}
