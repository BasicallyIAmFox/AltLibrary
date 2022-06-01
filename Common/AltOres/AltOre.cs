using Microsoft.Xna.Framework;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Terraria.Localization;
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
        /// Whether or not this ore will appear on the selection menu.
        /// </summary>
        public bool Selectable = true;
        /// <summary>
        /// The color of this ore's name that will appear on the biome selection menu.
        /// </summary>
        public virtual Color NameColor => new(255, 255, 255);
        /// <summary>
        /// Tells the Library what ore this is an alternative to
        /// </summary>
        public OreType OreType;

        public int? Candle = null;
        public int? Watch = null;

        public int Type { get; internal set; }

        /// <summary>
        /// The name of this ore that will display on the biome selection screen.
        /// </summary>
        public ModTranslation DisplayName
        {
            get;
            private set;
        }
        /// <summary>
        /// The description for this ore that will appear on the biome selection screen.
        /// </summary>
        public ModTranslation Description
        {
            get;
            private set;
        }
        /// <summary>
        /// The 'World was blessed by ...' message. Used for hardmode ore alts.
        /// </summary>
        public ModTranslation BlessingMessage
        {
            get;
            private set;
        }
        /// <summary>
        /// Used for adamantite ore alts.
        /// </summary>
        public ModTranslation GuideHelpText
        {
            get;
            private set;
        }

        protected sealed override void Register()
        {
            ModTypeLookup<AltOre>.Register(this);

            DisplayName = (ModTranslation)typeof(LocalizationLoader).GetMethod("GetOrCreateTranslation",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static,
                new Type[] { typeof(Mod), typeof(string), typeof(bool) }).Invoke(null, new object[] { Mod, $"AltOreName.{Name}", false });
            Description = (ModTranslation)typeof(LocalizationLoader).GetMethod("GetOrCreateTranslation",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static,
                new Type[] { typeof(Mod), typeof(string), typeof(bool) }).Invoke(null, new object[] { Mod, $"AltOreDescription.{Name}", true });
            BlessingMessage = (ModTranslation)typeof(LocalizationLoader).GetMethod("GetOrCreateTranslation",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static,
                new Type[] { typeof(Mod), typeof(string), typeof(bool) }).Invoke(null, new object[] { Mod, $"AltOreBless.{Name}", true });
            GuideHelpText = (ModTranslation)typeof(LocalizationLoader).GetMethod("GetOrCreateTranslation",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static,
                new Type[] { typeof(Mod), typeof(string), typeof(bool) }).Invoke(null, new object[] { Mod, $"AltOreHelpText.{Name}", true });

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
            if (BlessingMessage.IsDefault())
            {
                BlessingMessage.SetDefault(Language.GetTextValue("Mods.AltLibrary.BlessBase", Name));
            }
        }
    }
}
