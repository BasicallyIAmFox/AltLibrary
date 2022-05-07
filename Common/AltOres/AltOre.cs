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
        public int ore;
        public int bar;
        public bool Selectable = true;
        public virtual Color NameColor => new(255, 255, 255);

        public OreType OreType;

        internal int Type { get; set; }
        public virtual ModTranslation DisplayName
        {
            get;
            internal set;
        }
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

            AltLibrary.ores.Add(this);
            Type = AltLibrary.ores.Count;
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
