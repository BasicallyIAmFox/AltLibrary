using Microsoft.Xna.Framework;
using System;
using System.Reflection;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltBiomes
{
    internal sealed class VanillaBiome : AltBiome
    {
        public override string IconSmall => "Terraria/Images/UI/Bestiary/Icon_Tags_Shadow";
        public override string Name => name;
        public override Color NameColor => nameColor;

        private readonly string name;
        private readonly Color nameColor;
        public VanillaBiome(string name, BiomeType biome, int type, Color nameColor, bool? fix = null)
        {
            this.name = name;
            if (name == "CorruptBiome") SpecialValueForWorldUIDoNotTouchElseYouCanBreakStuff = -1;
            if (name == "CrimsonBiome") SpecialValueForWorldUIDoNotTouchElseYouCanBreakStuff = -2;
            if (name == "HallowBiome") SpecialValueForWorldUIDoNotTouchElseYouCanBreakStuff = -3;
            if (name == "JungleBiome") SpecialValueForWorldUIDoNotTouchElseYouCanBreakStuff = -4;
            if (name == "UnderworldBiome") SpecialValueForWorldUIDoNotTouchElseYouCanBreakStuff = -5;
            BiomeType = biome;
            Type = type;
            this.nameColor = nameColor;
            IsForCrimsonOrCorruptWorldUIFix = fix;
        }

        public override bool IsLoadingEnabled(Mod mod) => false;
    }
}
