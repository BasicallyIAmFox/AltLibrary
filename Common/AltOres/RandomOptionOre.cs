using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltOres
{
	internal sealed class RandomOptionOre : AltOre
	{
		public override Color NameColor => Color.Yellow;
		public override string Name => name;

		private readonly string name;
		private readonly string display;
		public RandomOptionOre(string name, string overrideDisplay = "") : base()
		{
			this.name = name;
			display = overrideDisplay;
		}

		public override void SetStaticDefaults()
		{
			if (display != "")
			{
				DisplayName.SetDefault(display);
			}
		}

		public override bool IsLoadingEnabled(Mod mod) => false;
	}
}
