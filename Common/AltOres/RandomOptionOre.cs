using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltOres
{
    internal sealed class RandomOptionOre : AltOre
    {
        public override Color NameColor => Color.Yellow;
        public override string Name => name;

        private readonly string name;
        public RandomOptionOre(string name) : base()
        {
            this.name = name;
        }

        public override bool IsLoadingEnabled(Mod mod) => false;
    }
}
