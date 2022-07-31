using AltLibrary.Core.Generation;
using Microsoft.Xna.Framework;
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

		public override EvilBiomeGenerationPass GetEvilBiomeGenerationPass()
		{
			return SpecialValueForWorldUIDoNotTouchElseYouCanBreakStuff switch
			{
				-1 => corruptPass,
				_ => crimsonPass,
			};
		}

		internal static readonly EvilBiomeGenerationPass corruptPass = new CorruptionEvilBiomeGenerationPass();
		internal static readonly EvilBiomeGenerationPass crimsonPass = new CrimsonEvilBiomeGenerationPass();

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
