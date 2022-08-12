using AltLibrary.Common.AltBiomes;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary
{
	internal static class ALTextureAssets
	{
		internal static Asset<Texture2D>[] AnimatedModIcon = new Asset<Texture2D>[4];
		internal static Asset<Texture2D> Button;
		internal static Asset<Texture2D> Button2;
		internal static Asset<Texture2D> ButtonCorrupt;
		internal static Asset<Texture2D> ButtonHallow;
		internal static Asset<Texture2D> ButtonJungle;
		internal static Asset<Texture2D> ButtonHell;
		internal static Asset<Texture2D> ButtonWarn;
		internal static Asset<Texture2D> ButtonClose;
		internal static Asset<Texture2D> OreIcons;
		internal static Asset<Texture2D> Empty;
		internal static Asset<Texture2D> Random;
		internal static Asset<Texture2D> BestiaryIcons;
		internal static Asset<Texture2D> WorldIconNormal;
		internal static Asset<Texture2D> WorldIconDrunk;
		internal static Asset<Texture2D> WorldIconDrunkCrimson;
		internal static Asset<Texture2D> WorldIconDrunkCorrupt;
		internal static Asset<Texture2D> WorldIconForTheWorthy;
		internal static Asset<Texture2D> WorldIconNotTheBees;
		internal static Asset<Texture2D> WorldIconAnniversary;
		internal static Asset<Texture2D> WorldIconDontStarve;
		internal static Asset<Texture2D> NullPreview;
		internal static Asset<Texture2D> OuterTexture;
		internal static Asset<Texture2D> OuterLowerTexture;
		internal static Asset<Texture2D>[] BiomeIconLarge;
		internal static Asset<Texture2D>[] BiomeIconSmall;
		internal static Asset<Texture2D>[] BiomeOuter;
		internal static Asset<Texture2D>[] BiomeLower;
		internal static Asset<Texture2D>[] PreviewSizes;
		internal static Asset<Texture2D>[,] PreviewSpecialSizes;
		internal static Asset<Texture2D>[] UIWorldSeedIcon;

		internal static void Load()
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			for (int i = 0; i < AnimatedModIcon.Length; i++)
			{
				AnimatedModIcon[i] = ModContent.Request<Texture2D>($"AltLibrary/Assets/Icons/AMIcon_{i}", AssetRequestMode.ImmediateLoad);
			}
			Button = ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/Button", AssetRequestMode.ImmediateLoad);
			Button2 = ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/Button2", AssetRequestMode.ImmediateLoad);
			ButtonClose = ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/ButtonClose", AssetRequestMode.ImmediateLoad);
			ButtonCorrupt = ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/ButtonCorrupt", AssetRequestMode.ImmediateLoad);
			ButtonHallow = ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/ButtonHallow", AssetRequestMode.ImmediateLoad);
			ButtonJungle = ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/ButtonJungle", AssetRequestMode.ImmediateLoad);
			ButtonHell = ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/ButtonHell", AssetRequestMode.ImmediateLoad);
			ButtonWarn = ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/ButtonWarn", AssetRequestMode.ImmediateLoad);
			OreIcons = ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/OreIcons", AssetRequestMode.ImmediateLoad);
			Empty = ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/Empty", AssetRequestMode.ImmediateLoad);
			Random = ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/Random", AssetRequestMode.ImmediateLoad);
			BestiaryIcons = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Tags_Shadow", AssetRequestMode.ImmediateLoad);
			WorldIconNormal = ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/IconNormal", AssetRequestMode.ImmediateLoad);
			WorldIconDrunk = ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/IconDrunk", AssetRequestMode.ImmediateLoad);
			WorldIconDrunkCrimson = ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/DrunkBase/Crimson", AssetRequestMode.ImmediateLoad);
			WorldIconDrunkCorrupt = ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/DrunkBase/Corruption", AssetRequestMode.ImmediateLoad);
			WorldIconForTheWorthy = ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/IconForTheWorthy", AssetRequestMode.ImmediateLoad);
			WorldIconNotTheBees = ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/IconNotTheBees", AssetRequestMode.ImmediateLoad);
			WorldIconAnniversary = ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/IconAnniversary", AssetRequestMode.ImmediateLoad);
			WorldIconDontStarve = ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/IconDontStarve", AssetRequestMode.ImmediateLoad);
			NullPreview = ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/NullBiomePreview", AssetRequestMode.ImmediateLoad);
			OuterTexture = ModContent.Request<Texture2D>("AltLibrary/Assets/Loading/Outer Empty", AssetRequestMode.ImmediateLoad);
			OuterLowerTexture = ModContent.Request<Texture2D>("AltLibrary/Assets/Loading/Outer Lower Empty", AssetRequestMode.ImmediateLoad);
			UIWorldSeedIcon = new Asset<Texture2D>[2];
			UIWorldSeedIcon[0] = ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/ShadowIcon", AssetRequestMode.ImmediateLoad);
			UIWorldSeedIcon[1] = ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/ShadowIcon2", AssetRequestMode.ImmediateLoad);
			PreviewSpecialSizes = new Asset<Texture2D>[6, 3];
			PreviewSpecialSizes[0, 0] = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewSizeSmall", AssetRequestMode.ImmediateLoad);
			PreviewSpecialSizes[0, 1] = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewSizeMedium", AssetRequestMode.ImmediateLoad);
			PreviewSpecialSizes[0, 2] = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewSizeLarge", AssetRequestMode.ImmediateLoad);
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					PreviewSpecialSizes[i + 1, j] = ModContent.Request<Texture2D>($"AltLibrary/Assets/WorldPreviews/Preview_{i}_{j}", AssetRequestMode.ImmediateLoad);
				}
			}
		}

		internal static void PostContentLoad()
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			List<Asset<Texture2D>> biomeLarge = new();
			biomeLarge.Clear();
			foreach (AltBiome biome in AltLibrary.Biomes)
			{
				string path = null;
				if (ModContent.RequestIfExists(biome.IconLarge, out Asset<Texture2D> asset))
					path = biome.Mod.Name + "/" + asset.Name;
				biomeLarge.Add(path != null ? ModContent.Request<Texture2D>(path, AssetRequestMode.ImmediateLoad) : null);
			}
			BiomeIconLarge = biomeLarge.ToArray();

			List<Asset<Texture2D>> biomeSmall = new();
			biomeSmall.Clear();
			foreach (AltBiome biome in AltLibrary.Biomes)
			{
				string path = "AltLibrary/Assets/Menu/Empty";
				if (biome.IconSmall != path && ModContent.RequestIfExists(biome.IconSmall, out Asset<Texture2D> asset))
					path = biome.Mod.Name + "/" + asset.Name;
				biomeSmall.Add(path != "AltLibrary/Assets/Menu/Empty" ? ModContent.Request<Texture2D>(path, AssetRequestMode.ImmediateLoad) : null);
			}
			BiomeIconSmall = biomeSmall.ToArray();

			List<Asset<Texture2D>> biomeOuter = new();
			biomeOuter.Clear();
			foreach (AltBiome biome in AltLibrary.Biomes)
			{
				string path = "AltLibrary/Assets/Loading/Outer Empty";
				if (biome.OuterTexture != path && ModContent.RequestIfExists(biome.OuterTexture, out Asset<Texture2D> asset))
					path = biome.Mod.Name + "/" + asset.Name;
				biomeOuter.Add(ModContent.Request<Texture2D>(path, AssetRequestMode.ImmediateLoad));
			}
			BiomeOuter = biomeOuter.ToArray();

			List<Asset<Texture2D>> biomeLower = new();
			biomeLower.Clear();
			foreach (AltBiome biome in AltLibrary.Biomes)
			{
				string path = "AltLibrary/Assets/Loading/Outer Lower Empty";
				if (biome.LowerTexture != path && ModContent.RequestIfExists(biome.LowerTexture, out Asset<Texture2D> asset))
					path = biome.Mod.Name + "/" + asset.Name;
				biomeLower.Add(ModContent.Request<Texture2D>(path, AssetRequestMode.ImmediateLoad));
			}
			BiomeLower = biomeLower.ToArray();
		}

		internal static void Unload()
		{
			AnimatedModIcon = null;
			Button = null;
			Button2 = null;
			ButtonCorrupt = null;
			ButtonHallow = null;
			ButtonJungle = null;
			ButtonHell = null;
			ButtonWarn = null;
			ButtonClose = null;
			OreIcons = null;
			Empty = null;
			Random = null;
			BestiaryIcons = null;
			WorldIconNormal = null;
			WorldIconDrunk = null;
			WorldIconDrunkCrimson = null;
			WorldIconDrunkCorrupt = null;
			WorldIconForTheWorthy = null;
			WorldIconNotTheBees = null;
			WorldIconAnniversary = null;
			WorldIconDontStarve = null;
			BiomeIconLarge = null;
			BiomeIconSmall = null;
			BiomeLower = null;
			BiomeOuter = null;
			OuterTexture = null;
			OuterLowerTexture = null;
			PreviewSizes = null;
			PreviewSpecialSizes = null;
			UIWorldSeedIcon = null;
		}
	}
}
