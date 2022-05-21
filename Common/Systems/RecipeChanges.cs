using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace AltLibrary.Common.Systems
{
    internal class RecipeChanges : ModSystem
    {
        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];
                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.DeerThing },
                                  new int[] { ItemID.DemoniteOre },
                                  "AltLibrary:EvilOres",
                                  ref i,
                                  ItemID.CrimtaneOre);
                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.Magiluminescence },
                                  new int[] { ItemID.DemoniteBar },
                                  "AltLibrary:EvilBars",
                                  ref i,
                                  ItemID.CrimtaneBar);
                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.OpticStaff, ItemID.PumpkinMoonMedallion },
                                  new int[] { ItemID.HallowedBar },
                                  "AltLibrary:HallowBars");
                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.DrillContainmentUnit },
                                  new int[] { ItemID.HellstoneBar },
                                  "AltLibrary:HellBars");
                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.DrillContainmentUnit, ItemID.VenomStaff, ItemID.TrueExcalibur },
                                  new int[] { ItemID.ChlorophyteBar },
                                  "AltLibrary:JungleBars");
                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.DrillContainmentUnit, ItemID.MiniNukeI, ItemID.MiniNukeII },
                                  new int[] { ItemID.ShroomiteBar },
                                  "AltLibrary:MushroomBars");
                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.PeaceCandle, ItemID.Throne, ItemID.FlinxFurCoat, ItemID.FlinxStaff },
                                  new int[] { ItemID.GoldBar },
                                  "AltLibrary:GoldBars",
                                  ref i,
                                  ItemID.PlatinumBar);
                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.FrostBreastplate, ItemID.FrostLeggings, ItemID.FrostHelmet, ItemID.AncientBattleArmorHat, ItemID.AncientBattleArmorPants, ItemID.AncientBattleArmorShirt },
                                  new int[] { ItemID.AdamantiteBar },
                                  "AltLibrary:AdamantiteBars",
                                  ref i,
                                  ItemID.TitaniumBar);

                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.NightsEdge },
                                  new int[] { ItemID.LightsBane },
                                  "AltLibrary:EvilSwords",
                                  ref i,
                                  ItemID.BloodButcherer);
                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.NightsEdge },
                                  new int[] { ItemID.BladeofGrass },
                                  "AltLibrary:JungleSwords");
                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.NightsEdge },
                                  new int[] { ItemID.FieryGreatsword },
                                  "AltLibrary:HellSwords");
                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.TerraBlade },
                                  new int[] { ItemID.TrueNightsEdge },
                                  "AltLibrary:TrueComboSwords");
                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.TerraBlade },
                                  new int[] { ItemID.TrueExcalibur },
                                  "AltLibrary:TrueHallowSwords");

                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.MonsterLasagna, ItemID.CoffinMinecart, ItemID.MechanicalWorm, ItemID.BattlePotion },
                                  new int[] { ItemID.RottenChunk },
                                  "AltLibrary:RottenChunks",
                                  ref i,
                                  ItemID.Vertebrae);
                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.MeteorStaff, ItemID.GreaterHealingPotion },
                                  new int[] { ItemID.PixieDust },
                                  "AltLibrary:PixieDusts");
                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.SuperManaPotion },
                                  new int[] { ItemID.UnicornHorn },
                                  "AltLibrary:UnicornHorns");
                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.SuperManaPotion, ItemID.GreaterHealingPotion, ItemID.BluePhasesaber, ItemID.GreenPhasesaber, ItemID.PurplePhasesaber, ItemID.RedPhasesaber, ItemID.WhitePhasesaber, ItemID.YellowPhasesaber, ItemID.OrangePhasesaber },
                                  new int[] { ItemID.CrystalShard },
                                  "AltLibrary:CrystalShards");
                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.VoidLens, ItemID.VoidVault, ItemID.ObsidianHelm, ItemID.ObsidianShirt, ItemID.ObsidianPants },
                                  new int[] { ItemID.ShadowScale },
                                  "AltLibrary:ShadowScales",
                                  ref i,
                                  ItemID.TissueSample);

                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.VoidLens, ItemID.VoidVault },
                                  new int[] { ItemID.JungleSpores },
                                  "AltLibrary:JungleSpores");
                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.GarlandHat, ItemID.GenderChangePotion, ItemID.BattlePotion, ItemID.GravitationPotion, ItemID.MagicPowerPotion, ItemID.StinkPotion, ItemID.TitanPotion },
                                  new int[] { ItemID.Deathweed },
                                  "AltLibrary:Deathweed");
                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.GarlandHat, ItemID.GenderChangePotion, ItemID.GravitationPotion, ItemID.TeleportationPotion },
                                  new int[] { ItemID.Fireblossom },
                                  "AltLibrary:Fireblossom");
                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.GarlandHat, ItemID.GenderChangePotion, ItemID.BuilderPotion, ItemID.CratePotion, ItemID.LifeforcePotion, ItemID.SpelunkerPotion },
                                  new int[] { ItemID.JungleSpores },
                                  "AltLibrary:JungleSpores");

                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.AdamantiteForge, ItemID.TitaniumForge },
                                  new int[] { ItemID.Hellforge },
                                  "AltLibrary:Hellforges");

                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.WaterCandle },
                                  new int[] { ItemID.Candle },
                                  "AltLibrary:GoldCandles",
                                  ref i,
                                  ItemID.PlatinumCandle);
                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.Timer5Second },
                                  new int[] { ItemID.CopperWatch },
                                  "AltLibrary:CopperWatches",
                                  ref i,
                                  ItemID.TinWatch);
                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.Timer3Second },
                                  new int[] { ItemID.SilverWatch },
                                  "AltLibrary:SilverWatches",
                                  ref i,
                                  ItemID.TungstenWatch);
                ReplaceRecipe(ref recipe,
                                  new int[] { ItemID.Timer1Second },
                                  new int[] { ItemID.GoldWatch },
                                  "AltLibrary:GoldWatches",
                                  ref i,
                                  ItemID.PlatinumWatch);
            }
        }

        private static void ReplaceRecipe(ref Recipe r, int[] results, int[] ingredients, string group)
        {
            foreach (int result in results)
            {
                if (r.HasResult(result))
                {
                    foreach (int ingredient in ingredients)
                    {
                        if (r.HasIngredient(ingredient))
                        {
                            r.TryGetIngredient(ingredient, out Item ing);
                            var amount = ing.stack;
                            r.RemoveIngredient(ing);
                            r.AddRecipeGroup(group, amount);
                        }
                    }
                }
            }
        }

        private static void ReplaceRecipe(ref Recipe r, int[] results, int[] ingredients, string group, ref int i, int altIng)
        {
            foreach (int result in results)
            {
                if (r.HasResult(result))
                {
                    foreach (int ingredient in ingredients)
                    {
                        if (r.HasIngredient(altIng))
                        {
                            r.DisableRecipe();
                            //i--; // not need anymore?
                        }
                        else if (r.HasIngredient(ingredient))
                        {
                            r.TryGetIngredient(ingredient, out Item ing);
                            var amount = ing.stack;
                            r.RemoveIngredient(ing);
                            r.AddRecipeGroup(group, amount);
                        }
                    }
                }
            }
        }
    }
}
