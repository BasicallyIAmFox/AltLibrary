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
                List<int> results = new() { ItemID.DeerThing };
                ReplaceRecipe(ref i, results, ItemID.DemoniteOre, ItemID.CrimtaneOre, "AltLibrary:EvilOres");

                results = new() { ItemID.MonsterLasagna, ItemID.CoffinMinecart, ItemID.MechanicalWorm, ItemID.BattlePotion };
                ReplaceRecipe(ref i, results, ItemID.RottenChunk, ItemID.Vertebrae, "AltLibrary:RottenChunks");

                #region old
                Recipe recipe = Main.recipe[i];
                if (recipe.HasResult(ItemID.Magiluminescence))
                {
                    if (recipe.HasIngredient(ItemID.CrimtaneBar))
                    {
                        recipe.RemoveRecipe();
                        i--;
                    }
                    else if (recipe.HasIngredient(ItemID.DemoniteBar))
                    {
                        recipe.TryGetIngredient(ItemID.DemoniteBar, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:EvilBars", 12);
                    }
                }
                if (recipe.HasResult(ItemID.PumpkinMoonMedallion) && recipe.HasIngredient(ItemID.HallowedBar))
                {
                    recipe.TryGetIngredient(ItemID.HallowedBar, out Item ing);
                    recipe.RemoveIngredient(ing);
                    recipe.AddRecipeGroup("AltLibrary:HallowBars", 10);
                }
                if (recipe.HasResult(ItemID.DrillContainmentUnit))
                {
                    if (recipe.HasIngredient(ItemID.ChlorophyteBar))
                    {
                        recipe.TryGetIngredient(ItemID.ChlorophyteBar, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:JungleBars", 40);
                    }
                    if (recipe.HasIngredient(ItemID.ShroomiteBar))
                    {
                        recipe.TryGetIngredient(ItemID.ShroomiteBar, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:MushroomBars", 40);
                    }
                    if (recipe.HasIngredient(ItemID.HellstoneBar))
                    {
                        recipe.TryGetIngredient(ItemID.HellstoneBar, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:HellBars", 40);
                    }
                }
                if (recipe.HasResult(ItemID.VenomStaff) && recipe.HasIngredient(ItemID.ChlorophyteBar))
                {
                    recipe.TryGetIngredient(ItemID.ChlorophyteBar, out Item ing);
                    recipe.RemoveIngredient(ing);
                    recipe.AddRecipeGroup("AltLibrary:JungleBars", 14);
                }
                if (recipe.HasResult(ItemID.TrueExcalibur) && recipe.HasIngredient(ItemID.ChlorophyteBar))
                {
                    recipe.TryGetIngredient(ItemID.ChlorophyteBar, out Item ing);
                    recipe.RemoveIngredient(ing);
                    recipe.AddRecipeGroup("AltLibrary:JungleBars", 24);
                }
                if (recipe.HasIngredient(ItemID.ShroomiteBar))
                {
                    if (recipe.HasResult(ItemID.MiniNukeI) || recipe.HasResult(ItemID.MiniNukeII))
                    {
                        recipe.TryGetIngredient(ItemID.ShroomiteBar, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:MushroomBars");
                    }
                }

                if (recipe.HasResult(ItemID.NightsEdge))
                {
                    if (recipe.HasIngredient(ItemID.BloodButcherer))
                    {
                        recipe.RemoveRecipe();
                        i--;
                    }
                    else if (recipe.HasIngredient(ItemID.LightsBane))
                    {
                        recipe.TryGetIngredient(ItemID.LightsBane, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:EvilSwords");
                    }
                    if (recipe.HasIngredient(ItemID.BladeofGrass))
                    {
                        recipe.TryGetIngredient(ItemID.BladeofGrass, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:JungleSwords");
                    }
                    if (recipe.HasIngredient(ItemID.FieryGreatsword))
                    {
                        recipe.TryGetIngredient(ItemID.FieryGreatsword, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:HellSwords");
                    }
                }
                if (recipe.HasResult(ItemID.TerraBlade))
                {
                    if (recipe.HasIngredient(ItemID.TrueNightsEdge))
                    {
                        recipe.TryGetIngredient(ItemID.TrueNightsEdge, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:TrueComboSwords");
                    }
                    if (recipe.HasIngredient(ItemID.TrueExcalibur))
                    {
                        recipe.TryGetIngredient(ItemID.TrueExcalibur, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:TrueHallowSwords");
                    }
                }

                if (recipe.HasResult(ItemID.MonsterLasagna))
                {
                    if (recipe.HasIngredient(ItemID.Vertebrae))
                    {
                        recipe.RemoveRecipe();
                        i--;
                    }
                    else if (recipe.HasIngredient(ItemID.RottenChunk))
                    {
                        recipe.TryGetIngredient(ItemID.RottenChunk, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:RottenChunks", 8);
                    }
                }
                if (recipe.HasResult(ItemID.CoffinMinecart))
                {
                    if (recipe.HasIngredient(ItemID.Vertebrae))
                    {
                        recipe.RemoveRecipe();
                        i--;
                    }
                    else if (recipe.HasIngredient(ItemID.RottenChunk))
                    {
                        recipe.TryGetIngredient(ItemID.RottenChunk, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:RottenChunks", 10);
                    }
                }
                if (recipe.HasResult(ItemID.BattlePotion))
                {
                    if (recipe.HasIngredient(ItemID.Vertebrae))
                    {
                        recipe.RemoveRecipe();
                        i--;
                    }
                    else if (recipe.HasIngredient(ItemID.RottenChunk))
                    {
                        recipe.TryGetIngredient(ItemID.RottenChunk, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:RottenChunks");
                    }
                    if (recipe.HasIngredient(ItemID.Deathweed))
                    {
                        recipe.TryGetIngredient(ItemID.Deathweed, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:Deathweed");
                    }
                }
                if (recipe.HasResult(ItemID.MechanicalWorm))
                {
                    if (recipe.HasIngredient(ItemID.Vertebrae))
                    {
                        recipe.RemoveRecipe();
                        i--;
                    }
                    else if (recipe.HasIngredient(ItemID.RottenChunk))
                    {
                        recipe.TryGetIngredient(ItemID.RottenChunk, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:RottenChunks", 6);
                    }
                }

                if (recipe.HasResult(ItemID.SuperManaPotion))
                {
                    if (recipe.HasIngredient(ItemID.CrystalShard))
                    {
                        recipe.TryGetIngredient(ItemID.CrystalShard, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:CrystalShards", 3);
                    }
                    if (recipe.HasIngredient(ItemID.UnicornHorn))
                    {
                        recipe.TryGetIngredient(ItemID.UnicornHorn, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:UnicornHorns");
                    }
                }
                if (recipe.HasResult(ItemID.GreaterHealingPotion))
                {
                    if (recipe.HasIngredient(ItemID.CrystalShard))
                    {
                        recipe.TryGetIngredient(ItemID.CrystalShard, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:CrystalShards");
                    }
                    if (recipe.HasIngredient(ItemID.PixieDust))
                    {
                        recipe.TryGetIngredient(ItemID.PixieDust, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:PixieDusts", 3);
                    }
                }
                if (recipe.HasResult(ItemID.MeteorStaff) && recipe.HasIngredient(ItemID.PixieDust))
                {
                    recipe.TryGetIngredient(ItemID.PixieDust, out Item ing);
                    recipe.RemoveIngredient(ing);
                    recipe.AddRecipeGroup("AltLibrary:PixieDusts", 10);
                }
                int[] phasesabers = new int[] { ItemID.BluePhasesaber, ItemID.GreenPhasesaber, ItemID.PurplePhasesaber, ItemID.OrangePhasesaber, ItemID.RedPhasesaber, ItemID.YellowPhaseblade };
                foreach (int j in phasesabers)
                {
                    if (recipe.HasResult(j) && recipe.HasIngredient(ItemID.CrystalShard))
                    {
                        recipe.TryGetIngredient(ItemID.CrystalShard, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:CrystalShards", 50);
                        break;
                    }
                }

                if (recipe.HasResult(ItemID.VoidLens)) // void bag
                {
                    if (recipe.HasIngredient(ItemID.TissueSample))
                    {
                        recipe.RemoveRecipe();
                        i--;
                    }
                    else if (recipe.HasIngredient(ItemID.ShadowScale))
                    {
                        recipe.TryGetIngredient(ItemID.ShadowScale, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:ShadowScales", 30);
                    }
                    if (recipe.HasIngredient(ItemID.JungleSpores))
                    {
                        recipe.TryGetIngredient(ItemID.JungleSpores, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:JungleSpores", 15);
                    }
                }
                if (recipe.HasResult(ItemID.VoidVault))
                {
                    if (recipe.HasIngredient(ItemID.TissueSample))
                    {
                        recipe.RemoveRecipe();
                        i--;
                    }
                    else if (recipe.HasIngredient(ItemID.ShadowScale))
                    {
                        recipe.TryGetIngredient(ItemID.ShadowScale, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:ShadowScales", 15);
                    }
                    if (recipe.HasIngredient(ItemID.JungleSpores))
                    {
                        recipe.TryGetIngredient(ItemID.JungleSpores, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:JungleSpores", 8);
                    }
                }

                if (recipe.HasResult(ItemID.ObsidianHelm) || recipe.HasResult(ItemID.ObsidianPants))
                {
                    if (recipe.HasIngredient(ItemID.TissueSample))
                    {
                        recipe.RemoveRecipe();
                        i--;
                    }
                    else if (recipe.HasIngredient(ItemID.ShadowScale))
                    {
                        recipe.TryGetIngredient(ItemID.ShadowScale, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:ShadowScales", 5);
                    }
                }
                if (recipe.HasResult(ItemID.ObsidianShirt))
                {
                    if (recipe.HasIngredient(ItemID.TissueSample))
                    {
                        recipe.RemoveRecipe();
                        i--;
                    }
                    else if (recipe.HasIngredient(ItemID.ShadowScale))
                    {
                        recipe.TryGetIngredient(ItemID.ShadowScale, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:ShadowScales", 10);
                    }
                }

                if (recipe.HasResult(ItemID.GarlandHat) || recipe.HasResult(ItemID.GenderChangePotion))
                {
                    if (recipe.HasIngredient(ItemID.Deathweed))
                    {
                        recipe.TryGetIngredient(ItemID.Deathweed, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:Deathweed");
                    }
                    if (recipe.HasIngredient(ItemID.Moonglow))
                    {
                        recipe.TryGetIngredient(ItemID.Moonglow, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:Moonglow");
                    }
                    if (recipe.HasIngredient(ItemID.Fireblossom))
                    {
                        recipe.TryGetIngredient(ItemID.Fireblossom, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:Fireblossom");
                    }
                }
                if (recipe.HasResult(ItemID.MagicPowerPotion))
                {
                    if (recipe.HasIngredient(ItemID.Deathweed))
                    {
                        recipe.TryGetIngredient(ItemID.Deathweed, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:Deathweed");
                    }
                    if (recipe.HasIngredient(ItemID.Moonglow))
                    {
                        recipe.TryGetIngredient(ItemID.Moonglow, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:Moonglow");
                    }
                }
                if ((recipe.HasResult(ItemID.CratePotion) || recipe.HasResult(ItemID.LifeforcePotion) || recipe.HasResult(ItemID.SpelunkerPotion) || recipe.HasResult(ItemID.BuilderPotion)) && recipe.HasIngredient(ItemID.Moonglow))
                {
                    recipe.TryGetIngredient(ItemID.Moonglow, out Item ing);
                    recipe.RemoveIngredient(ing);
                    recipe.AddRecipeGroup("AltLibrary:Moonglow");
                }
                if (recipe.HasResult(ItemID.GravitationPotion))
                {
                    if (recipe.HasIngredient(ItemID.Deathweed))
                    {
                        recipe.TryGetIngredient(ItemID.Deathweed, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:Deathweed");
                    }
                    if (recipe.HasIngredient(ItemID.Fireblossom))
                    {
                        recipe.TryGetIngredient(ItemID.Fireblossom, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:Fireblossom");
                    }
                }

                if ((recipe.HasResult(ItemID.AdamantiteForge) || recipe.HasResult(ItemID.TitaniumForge)) && recipe.HasIngredient(ItemID.Hellforge))
                {
                    recipe.TryGetIngredient(ItemID.Hellforge, out Item ing);
                    recipe.RemoveIngredient(ing);
                    recipe.AddRecipeGroup("AltLibrary:Hellforges");
                }
                if (recipe.HasResult(ItemID.FlinxFurCoat))
                {
                    if (recipe.HasIngredient(ItemID.PlatinumBar))
                    {
                        recipe.RemoveRecipe();
                        i--;
                    } 
                    else if (recipe.HasIngredient(ItemID.GoldBar))
                    {
                        recipe.TryGetIngredient(ItemID.GoldBar, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:GoldBars", 8);
                    }
                }
                if (recipe.HasResult(ItemID.FlinxStaff))
                {
                    if (recipe.HasIngredient(ItemID.PlatinumBar))
                    {
                        recipe.RemoveRecipe();
                        i--;
                    }
                    else if (recipe.HasIngredient(ItemID.GoldBar))
                    {
                        recipe.TryGetIngredient(ItemID.GoldBar, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:GoldBars", 10);
                    }
                }
                if (recipe.HasResult(ItemID.AncientBattleArmorHat) || recipe.HasResult(ItemID.FrostHelmet))
                {
                    if (recipe.HasIngredient(ItemID.TitaniumBar))
                    {
                        recipe.RemoveRecipe();
                        i--;
                    }
                    else if (recipe.HasIngredient(ItemID.AdamantiteBar))
                    {
                        recipe.TryGetIngredient(ItemID.AdamantiteBar, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:AdamantiteBars", 10);
                    }
                }
                if (recipe.HasResult(ItemID.AncientBattleArmorShirt) || recipe.HasResult(ItemID.FrostBreastplate))
                {
                    if (recipe.HasIngredient(ItemID.TitaniumBar))
                    {
                        recipe.RemoveRecipe();
                        i--;
                    }
                    else if (recipe.HasIngredient(ItemID.AdamantiteBar))
                    {
                        recipe.TryGetIngredient(ItemID.AdamantiteBar, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:AdamantiteBars", 20);
                    }
                }
                if (recipe.HasResult(ItemID.AncientBattleArmorPants) || recipe.HasResult(ItemID.FrostLeggings))
                {
                    if (recipe.HasIngredient(ItemID.TitaniumBar))
                    {
                        recipe.RemoveRecipe();
                        i--;
                    }
                    else if (recipe.HasIngredient(ItemID.AdamantiteBar))
                    {
                        recipe.TryGetIngredient(ItemID.AdamantiteBar, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:AdamantiteBars", 16);
                    }
                }
                if (recipe.HasResult(ItemID.DeerThing))
                {
                    if (recipe.HasIngredient(ItemID.CrimtaneOre))
                    {
                        recipe.RemoveRecipe();
                        i--;
                    }
                    else if (recipe.HasIngredient(ItemID.DemoniteOre))
                    {
                        recipe.TryGetIngredient(ItemID.DemoniteOre, out Item ing);
                        recipe.RemoveIngredient(ing);
                        recipe.AddRecipeGroup("AltLibrary:EvilOres", 5);
                    }
                }
                #endregion 
            }
        }

        public void ReplaceRecipe(ref int i, List<int> results, int ingredient, string group)
        {
            Recipe recipe = Main.recipe[i];
            foreach (int result in results)
            {
                if (recipe.HasResult(result) && recipe.HasIngredient(ingredient))
                {
                    recipe.TryGetIngredient(ingredient, out Item ing);
                    var amount = ing.stack;
                    recipe.RemoveIngredient(ing);
                    recipe.AddRecipeGroup(group, amount);
                }
            }  
        }
        public void ReplaceRecipe(ref int i, List<int> results, int ingredient, int altIngredient, string group)
        {
            Recipe recipe = Main.recipe[i];
            foreach (int result in results)
            {
                if (recipe.HasIngredient(altIngredient))
                {
                    recipe.RemoveRecipe();
                    i--;
                }
                else if (recipe.HasIngredient(ingredient))
                {
                    recipe.TryGetIngredient(ingredient, out Item ing);
                    var amount = ing.stack;
                    recipe.RemoveIngredient(ing);
                    recipe.AddRecipeGroup(group, amount);
                }
            }
        }
    }
}
