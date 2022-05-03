using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltBiomes
{
    internal sealed class VanillaBiome : AltBiome
    {
        public override string IconSmall => "Terraria/Images/UI/Bestiary/Icon_Tags_Shadow";
        public override string Name => name;
        public override LocalizedText DisplayName => displayName;
        public override LocalizedText Description => desc;
        public override Color NameColor => nameColor;

        private readonly string name;
        private readonly LocalizedText displayName;
        private readonly LocalizedText desc;
        private readonly Color nameColor;
        public VanillaBiome(string name, BiomeType biome, int type, Color nameColor, LocalizedText displayName, LocalizedText desc)
        {
            this.name = name;
            if (name == "CorruptBiome") specialValueForWorldUIDoNotTouchElseYouCanBreakStuff = -1;
            if (name == "CrimsonBiome") specialValueForWorldUIDoNotTouchElseYouCanBreakStuff = -2;
            if (name == "HallowBiome") specialValueForWorldUIDoNotTouchElseYouCanBreakStuff = -3;
            if (name == "JungleBiome") specialValueForWorldUIDoNotTouchElseYouCanBreakStuff = -4;
            if (name == "UnderworldBiome") specialValueForWorldUIDoNotTouchElseYouCanBreakStuff = -5;
            BiomeType = biome;
            Type = type;
            this.displayName = displayName;
            this.desc = desc;
            this.nameColor = nameColor;
        }

        public override bool IsLoadingEnabled(Mod mod) => false;
    }
}
