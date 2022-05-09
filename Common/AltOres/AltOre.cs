using Microsoft.Xna.Framework;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltOres
{
    public abstract class AltOre : ModTexturedType
    {
        /// <summary>
        /// The TileID of the ore that will generate in the world.
        /// </summary>
        public int ore;
        /// <summary>
        /// The ItemID of the bar that will generate as chest loot.
        /// </summary>
        public int bar;
        /// <summary>
        /// Whether or not this biome will appear on the selection menu.
        /// </summary>
        public bool Selectable = true;
        /// <summary>
        /// The color of this biome's name that will appear on the biome selection menu.
        /// </summary>
        public virtual Color NameColor => new(255, 255, 255);
        /// <summary>
        /// Tells the Library what ore this is an alternative to
        /// </summary>
        public OreType OreType;

        internal int Type { get; set; }

        /// <summary>
        /// The name of this ore that will display on the biome selection screen.
        /// </summary>
        public virtual ModTranslation DisplayName
        {
            get;
            internal set;
        }
        /// <summary>
        /// The description for this ore that will appear on the biome selection screen.
        /// </summary>
        public virtual ModTranslation Description
        {
            get;
            internal set;
        }

        protected sealed override void Register()
        {
            ModTypeLookup<AltOre>.Register(this);

            DisplayName = (ModTranslation)typeof(LocalizationLoader).GetMethod("GetOrCreateTranslation",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static,
                new Type[] { typeof(Mod), typeof(string), typeof(bool) }).Invoke(null, new object[] { Mod, $"OreName.{Name}", false });
            Description = (ModTranslation)typeof(LocalizationLoader).GetMethod("GetOrCreateTranslation",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static,
                new Type[] { typeof(Mod), typeof(string), typeof(bool) }).Invoke(null, new object[] { Mod, $"OreDescription.{Name}", true });

            AltLibrary.Ores.Add(this);
            Type = AltLibrary.Ores.Count;
        }

        public sealed override void SetupContent()
        {
            AutoStaticDefaults();
            SetStaticDefaults();
        }

        public virtual void AutoStaticDefaults()
        {
            if (DisplayName.IsDefault())
            {
                DisplayName.SetDefault(Regex.Replace(Name, "([A-Z])", " $1").Trim());
            }
        }
    }
}
