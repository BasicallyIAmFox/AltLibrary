using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltBiomes
{
    public abstract class AltBiome : ModType
    {
        internal int SpecialValueForWorldUIDoNotTouchElseYouCanBreakStuff { get; set; }
        internal bool? IsForCrimsonOrCorruptWorldUIFix { get; set; }
        /// <summary>
        /// Tells the Library what biome this is an alternative to
        /// </summary>
        public BiomeType BiomeType { get; set; }
        public ModBiome Biome { get; set; }
        internal int Type { get; set; }

        /// <summary>
        /// The name of this biome that will display on the biome selection screen.
        /// </summary>
        public virtual ModTranslation DisplayName
        {
            get;
            internal set;
        }
        /// <summary>
        /// The description for this biome that will appear on the biome selection screen.
        /// </summary>
        public virtual ModTranslation Description
        {
            get;
            internal set;
        }

        #region Dungeon Loot
        /// <summary>
        /// For Underworld alts. If your biome uses a different kind of locked chest than a Shadow Chest, set this field to your equivalent to a Shadow Key so that it may appear in the Dungeon
        /// </summary>
        public int? ShadowKeyAlt = null;
        
        /// <summary>
        /// For Jungle, Evil, and Hallow alts. The ItemID for the rare item that will be found inside this biome's dungeon chest.
        /// </summary>
        public int? BiomeChestItem = null;
        /// <summary>
        /// For Jungle, Evil, and Hallow alts. The TileID of the special biome chest which will generate in the dungeon.
        /// </summary>
        public int? BiomeChestTile = null;
        /// <summary>
        /// For Jungle, Evil, and Hallow alts. The style number of the biome chest tile to be placed. Defaults to 0.
        /// </summary>
        public int? BiomeChestTileStyle = 0;
        #endregion

        #region Blocks and Convertables
        /// <summary>
        /// For Jungle and Evil alts. The TileID of the biome's associated ore.
        /// For Jungle alts, this block will begin to appear during hardmode and slow down the spread of nearby evil biome blocks. 
        /// </summary>
        public int? BiomeOre = null;
        /// <summary>
        /// For Jungle and Evil alts. The TileID of the biome's associated ore brick.
        /// For Jungle alts, this block will slow down the spread of nearby evil biome blocks. 
        /// For Evil alts, this is the block that will surround the loot of the Underworld boss. (WoF or Alt)
        /// </summary>
        public int? BiomeOreBrick = null;
        /// <summary>
        /// For Jungle, Evil, and Hallow alts. The TileID of the biome's grass block, which will also spread to dirt in a 1 block radius.
        /// For Evil and Hallow alts, this is the block that grass blocks will be converted into.
        /// For Jungle alts, this will block will spread to mud instead of dirt.
        /// </summary>
        public int? BiomeGrass = null;
        /// <summary>
        /// For Hallow alts. The TileID of the biome's mowed grass block. 
        /// You can also use this field for Evil or Jungle alts if you desire to make the grass able to be mowed.
        /// </summary>
        public int? BiomeMowedGrass = null;

        /// <summary>
        /// For Evil and Hallow alts. The tile which convertable stone will be turned into.
        /// </summary>
        public int? BiomeStone = null;
        /// <summary>
        /// For Evil and Hallow alts. The tile which convertable sand will be turned into.
        /// </summary>
        public int? BiomeSand = null;
        /// <summary>
        /// For Evil and Hallow alts. The tile which convertable sandstone will be turned into.
        /// </summary>
        public int? BiomeSandstone = null;
        /// <summary>
        /// For Evil and Hallow alts. The tile which convertable hardened sand will be turned into.
        /// </summary>
        public int? BiomeHardenedSand = null;
        /// <summary>
        /// For Evil and Hallow alts. The tile which convertable ice will be turned into.
        /// </summary>
        public int? BiomeIce = null;

        /// <summary>
        /// For Evil alts. Whether or not this biome will turn mud into dirt, as the Corruption and Crimson do. Defaults to false.
        /// </summary>
        public bool MudToDirt => false;
        /// <summary>
        /// For Evil and Hallow alts. The tile that this biome will convert jungle grass into. Leave null to not convert Jungle grass.
        /// For Evil alts, this should be the same as biomeGrass.
        /// For Hallow alts, this should remain null.
        /// DO NOT USE FOR JUNGLE ALTS.
        /// </summary>
        public int? BiomeJungleGrass = null;
        /// <summary>
        /// For Evil and Hallow alts. The tile that this biome will convert thorn bushes into.
        /// Leave as is if your Spreading biome does not have thorn bushes.
        /// </summary>
        public int? BiomeThornBush = null;

        /// <summary>
        /// For Evil and Hallow alts. The list of tiles that can spread this biome. 
        /// This should generally include the biomeGrass, biomeStone, etc blocks, but they can be omitted if you for some reason do not wish for those blocks to spread the biome.
        /// </summary>
        public virtual List<int> SpreadingTiles => new();
        /// <summary>
        /// For Evil and Hallow alts. You may list additional tiles that this biome can convert into its own blocks.
        /// The first value is the pure tile, the second value is its infected counterpart.
        /// </summary>
        public virtual Dictionary<int, int> SpecialConversion => new();

        /// <summary>
        /// For Evil alts, this is the TileID of this biome's equivalent to Demon Altars.
        /// For Underworld alts, this is the TileID of this biome's equivalent to Hellforges.
        /// </summary>
        public int? AltarTile = null;

        /// <summary>
        /// For Jungle alts, if you use a mechanic similar to Plantera's bulb, list the TileID of that tile here.
        /// </summary>
        public int? BossBulb = null;

        public virtual WallContext WallContext => new();

        public virtual List<int> HardmodeWalls => new();
        #endregion

        #region Blood Moon
        /// <summary>
        /// For Evil alts, the NPCID of the creature Bunnies will turn into during a blood moon.
        /// </summary>
        public int? BloodBunny = null;
        /// <summary>
        /// For Evil alts, the NPCID of the creature Goldfish will turn into during a blood moon.
        /// </summary>
        public int? BloodGoldfish = null;
        /// <summary>
        /// For Evil alts, the NPCID of the creature Penguins will turn into during a blood moon.
        /// </summary>
        public int? BloodPenguin = null;
        #endregion

        #region Mimic
        /// <summary>
        /// For Evil and Hallow alts. The NPC ID to be spawned when placing a Key of Night or Light respectively in a chest.
        /// </summary>
        public int? MimicType = null;
        /// <summary>
        /// For Evil and Hallow alts. The NPC ID to be spawned when placing a Key of Night or Light respectively in a chest.
        /// </summary>
        public int? MimicKeyType = null;
        #endregion

        #region Boss Loot
        /// <summary>
        /// For Hallow alts. The ItemID of the metal that the mechanical bosses will drop in place of Hallowed Bars.
        /// </summary>
        public int? MechDropItemType = null;
        /// <summary>
        /// For Hallow alts. The ItemID of your biome's counterpart to the Pwnhammer, if it has one. 
        /// </summary>
        public int HammerType = ItemID.Pwnhammer;

        /// <summary>
        /// For Evil Alts. The ItemID of the seeds that Eye of Cthulhu will drop in worlds with this biome.
        /// </summary>
        public int? SeedType = null;
        /// <summary>
        /// For Evil Alts. The ItemID of the ore that Eye of Cthulhu will drop in worlds with this biome.
        /// </summary>
        public int? BiomeOreItem = null;
        /// <summary>
        /// For Evil Alts. In Corruption worlds, EoC will drop Unholy Arrows, which the Crimson has no equivalent for. 
        /// If you have a comparable item you wish for EoC to drop, define it here.
        /// </summary>
        public int? ArrowType = null;
        #endregion

        #region Menu Graphics
        /// <summary>
        /// The path to the texture of the large icon that will appear on the biome selection screen to represent this biome.
        /// </summary>
        public virtual string IconLarge => null;
        /// <summary>
        /// The path to the 30x30 texture of the small icon that will appear on the biome selection screen to represent this biome. This should typically be the same as the biome's main bestiary icon.
        /// </summary>
        public virtual string IconSmall => null;

        /// <summary>
        /// The path to the texture that will serve as one of the layers of the tree on the world selection screen.
        /// </summary>
        public virtual string WorldIcon => "AltLibrary/Assets/WorldIcons/NullBiome/NullBiome";
        /// <summary>
        /// For Evil biomes. The texture that appears around the loading bar on world creation.
        /// </summary>
        public virtual string OuterTexture => "AltLibrary/Assets/Loading/Outer Empty";
        /// <summary>
        /// For Underworld biomes. The texture that appears around the lower loading bar on world creation.
        /// </summary>
        public virtual string LowerTexture => "AltLibrary/Assets/Loading/Outer Lower Empty";// TODO: create a default/template sprite to make the bar look less ugly when no bar is specified
        /// <summary>
        /// For Evil biomes. The texture that appears inside the loading bar on world creation.
        /// </summary>
        public virtual Color OuterColor => new(127, 127, 127);
        /// <summary>
        /// The color of this biome's name that will appear on the biome selection menu.
        /// </summary>
        public virtual Color NameColor => new(255, 255, 255);
        /// <summary>
        /// Whether or not this biome will appear on the selection menu.
        /// </summary>
        public bool Selectable = true;
        #endregion

        /// <summary>
        /// In CelebrationMk10 worlds, sky islands may generate as the world's Hallow biome, with a corresponding water fountain.
        /// Set this field to the TileID of your biome's water fountain if you are making a Hallow alt.
        /// </summary>
        public int? FountainTile = null;
        /// <summary>
        /// If your Hallow alt's water fountain is part of a larger tilesheet, specify the style of the appropriate water fountain here.
        /// </summary>
        public int? FountainTileStyle = null;
        /// <summary>
        /// If your Hallow alt's water fountain is part of a larger tilesheet, specify the x frame of the appropriate active water fountain here.
        /// </summary>
        public int? FountainActiveFrameX = null;
        /// <summary>
        /// If your Hallow alt's water fountain is part of a larger tilesheet, specify the y frame of the appropriate active water fountain here.
        /// </summary>
        public int? FountainActiveFrameY = null;

        public virtual Color AltUnderworldColor => Color.Black;
        public virtual Asset<Texture2D>[] AltUnderworldBackgrounds => new Asset<Texture2D>[14];

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

        protected sealed override void Register()
        {
            ModTypeLookup<AltBiome>.Register(this);

            DisplayName = (ModTranslation)typeof(LocalizationLoader).GetMethod("GetOrCreateTranslation",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static,
                new Type[] { typeof(Mod), typeof(string), typeof(bool) }).Invoke(null, new object[] { Mod, $"AltBiomeName.{Name}", false });
            Description = (ModTranslation)typeof(LocalizationLoader).GetMethod("GetOrCreateTranslation",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static,
                new Type[] { typeof(Mod), typeof(string), typeof(bool) }).Invoke(null, new object[] { Mod, $"AltBiomeDescription.{Name}", true });

            AltLibrary.Biomes.Add(this);
            if (BossBulb != null) AltLibrary.planteraBulbs.Add((int)BossBulb);
            if (BiomeType == BiomeType.Jungle)
            {
                if (BiomeGrass != null)
                {
                    AltLibrary.jungleGrass.Add((int)BiomeGrass);
                } else
                {
                    if (BiomeJungleGrass != null) AltLibrary.jungleGrass.Add((int)BiomeJungleGrass);
                }
                if (BiomeMowedGrass != null) AltLibrary.jungleGrass.Add((int)BiomeGrass);
                if (BiomeThornBush != null) AltLibrary.jungleThorns.Add((int)BiomeGrass);
                if (BiomeOre != null) AltLibrary.evilStoppingOres.Add((int)BiomeOre);
                if (BiomeOreBrick != null) AltLibrary.evilStoppingOres.Add((int)BiomeOreBrick);
            }
            AltLibrary.HellAltTrans.Add(FullName, 0f);
            Type = AltLibrary.Biomes.Count;
        }
    }

    public class WallContext
    {
        public Dictionary<ushort, ushort> wallsReplacement;

        public WallContext()
        {
            wallsReplacement = new Dictionary<ushort, ushort>();
        }

        public WallContext AddReplacement(ushort orig, ushort with)
        {
            wallsReplacement.Add(orig, with);
            return this;
        }
    }
}
