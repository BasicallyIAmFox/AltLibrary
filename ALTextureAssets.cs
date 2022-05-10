using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
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
        internal static Asset<Texture2D> OreIcons;
        internal static Asset<Texture2D> Empty;
        internal static Asset<Texture2D> Empty2;
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

        internal static void Load()
        {
            for (int i = 0; i < AnimatedModIcon.Length; i++)
            {
                AnimatedModIcon[i] = ModContent.Request<Texture2D>($"AltLibrary/Assets/Icons/AMIcon_{i}", AssetRequestMode.ImmediateLoad);
            }
            Button = ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/Button", AssetRequestMode.ImmediateLoad);
            Button2 = ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/Button2", AssetRequestMode.ImmediateLoad);
            ButtonCorrupt = ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/ButtonCorrupt", AssetRequestMode.ImmediateLoad);
            ButtonHallow = ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/ButtonHallow", AssetRequestMode.ImmediateLoad);
            ButtonJungle = ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/ButtonJungle", AssetRequestMode.ImmediateLoad);
            ButtonHell = ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/ButtonHell", AssetRequestMode.ImmediateLoad);
            ButtonWarn = ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/ButtonWarn", AssetRequestMode.ImmediateLoad);
            OreIcons = ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/OreIcons", AssetRequestMode.ImmediateLoad);
            Empty = ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/Empty", AssetRequestMode.ImmediateLoad);
            Empty2 = ModContent.Request<Texture2D>("AltLibrary/Assets/WorldPreviews/Empty", AssetRequestMode.ImmediateLoad);
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
        }
    }
}
