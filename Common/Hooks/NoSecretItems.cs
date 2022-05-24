using AltLibrary.Content.Items;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common.Hooks
{
    internal static class NoSecretItems
    {
        internal static void Load()
        {
            //HerosDetour.Load();
            //CheatSheetChanges.Load();
        }

        internal static void Unload()
        {
            //HerosDetour.Unload();
            //CheatSheetChanges.Unload();
        }

        /*[JITWhenModsEnabled("HEROsMod")]
        internal static class HerosDetour
        {
            private static Mod mod;

            static event hook_GetItems GetItems
            {
                add => HookEndpointManager.Add(mod.GetType().Assembly
                        .GetType("HEROsMod.UIKit.UIComponents.ItemBrowser")
                        .GetMethod("GetItems", BindingFlags.Public | BindingFlags.Instance), value);
                remove => HookEndpointManager.Remove(mod.GetType().Assembly
                        .GetType("HEROsMod.UIKit.UIComponents.ItemBrowser")
                        .GetMethod("GetItems", BindingFlags.Public | BindingFlags.Instance), value);
            }
            delegate Item[] hook_GetItems(orig_GetItems orig, object self);
            delegate Item[] orig_GetItems(object self);

            internal static void Load()
            {
                mod = null;
                if (!ModLoader.TryGetMod("HEROsMod", out mod))
                    return;
                GetItems += HerosDetour_GetItems;
            }

            internal static void Unload()
            {
                if (mod != null)
                {
                    GetItems -= HerosDetour_GetItems;
                }
                mod = null;
            }

            private static Item[] HerosDetour_GetItems(orig_GetItems orig, object self)
            {
                List<Item> items = orig(self).ToList();
                items.RemoveAll(x => x.type == ModContent.ItemType<HallowBunny>() || x.type == ModContent.ItemType<HallowBunnyCage>());
                return items.ToArray();
            }
        }

        [JITWhenModsEnabled("CheatSheet")]
        internal static class CheatSheetChanges
        {
            private static Mod mod;

            private static MethodInfo ParseList2 = null;
            private static event ILContext.Manipulator ModifyParseList2
            {
                add
                {
                    HookEndpointManager.Modify(ParseList2, value);
                }
                remove
                {
                    HookEndpointManager.Unmodify(ParseList2, value);
                }
            }

            private static MethodInfo ParseList1 = null;
            private static event ILContext.Manipulator ModifyParseList1
            {
                add
                {
                    HookEndpointManager.Modify(ParseList1, value);
                }
                remove
                {
                    HookEndpointManager.Unmodify(ParseList1, value);
                }
            }

            private static MethodInfo RecipeBrowser = null;
            private static event ILContext.Manipulator ModifyRecipeBrowser
            {
                add
                {
                    HookEndpointManager.Modify(RecipeBrowser, value);
                }
                remove
                {
                    HookEndpointManager.Unmodify(RecipeBrowser, value);
                }
            }

            internal static void Load()
            {
                mod = null;
                if (!ModLoader.TryGetMod("CheatSheet", out mod))
                    return;

                var p = mod.GetType().Assembly.GetType("CheatSheet.Menus.ItemBrowser");
                ParseList2 = p.GetMethod("ParseList2", BindingFlags.NonPublic | BindingFlags.Instance);
                ModifyParseList2 += CheatSheetChanges_ModifyParseList2;
                p = mod.GetType().Assembly.GetType("CheatSheet.Menus.NPCBrowser");
                ParseList1 = p.GetMethod("ParseList2", BindingFlags.NonPublic | BindingFlags.Instance);
                ModifyParseList1 += CheatSheetChanges_ModifyParseList1;
                p = mod.GetType().Assembly.GetType("CheatSheet.Menus.RecipeBrowserWindow");
                RecipeBrowser = p.GetMethod("Draw", BindingFlags.Public | BindingFlags.Instance);
                ModifyRecipeBrowser += CheatSheetChanges_ModifyRecipeBrowser;
            }

            internal static void Unload()
            {
                if (ParseList2 != null) // just one check is enough, as all fields get their values if mod present
                {
                    ModifyParseList2 -= CheatSheetChanges_ModifyParseList2;
                    ParseList2 = null;
                    ModifyParseList1 -= CheatSheetChanges_ModifyParseList1;
                    ParseList1 = null;
                    ModifyRecipeBrowser -= CheatSheetChanges_ModifyRecipeBrowser;
                    RecipeBrowser = null;
                }
                mod = null;
            }

            private static void CheatSheetChanges_ModifyRecipeBrowser(ILContext il)
            {
                ILCursor c = new(il);
                if (!c.TryGotoNext(i => i.MatchLdsfld<Recipe>(nameof(Recipe.numRecipes))))
                {
                    AltLibrary.Instance.Logger.Info("6 $ 1");
                    return;
                }

                c.Index++;
                c.EmitDelegate<Func<int, int>>((orig) => orig);

                if (!c.TryGotoNext(i => i.MatchLdarg(0),
                    i => i.MatchCall(mod.GetType().Assembly.GetType("CheatSheet.Menus.RecipeBrowserWindow").GetMethod("InitializeRecipeCategories", BindingFlags.NonPublic | BindingFlags.Instance))))
                {
                    AltLibrary.Instance.Logger.Info("6 $ 2");
                    return;
                }

                c.Emit(OpCodes.Ldsfld, mod.GetType().Assembly.GetType("CheatSheet.Menus.RecipeBrowserWindow").GetField("recipeView", BindingFlags.NonPublic | BindingFlags.Static));
                c.Emit(OpCodes.Ldfld, mod.GetType().Assembly.GetType("CheatSheet.Menus.RecipeView").GetField("allRecipeSlot", BindingFlags.Public | BindingFlags.Instance));
                c.EmitDelegate<Func<object[], object[]>>((num254) =>
                {
                    List<object> allRecipeSlot = num254.ToList();
                    for (int j = 0; j < allRecipeSlot.Count; j++)
                    {
                        if (Main.recipe[j].Mod != null && Main.recipe[j].Mod.Name == AltLibrary.Instance.Name)
                        {
                            allRecipeSlot.RemoveAt(j);
                        }
                    }
                    return allRecipeSlot.ToArray();
                });
                c.Emit(OpCodes.Ldsfld, mod.GetType().Assembly.GetType("CheatSheet.Menus.RecipeBrowserWindow").GetField("recipeView", BindingFlags.NonPublic | BindingFlags.Static));
                c.Emit(OpCodes.Stfld, mod.GetType().Assembly.GetType("CheatSheet.Menus.RecipeView").GetField("allRecipeSlot", BindingFlags.Public | BindingFlags.Instance));
            }

            private static void CheatSheetChanges_ModifyParseList1(ILContext il)
            {
                ILCursor c = new(il);
                ILLabel label = null;
                if (!c.TryGotoNext(i => i.MatchLdfld<NPC>(nameof(NPC.type)),
                    i => i.MatchLdcI4(NPCID.Count),
                    i => i.MatchBlt(out label)))
                {
                    AltLibrary.Instance.Logger.Info("5 $ 1");
                    return;
                }
                c.Index += 3;
                c.Emit(OpCodes.Ldloc, 1);
                c.EmitDelegate<Func<int, bool>>((j) =>
                {
                    return j != ModContent.NPCType<Content.NPCs.HallowBunny>();
                });
                c.Emit(OpCodes.Brfalse, label);
            }

            private static void CheatSheetChanges_ModifyParseList2(ILContext il)
            {
                ILCursor c = new(il);
                ILLabel label = null;
                if (!c.TryGotoNext(i => i.MatchLdcI4(ItemID.Count),
                    i => i.MatchBlt(out label)))
                {
                    AltLibrary.Instance.Logger.Info("4 $ 1");
                    return;
                }
                c.Index += 2;
                c.Emit(OpCodes.Ldloc, 1);
                c.EmitDelegate<Func<int, bool>>((j) =>
                {
                    return j < 5125 && j != ModContent.ItemType<HallowBunny>() && j != ModContent.ItemType<HallowBunnyCage>();
                });
                c.Emit(OpCodes.Brfalse, label);
            }
        }*/
    }
}
