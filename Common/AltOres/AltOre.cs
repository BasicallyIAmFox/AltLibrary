using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltOres
{
    public abstract class AltOre : ModTexturedType
    {
        public int ore;
        public int bar;
        public bool Selectable = true;
        public virtual Color NameColor => new(255, 255, 255);

        public OreType OreType;

        internal int Type { get; set; }
        public virtual LocalizedText DisplayName
        {
            get;
            private set;
        }
        public virtual LocalizedText Description
        {
            get;
            private set;
        }

        protected sealed override void Register()
        {
            ModTypeLookup<AltOre>.Register(this);
            AltLibrary.ores.Add(this);
            Type = AltLibrary.ores.Count;
        }

        public sealed override void SetupContent()
        {
            SetStaticDefaults();
        }
    }
}
