using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace AltLibrary.Common.Hooks
{
	internal static class NoSecretItems
	{
		internal static void Load()
		{
			//Visuals.Load();
			//HerosDetour.Load();
			//CheatSheetChanges.Load();
		}

		internal static void Unload()
		{
			//Visuals.Unload();
			//HerosDetour.Unload();
			//CheatSheetChanges.Unload();
		}

		[Autoload(false)]
		internal static class Visuals
		{
			/*internal static void Load()
            {
                IL.Terraria.Main.DrawSunAndMoon += ReplaceAllSunAndMoons;
                On.Terraria.Main.DrawSunAndMoon += Main_DrawSunAndMoon1;
                IL.Terraria.Main.UpdateTime_StartDay += Main_UpdateTime_StartDay;
                IL.Terraria.Player.ItemCheck_UseEventItems += Main_UpdateTime_StartDay;
                IL.Terraria.GameContent.Events.MoonlordDeathDrama.MoonlordExplosion.Draw += MoonlordExplosion_Draw;
                IL.Terraria.GameContent.Events.MoonlordDeathDrama.MoonlordPiece.Draw += MoonlordPiece_Draw;
                IL.Terraria.Main.DrawNPCDirect_Inner += Main_DrawNPCDirect;
                IL.Terraria.Main.DrawProj_DrawNormalProjs += Main_DrawProj;
            }

            internal static void Unload()
            {
                IL.Terraria.Main.DrawSunAndMoon -= ReplaceAllSunAndMoons;
                On.Terraria.Main.DrawSunAndMoon -= Main_DrawSunAndMoon1;
                IL.Terraria.Main.UpdateTime_StartDay -= Main_UpdateTime_StartDay;
                IL.Terraria.Player.ItemCheck_UseEventItems -= Main_UpdateTime_StartDay;
                IL.Terraria.GameContent.Events.MoonlordDeathDrama.MoonlordExplosion.Draw -= MoonlordExplosion_Draw;
                IL.Terraria.GameContent.Events.MoonlordDeathDrama.MoonlordPiece.Draw -= MoonlordPiece_Draw;
                IL.Terraria.Main.DrawNPCDirect_Inner -= Main_DrawNPCDirect;
                IL.Terraria.Main.DrawProj_DrawNormalProjs -= Main_DrawProj;
            }

            private static void Main_DrawProj(ILContext il)
            {
                ILCursor c = new(il);
                if (!c.TryGotoNext(i => i.MatchLdcI4(ProjectileID.PhantasmalEye)))
                {
                    AltLibrary.Instance.Logger.Info($"14 $ 1");
                    return;
                }

                if (!c.TryGotoNext(i => i.MatchStloc(11)))
                {
                    AltLibrary.Instance.Logger.Info($"14 $ 2");
                    return;
                }

                c.Index++;
                c.Emit(OpCodes.Ldloc, 11);
                c.Emit(OpCodes.Ldarg, 2);
                c.EmitDelegate<Func<Texture2D, Projectile, Texture2D>>((value, proj) => proj.type switch
                    {
                        ProjectileID.PhantasmalEye when AltLibrary.LordMoonFleshy => ALTextureAssets.LordFleshy[27].Value,
                        ProjectileID.PhantasmalSphere when AltLibrary.LordMoonFleshy => ALTextureAssets.LordFleshy[28].Value,
                        _ => value,
                    });
                c.Emit(OpCodes.Stloc, 11);

                if (!c.TryGotoNext(i => i.MatchLdcI4(ProjectileID.PhantasmalDeathray)))
                {
                    AltLibrary.Instance.Logger.Info($"14 $ 3");
                    return;
                }

                void FindAndReplace(int order, int index, int num)
                {
                    if (!c.TryGotoNext(i => i.MatchStloc(order)))
                    {
                        AltLibrary.Instance.Logger.Info($"14 $ {num}");
                        return;
                    }

                    c.Index++;
                    c.Emit(OpCodes.Ldloc, order);
                    c.EmitDelegate<Func<Texture2D, Texture2D>>((value) => AltLibrary.LordMoonFleshy ? ALTextureAssets.LordFleshy[index].Value : value);
                    c.Emit(OpCodes.Stloc, order);
                }

                FindAndReplace(331, 29, 4);
                FindAndReplace(332, 8, 5);
                FindAndReplace(333, 9, 6);

                if (!c.TryGotoNext(i => i.MatchLdcI4(ProjectileID.MoonLeech)))
                {
                    AltLibrary.Instance.Logger.Info($"14 $ 7");
                    return;
                }

                FindAndReplace(381, 10, 8);
                FindAndReplace(382, 11, 9);

                if (!c.TryGotoNext(i => i.MatchLdarg(2),
                    i => i.MatchCallvirt<Entity>("get_Center"),
                    i => i.MatchLdsfld<Main>(nameof(Main.screenPosition))))
                {
                    AltLibrary.Instance.Logger.Info($"14 $ 10");
                    return;
                }

                c.Index++;
                c.EmitDelegate<Func<Texture2D, Texture2D>>((value) => AltLibrary.LordMoonFleshy ? ALTextureAssets.LordFleshy[30].Value : value);
            }

            private static void Main_DrawNPCDirect(ILContext il)
            {
                ILCursor c = new(il);

                void FindAndReplace(int order, int index, int num)
                {
                    if (!c.TryGotoNext(i => i.MatchStloc(order)))
                    {
                        AltLibrary.Instance.Logger.Info($"13 $ {num}");
                        return;
                    }

                    c.Index++;
                    c.Emit(OpCodes.Ldloc, order);
                    c.EmitDelegate<Func<Texture2D, Texture2D>>((value) => AltLibrary.LordMoonFleshy ? ALTextureAssets.LordFleshy[index].Value : value);
                    c.Emit(OpCodes.Stloc, order);
                }

                // NPCID.MoonLordCore

                FindAndReplace(275, 20, -7);
                FindAndReplace(276, 4, -8);
                FindAndReplace(277, 2, -9);
                FindAndReplace(282, 1, -10);

                // NPCID.MoonLordHand

                FindAndReplace(299, 19, -2);
                FindAndReplace(303, 3, -3);
                FindAndReplace(310, 5, -4);
                FindAndReplace(311, 7, -5);
                FindAndReplace(315, 13, -6);

                // NPCID.MoonLordHead

                FindAndReplace(323, 0, 2);
                FindAndReplace(325, 6, 3);
                FindAndReplace(326, 7, 4);
                FindAndReplace(330, 12, 5);
                FindAndReplace(333, 14, 6);
                FindAndReplace(336, 13, 7);

                // NPCID.MoonLordFreeEye

                FindAndReplace(342, 21, 9);
                FindAndReplace(343, 7, 10);
            }

            private static void MoonlordPiece_Draw(ILContext il)
            {
                ILCursor c = new(il);
                if (!c.TryGotoNext(i => i.MatchLdfld<MoonlordDeathDrama.MoonlordPiece>("_texture")))
                {
                    AltLibrary.Instance.Logger.Info("12 $ 1");
                    return;
                }

                c.Index++;
                c.EmitDelegate<Func<Texture2D, Texture2D>>((value) =>
                {
                    if (AltLibrary.LordMoonFleshy)
                    {
                        if (value.Name.EndsWith("Spine"))
                        {
                            return ALTextureAssets.LordFleshy[25].Value;
                        }
                        else if (value.Name.EndsWith("Shoulder"))
                        {
                            return ALTextureAssets.LordFleshy[24].Value;
                        }
                        else if (value.Name.EndsWith("Torso"))
                        {
                            return ALTextureAssets.LordFleshy[26].Value;
                        }
                        else
                        {
                            return ALTextureAssets.LordFleshy[23].Value;
                        }
                    }
                    else
                    {
                        return value;
                    }
                });
            }

            private static void MoonlordExplosion_Draw(ILContext il)
            {
                ILCursor c = new(il);
                if (!c.TryGotoNext(i => i.MatchLdfld<MoonlordDeathDrama.MoonlordExplosion>("_texture")))
                {
                    AltLibrary.Instance.Logger.Info("11 $ 1");
                    return;
                }

                c.Index++;
                c.EmitDelegate<Func<Texture2D, Texture2D>>((value) => AltLibrary.LordMoonFleshy ? ALTextureAssets.LordFleshy[22].Value : value);
            }

            private static void Main_UpdateTime_StartDay(ILContext il)
            {
                ILCursor c = new(il);
                while (c.TryGotoNext(i => i.MatchLdsfld<Lang>(nameof(Lang.misc)),
                    i => i.MatchLdcI4(20),
                    i => i.MatchLdelemRef()))
                {
                    c.Index += 3;
                    c.EmitDelegate<Func<LocalizedText, LocalizedText>>((value) => AltLibrary.MoonSunViceVersa ? Language.GetText("Mods.AltLibrary.MoonEclipse") : value);
                }
            }

            private static void Main_DrawSunAndMoon1(On.Terraria.Main.orig_DrawSunAndMoon orig, Main self, Main.SceneArea sceneArea, Color moonColor, Color sunColor, float tempMushroomInfluence)
            {
                if (AltLibrary.MoonSunWiseTimer < 2510)
                    AltLibrary.MoonSunWiseTimer++;
                if (Main.alreadyGrabbingSunOrMoon && AltLibrary.MoonSunWiseTimer == 2509)
                {
                    AltLibrary.MoonSunViceVersa = true;
                    SoundEngine.PlaySound(SoundID.AbigailSummon);
                    AltLibrary.MoonSunWiseTimer = 2510;
                }
                orig(self, sceneArea, moonColor, sunColor, tempMushroomInfluence);
            }

            private static void ReplaceAllSunAndMoons(ILContext il)
            {
                ILCursor c = new(il);
                while (c.TryGotoNext(i => i.MatchLdsfld(typeof(TextureAssets).GetField(nameof(TextureAssets.Sun)))))
                {
                    c.Index++;
                    c.EmitDelegate<Func<Asset<Texture2D>, Asset<Texture2D>>>((value) => AltLibrary.MoonSunViceVersa ? ALTextureAssets.MoonSun[0] : value);
                }
                c = new(il);
                while (c.TryGotoNext(i => i.MatchLdsfld(typeof(TextureAssets).GetField(nameof(TextureAssets.Sun3)))))
                {
                    c.Index++;
                    c.EmitDelegate<Func<Asset<Texture2D>, Asset<Texture2D>>>((value) => AltLibrary.MoonSunViceVersa ? ALTextureAssets.MoonSun[2] : value);
                }
                c = new(il);
                while (c.TryGotoNext(i => i.MatchLdsfld(typeof(TextureAssets).GetField(nameof(TextureAssets.Sun2)))))
                {
                    c.Index++;
                    c.EmitDelegate<Func<Asset<Texture2D>, Asset<Texture2D>>>((value) => AltLibrary.MoonSunViceVersa ? ALTextureAssets.MoonSun[1] : value);
                }
                c = new(il);
                while (c.TryGotoNext(i => i.MatchLdsfld(typeof(TextureAssets).GetField(nameof(TextureAssets.SmileyMoon)))))
                {
                    c.Index++;
                    c.EmitDelegate<Func<Asset<Texture2D>, Asset<Texture2D>>>((value) => AltLibrary.MoonSunViceVersa ? ALTextureAssets.MoonSun[6] : value);
                }
                c = new(il);
                while (c.TryGotoNext(i => i.MatchLdsfld(typeof(TextureAssets).GetField(nameof(TextureAssets.PumpkinMoon)))))
                {
                    c.Index++;
                    c.EmitDelegate<Func<Asset<Texture2D>, Asset<Texture2D>>>((value) => AltLibrary.MoonSunViceVersa ? ALTextureAssets.MoonSun[5] : value);
                }
                c = new(il);
                while (c.TryGotoNext(i => i.MatchLdsfld(typeof(TextureAssets).GetField(nameof(TextureAssets.SnowMoon)))))
                {
                    c.Index++;
                    c.EmitDelegate<Func<Asset<Texture2D>, Asset<Texture2D>>>((value) => AltLibrary.MoonSunViceVersa ? ALTextureAssets.MoonSun[4] : value);
                }
                c = new(il);
                while (c.TryGotoNext(i => i.MatchLdsfld(typeof(TextureAssets).GetField(nameof(TextureAssets.Moon)))) && c.TryGotoNext(i => i.MatchLdelemRef()))
                {
                    c.Index++;
                    c.EmitDelegate<Func<Asset<Texture2D>, Asset<Texture2D>>>((value) => AltLibrary.MoonSunViceVersa ? ALTextureAssets.MoonSun[3] : value);
                }
            }
            */
		}

		[JITWhenModsEnabled("HEROsMod")]
		[ExtendsFromMod]
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
				items.RemoveAll(x => AltLibrary.ItemsToNowShowUp.Contains(x.type));
				return items.ToArray();
			}
		}

		[JITWhenModsEnabled("CheatSheet")]
		[ExtendsFromMod]
		internal static class CheatSheetChanges
		{
			private static Mod mod;

			private static MethodInfo ItemViewSet = null;
			private static event ILContext.Manipulator ModifyItemViewSet
			{
				add => HookEndpointManager.Modify(ItemViewSet, value);
				remove => HookEndpointManager.Unmodify(ItemViewSet, value);
			}
			private static MethodInfo RecipeViewSet = null;
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

				Type p = mod.GetType().Assembly.GetType("CheatSheet.Menus.ItemView");
				if (p != null)
				{
					ItemViewSet = p.GetMethod("set_selectedCategory", BindingFlags.Public | BindingFlags.Instance);
				}
				if (ItemViewSet != null)
				{
					ModifyItemViewSet += CheatSheetChanges_ModifyItemViewSet;
				}

				p = mod.GetType().Assembly.GetType("CheatSheet.Menus.RecipeBrowserWindow");
				if (p != null)
				{
					RecipeViewSet = p.GetMethod("Draw", BindingFlags.Public | BindingFlags.Instance);
				}
				if (RecipeViewSet != null)
				{
					ModifyRecipeViewSet += CheatSheetChanges_ModifyRecipeViewSet;
				}
			}

			internal static void Unload()
			{
				if (ItemViewSet != null)
				{
					ModifyItemViewSet -= CheatSheetChanges_ModifyItemViewSet;
				}
				if (RecipeViewSet != null)
				{
					ModifyRecipeViewSet -= CheatSheetChanges_ModifyRecipeViewSet;
				}
				ItemViewSet = null;
				RecipeViewSet = null;
				mod = null;
			}

			private static void CheatSheetChanges_ModifyRecipeViewSet(ILContext il)
			{
				ILCursor c = new(il);
				if (!c.TryGotoNext(i => i.MatchLdsfld<Recipe>(nameof(Recipe.numRecipes))))
					return;

				c.Index++;
				c.EmitDelegate<Func<int, int>>((orig) => orig - 1);

				if (!c.TryGotoNext(i => i.MatchLdarg(0),
					i => i.MatchCall(mod.GetType().Assembly.GetType("CheatSheet.Menus.RecipeBrowserWindow").GetMethod("InitializeRecipeCategories", BindingFlags.NonPublic | BindingFlags.Instance))))
				{
					AltLibrary.Instance.Logger.Info("6 $ 1");
					return;
				}

				c.Emit(OpCodes.Ldsfld, mod.GetType().Assembly.GetType("CheatSheet.Menus.RecipeBrowserWindow").GetField("recipeView", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static));
				c.Emit(OpCodes.Ldsfld, mod.GetType().Assembly.GetType("CheatSheet.Menus.RecipeBrowserWindow").GetField("recipeView", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static));
				c.Emit(OpCodes.Ldfld, mod.GetType().Assembly.GetType("CheatSheet.Menus.RecipeView").GetField("allRecipeSlot"));
				c.EmitDelegate<Func<object[], object[]>>((orig) =>
				{
					List<object> slots = orig.ToList();
					int index = slots.FindIndex(x =>
					{
						Recipe r = (Recipe)x.GetType().GetField("recipe").GetValue(x);
						return r.RecipeIndex == AltLibrary.HallowBunnyCageRecipeIndex;
					});
					if (index != -1)
					{
						slots.RemoveAt(index);
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
				c.EmitDelegate<Func<Item, bool>>((item) => AltLibrary.ItemsToNowShowUp.Contains(item.type));
				c.Emit(OpCodes.Brtrue_S, definition);
			}
		}
	}
}
