using AltLibrary.Content.Items;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace AltLibrary.Common.Hooks
{
    internal static class NoSecretItems
    {
        internal static void Load()
        {
            HerosDetour.Load();
            CheatSheetChanges.Load();
        }

        internal static void Unload()
        {
            HerosDetour.Unload();
            CheatSheetChanges.Unload();
        }

        [JITWhenModsEnabled("HEROsMod")]
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

            private static MethodInfo NPCViewSet = null;
            private static event ILContext.Manipulator ModifyNPCViewSet
            {
                add => HookEndpointManager.Modify(NPCViewSet, value);
                remove => HookEndpointManager.Unmodify(NPCViewSet, value);
            }
            private static MethodInfo ItemViewSet = null;
            private static event ILContext.Manipulator ModifyItemViewSet
            {
                add => HookEndpointManager.Modify(ItemViewSet, value);
                remove => HookEndpointManager.Unmodify(ItemViewSet, value);
            }
            private static ConstructorInfo RecipeViewSet = null;
            private static event ILContext.Manipulator ModifyRecipeViewSet
            {
                add => HookEndpointManager.Modify(RecipeViewSet, value);
                remove => HookEndpointManager.Unmodify(RecipeViewSet, value);
            }

            internal static void Load()
            {
                mod = null;
                if (!ModLoader.TryGetMod("CheatSheet", out mod))
                    return;

                var p = mod.GetType().Assembly.GetType("CheatSheet.Menus.NPCView");
                NPCViewSet = p.GetMethod("set_selectedCategory", BindingFlags.Public | BindingFlags.Instance);
                ModifyNPCViewSet += CheatSheetChanges_ModifyNPCViewSet;

                p = mod.GetType().Assembly.GetType("CheatSheet.Menus.ItemView");
                ItemViewSet = p.GetMethod("set_selectedCategory", BindingFlags.Public | BindingFlags.Instance);
                ModifyItemViewSet += CheatSheetChanges_ModifyItemViewSet;

                p = mod.GetType().Assembly.GetType("CheatSheet.Menus.RecipeView");
                RecipeViewSet = p.GetConstructor(BindingFlags.Public | BindingFlags.Instance, Array.Empty<Type>());
                ModifyRecipeViewSet += CheatSheetChanges_ModifyRecipeViewSet;
            }

            internal static void Unload()
            {
                if (NPCViewSet != null)
                {
                    ModifyNPCViewSet -= CheatSheetChanges_ModifyNPCViewSet;
                    ModifyItemViewSet -= CheatSheetChanges_ModifyItemViewSet;
                    ItemViewSet = null;
                    ModifyRecipeViewSet -= CheatSheetChanges_ModifyRecipeViewSet;
                    RecipeViewSet = null;
                    NPCViewSet = null;
                }
                mod = null;
            }

            private static void CheatSheetChanges_ModifyRecipeViewSet(ILContext il)
            {
                ILCursor c = new(il);
                if (!c.TryGotoNext(i => i.MatchRet()))
                {
                    AltLibrary.Instance.Logger.Info("6 $ 1");
                    return;
                }

                c.Emit(OpCodes.Ldarg, 0);
                c.Emit(OpCodes.Ldarg, 0);
                c.Emit(OpCodes.Ldfld, mod.GetType().Assembly.GetType("CheatSheet.Menus.RecipeView").GetField("allRecipeSlot"));
                c.EmitDelegate<Func<object[], object[]>>((orig) =>
                {
                    List<object> slots = orig.ToList();
                    foreach (object slot in slots)
                    {
                        if (((Recipe)slot.GetType().GetField("recipe").GetValue(slot)).HasIngredient(ModContent.ItemType<HallowBunny>()))
                        {
                            slots.Remove(slot);
                        }
                    }
                    return slots.ToArray();
                });
                c.Emit(OpCodes.Stfld, mod.GetType().Assembly.GetType("CheatSheet.Menus.RecipeView").GetField("allRecipeSlot"));
            }

            private static void CheatSheetChanges_ModifyItemViewSet(ILContext il)
            {
                ILCursor c = new(il);
                ILLabel definition = null;
                if (!c.TryGotoNext(i => i.MatchLdfld<Item>(nameof(Item.type)),
                    i => i.MatchBrfalse(out definition)))
                {
                    AltLibrary.Instance.Logger.Info("5 $ 1");
                    return;
                }

                c.Index += 2;
                c.Emit(OpCodes.Ldloc, 2);
                c.Emit(OpCodes.Ldfld, mod.GetType().Assembly.GetType("CheatSheet.Menus.Slot").GetField("item"));
                c.Emit(OpCodes.Ldfld, typeof(Item).GetField(nameof(Item.type)));
                c.EmitDelegate(() => ModContent.ItemType<HallowBunny>());
                c.Emit(OpCodes.Beq_S, definition);
                c.Emit(OpCodes.Ldloc, 2);
                c.Emit(OpCodes.Ldfld, mod.GetType().Assembly.GetType("CheatSheet.Menus.Slot").GetField("item"));
                c.Emit(OpCodes.Ldfld, typeof(Item).GetField(nameof(Item.type)));
                c.EmitDelegate(() => ModContent.ItemType<HallowBunnyCage>());
                c.Emit(OpCodes.Beq_S, definition);
            }

            private static void CheatSheetChanges_ModifyNPCViewSet(ILContext il)
            {
                ILCursor c = new(il);
                ILLabel definition = null;
                if (!c.TryGotoNext(i => i.MatchLdfld(mod.GetType().Assembly.GetType("CheatSheet.Menus.NPCSlot").GetField("npcType")),
                    i => i.MatchBrtrue(out definition)))
                {
                    AltLibrary.Instance.Logger.Info("4 $ 1");
                    return;
                }

                c.Index += 2;
                c.Emit(OpCodes.Ldarg, 0);
                c.Emit(OpCodes.Ldfld, mod.GetType().Assembly.GetType("CheatSheet.Menus.NPCView").GetField("allNPCSlot"));
                c.Emit(OpCodes.Ldloc, 0);
                c.Emit(OpCodes.Ldloc, 1);
                c.Emit(OpCodes.Callvirt, typeof(List<int>).GetMethod("get_Item"));
                c.Emit(OpCodes.Ldelem_Ref);
                c.Emit(OpCodes.Ldfld, mod.GetType().Assembly.GetType("CheatSheet.Menus.NPCSlot").GetField("npcType"));
                c.EmitDelegate(() => ModContent.NPCType<Content.NPCs.HallowBunny>());
                c.Emit(OpCodes.Beq_S, definition);
            }
        }
    }
}
