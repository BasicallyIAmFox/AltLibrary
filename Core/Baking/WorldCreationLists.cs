using AltLibrary.Common;
using AltLibrary.Common.AltOres;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Drawing;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static AltLibrary.Common.UIWorldCreationEdits;

namespace AltLibrary.Core.Baking
{
    internal class ALWorldCreationLists
    {
        internal static OreCreationList prehmOreData;
        internal static HardmodeOreCreationList hmOreData;

        internal class ALWorldCreationLists_Loader : ILoadable
        {
            public void Load(Mod mod)
            {
                prehmOreData = new();
                hmOreData = new();
            }

            public void Unload()
            {
                prehmOreData = null;
                hmOreData = null;
            }
        }

        public static void FillData()
        {
            prehmOreData.Initialize();
            hmOreData.Initialize();
        }

        internal class OreCreationList : WorldCreationList
        {
            internal List<ALOreDrawingStruct> Quenes;

            public override void Initialize()
            {
                List<AltOre> preOrder = new()
            {
                new VanillaOre("Copper", "Copper", -1, TileID.Copper, ItemID.CopperBar, OreType.Copper),
                new VanillaOre("Tin", "Tin", -2, TileID.Tin, ItemID.TinBar, OreType.Copper),
                new VanillaOre("Iron", "Iron", -3, TileID.Iron, ItemID.IronBar, OreType.Iron),
                new VanillaOre("Lead", "Lead", -4, TileID.Lead, ItemID.LeadBar, OreType.Iron),
                new VanillaOre("Silver", "Silver", -5, TileID.Silver, ItemID.SilverBar, OreType.Silver),
                new VanillaOre("Tungsten", "Tungsten", -6, TileID.Tungsten, ItemID.TungstenBar, OreType.Silver),
                new VanillaOre("Gold", "Gold", -7, TileID.Gold, ItemID.GoldBar, OreType.Gold),
                new VanillaOre("Platinum", "Platinum", -8, TileID.Platinum, ItemID.PlatinumBar, OreType.Gold)
            };
                foreach (AltOre ore in AltLibrary.Ores)
                {
                    if (ore.OreType <= OreType.Gold)
                    {
                        ore.CustomSelection(preOrder);
                    }
                }
                Ores = preOrder;

                List<ALOreDrawingStruct> quene = new()
            {
                #region Pre-HM Ores
#region Copper
                new("Terraria/Copper", false, (value) =>
            {
                return Copper switch
                {
                    >= 0 => ModContent.Request<Texture2D>(AltLibrary.Ores[Copper - 1].Texture),
                    _ => value
                };
            }, () =>
            {
                return Copper switch
                {
                    -1 => new(0, 0, 30, 30),
                    -2 => new(30, 0, 30, 30),
                    _ => null,
                };
            }, () =>
            {
                return Copper switch
                {
                    -1 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Copper"),
                    -2 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Tin"),
                    _ => AltLibrary.Ores[Copper - 1].Name
                };
            }, (mod) =>
            {
                return Copper switch
                {
                    >= 0 => AltLibrary.Ores[Copper - 1].Mod.Name,
                    _ => mod
                };
            }),
                #endregion
#region Iron
                new("Terraria/Iron", false,
                (value) =>
                {
return Iron switch
{
                        >= 0 => ModContent.Request<Texture2D>(AltLibrary.Ores[Iron - 1].Texture),
                        _ => value
                    };
                }, () =>
{
                    return Iron switch
                    {
                        -3 => new(60, 0, 30, 30),
                        -4 => new(90, 0, 30, 30),
                        _ => null,
                    };
                }, () =>
{
                    return Iron switch
                    {
                        -3 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Iron"),
                        -4 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Lead"),
_ => AltLibrary.Ores[Iron - 1].Name
                    };
                }, (mod) =>
                {
                    return Iron switch
{
                        >= 0 => AltLibrary.Ores[Iron - 1].Mod.Name,
_ => mod
                    };
                }),
                #endregion
#region Silver
                new("Terraria/Silver", false,
                (value) =>
                {
                    return Silver switch
                    {
                        >= 0 => ModContent.Request<Texture2D>(AltLibrary.Ores[Silver - 1].Texture),
                        _ => value
                    };
                }, () =>
                {
                    return Silver switch
                    {
                        -5 => new(120, 0, 30, 30),
                        -6 => new(150, 0, 30, 30),
                        _ => null,
                    };
                }, () =>
                {
                    return Silver switch
                    {
                        -5 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Silver"),
                        -6 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Tungsten"),
                        _ => AltLibrary.Ores[Silver - 1].Name
                    };
                }, (mod) =>
                {
                    return Silver switch
                    {
                        >= 0 => AltLibrary.Ores[Silver - 1].Mod.Name,
                        _ => mod
                    };
                }),
                #endregion
#region Gold
                new("Terraria/Gold", false,
                (value) =>
                {
                    return Gold switch
                    {
                        >= 0 => ModContent.Request<Texture2D>(AltLibrary.Ores[Gold - 1].Texture),
                        _ => value
                    };
                }, () =>
                {
                    return Gold switch
                    {
                        -7 => new(180, 0, 30, 30),
                        -8 => new(210, 0, 30, 30),
                        _ => null,
                    };
                }, () =>
                {
                    return Gold switch
                    {
                        -7 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Gold"),
                        -8 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Platinum"),
                        _ => AltLibrary.Ores[Gold - 1].Name
                    };
                }, (mod) =>
                {
                    return Gold switch
                    {
                        >= 0 => AltLibrary.Ores[Gold - 1].Mod.Name,
                        _ => mod
                    };
                }),
                #endregion
                #endregion
                #region HM Ores
#region Cobalt
                new("Terraria/Cobalt", true,
                (value) =>
                {
                    return Cobalt switch
                    {
                        >= 0 => ModContent.Request<Texture2D>(AltLibrary.Ores[Cobalt - 1].Texture),
                        _ => value
                    };
                }, () =>
                {
                    return Cobalt switch
                    {
                        -9 => new(0, 30, 30, 30),
                        -10 => new(30, 30, 30, 30),
                        _ => null,
                    };
                }, () =>
                {
                    return Cobalt switch
                    {
                        -9 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Cobalt"),
                        -10 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Palladium"),
                        _ => AltLibrary.Ores[Cobalt - 1].Name
                    };
                }, (mod) =>
                {
                    return Cobalt switch
                    {
                        >= 0 => AltLibrary.Ores[Cobalt - 1].Mod.Name,
                        _ => mod
                    };
                }),
                #endregion
#region Mythril
                new("Terraria/Mythril", true,
                (value) =>
                {
                    return Mythril switch
                    {
                        >= 0 => ModContent.Request<Texture2D>(AltLibrary.Ores[Mythril - 1].Texture),
                        _ => value
                    };
                }, () =>
                {
                    return Mythril switch
                    {
                        -11 => new(60, 30, 30, 30),
                        -12 => new(90, 30, 30, 30),
                        _ => null,
                    };
                }, () =>
                {
                    return Mythril switch
                    {
                        -11 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Mythril"),
                        -12 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Orichalcum"),
                        _ => AltLibrary.Ores[Mythril - 1].Name
                    };
                }, (mod) =>
                {
                    return Mythril switch
                    {
                        >= 0 => AltLibrary.Ores[Mythril - 1].Mod.Name,
                        _ => mod
                    };
                }),
                #endregion
#region Adamantite
                new("Terraria/Adamantite", true,
                (value) =>
                {
                    return Adamantite switch
                    {
                        >= 0 => ModContent.Request<Texture2D>(AltLibrary.Ores[Adamantite - 1].Texture),
                        _ => value
                    };
                }, () =>
                {
                    return Adamantite switch
                    {
                        -13 => new(120, 30, 30, 30),
                        -14 => new(150, 30, 30, 30),
                        _ => null,
                    };
                }, () =>
                {
                    return Adamantite switch
                    {
                        -13 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Adamantite"),
                        -14 => Language.GetTextValue("Mods.AltLibrary.AltOreName.Titanium"),
                        _ => AltLibrary.Ores[Adamantite - 1].Name
                    };
                }, (mod) =>
                {
                    return Adamantite switch
                    {
                        >= 0 => AltLibrary.Ores[Adamantite - 1].Mod.Name,
                        _ => mod
                    };
                })
#endregion
                #endregion
            };
                foreach (AltOre ore in AltLibrary.Ores)
                {
                    ore.AddOreOnScreenIcon(quene);
                }
                Quenes = quene;
            }
        }

        internal class HardmodeOreCreationList : WorldCreationList
        {
            public override void Initialize()
            {
                List<AltOre> preOrder = new()
            {
                new VanillaOre("Cobalt", "Cobalt", -9, TileID.Cobalt, ItemID.CobaltBar, OreType.Cobalt),
                new VanillaOre("Palladium", "Palladium", -10, TileID.Palladium, ItemID.PalladiumBar, OreType.Cobalt),
                new VanillaOre("Mythril", "Mythril", -11, TileID.Mythril, ItemID.MythrilBar, OreType.Mythril),
                new VanillaOre("Orichalcum", "Orichalcum", -12, TileID.Orichalcum, ItemID.OrichalcumBar, OreType.Mythril),
                new VanillaOre("Adamantite", "Adamantite", -13, TileID.Adamantite, ItemID.AdamantiteBar, OreType.Adamantite),
                new VanillaOre("Titanium", "Titanium", -14, TileID.Titanium, ItemID.TitaniumBar, OreType.Adamantite)
            };
                foreach (AltOre ore in AltLibrary.Ores)
                {
                    if (ore.OreType >= OreType.Cobalt)
                    {
                        ore.CustomSelection(preOrder);
                    }
                }
                Ores = preOrder;
            }
        }

        internal abstract class WorldCreationList
        {
            public List<AltOre> Ores = new();

            public abstract void Initialize();
        }
    }
}
