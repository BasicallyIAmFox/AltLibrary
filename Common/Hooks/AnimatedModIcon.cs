using Microsoft.Xna.Framework;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using System;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace AltLibrary.Common.Hooks
{
	internal class AnimatedModIcon
	{
		private static MethodInfo OnInit = null;

		private static event ILContext.Manipulator ModifyOnInit
		{
			add
			{
				HookEndpointManager.Modify(OnInit, value);
			}
			remove
			{
				HookEndpointManager.Unmodify(OnInit, value);
			}
		}

		internal static void Init()
		{
			var UIMods = typeof(Main).Assembly.GetType("Terraria.ModLoader.UI.UIModItem");
			OnInit = UIMods.GetMethod("OnInitialize", BindingFlags.Public | BindingFlags.Instance);
			ModifyOnInit += AnimatedModIcon_ModifyOnInit;
			NoSecretItems.Load();
		}

		internal static void Unload()
		{
			var UIMods = typeof(Main).Assembly.GetType("Terraria.ModLoader.UI.UIModItem");
			OnInit = UIMods.GetMethod("OnInitialize", BindingFlags.Public | BindingFlags.Instance);
			ModifyOnInit -= AnimatedModIcon_ModifyOnInit;
			OnInit = null;
			NoSecretItems.Unload();
		}

		private static void AnimatedModIcon_ModifyOnInit(ILContext il)
		{
			ILCursor c = new(il);
			FieldReference _mod = null;
			FieldReference _modIcon = null;
			if (!c.TryGotoNext(i => i.MatchLdfld(out _mod)))
			{
				AltLibrary.Instance.Logger.Info("a $ 1");
				return;
			}
			if (!c.TryGotoNext(i => i.MatchLdstr(".png")))
			{
				AltLibrary.Instance.Logger.Info("a $ 2");
				return;
			}
			if (!c.TryGotoNext(i => i.MatchStfld(out _modIcon)))
			{
				AltLibrary.Instance.Logger.Info("a $ 3");
				return;
			}
			if (!c.TryGotoNext(i => i.MatchLdstr("Unknown error")))
			{
				AltLibrary.Instance.Logger.Info("a $ 4");
				return;
			}
			if (!c.TryGotoNext(i => i.MatchLdarg(0)))
			{
				AltLibrary.Instance.Logger.Info("a $ 5");
				return;
			}

			var label = il.DefineLabel();

			c.Index++;
			c.Emit(OpCodes.Ldfld, _mod);
			c.Emit(OpCodes.Callvirt, typeof(Main).Assembly.GetType("Terraria.ModLoader.Core.LocalMod").GetMethod("get_Name"));
			c.Emit(OpCodes.Ldstr, "AltLibrary");
			c.Emit(OpCodes.Call, typeof(string).GetMethod("op_Equality", new Type[] { typeof(string), typeof(string) }));
			c.Emit(OpCodes.Brfalse_S, label);

			c.Emit(OpCodes.Ldarg, 0);
			c.Emit(OpCodes.Ldfld, _modIcon);
			c.EmitDelegate(() =>
			{
				UIImageFramed image = new(ALTextureAssets.AnimatedModIcon[0], new Rectangle(0, 0, 80, 80))
				{
					Left =
					{
						Percent = 0f
					},
					Top =
					{
						Percent = 0f
					}
				};
				image.OnUpdate += Image_OnUpdate;
				return image;
			});
			c.Emit(OpCodes.Callvirt, typeof(UIElement).GetMethod(nameof(UIElement.Append), new Type[] { typeof(UIElement) }));

			c.MarkLabel(label);
			c.Emit(OpCodes.Ldarg, 0);

			if (!c.TryGotoNext(i => i.MatchLdarg(0),
				i => i.MatchLdcI4(1),
				i => i.MatchStfld(out _),
				i => i.MatchLdcI4(6),
				i => i.MatchNewarr(out _),
				i => i.MatchDup(),
				i => i.MatchLdcI4(0),
				i => i.MatchLdloc(1),
				i => i.MatchCallvirt(out _),
				i => i.MatchCall(out _),
				i => i.MatchStelemI4(),
				i => i.MatchDup(),
				i => i.MatchLdcI4(1)))
			{
				AltLibrary.Instance.Logger.Info("a $ 6");
				return;
			}

			if (!c.TryGotoNext(i => i.MatchLdloc(1),
				i => i.MatchCallvirt(out _),
				i => i.MatchCall(out _),
				i => i.MatchStelemI4(),
				i => i.MatchDup(),
				i => i.MatchLdcI4(1)))
			{
				AltLibrary.Instance.Logger.Info("a $ 7");
				return;
			}

			c.Index += 3;
			c.Emit(OpCodes.Ldloc, 1);
			c.EmitDelegate<Func<int, Mod, int>>((itemCount, mod) =>
			{
				if (mod.Name == AltLibrary.Instance.Name)
				{
					return itemCount - AltLibrary.ItemsToNowShowUp.Count;
				}
				return itemCount;
			});

			if (!c.TryGotoNext(i => i.MatchLdloc(1),
				i => i.MatchCallvirt(out _),
				i => i.MatchCall(out _),
				i => i.MatchStelemI4(),
				i => i.MatchDup(),
				i => i.MatchLdcI4(2)))
			{
				AltLibrary.Instance.Logger.Info("a $ 8");
				return;
			}

			c.Index += 3;
			c.Emit(OpCodes.Ldloc, 1);
			c.EmitDelegate<Func<int, Mod, int>>((npcCount, mod) =>
			{
				if (mod.Name == AltLibrary.Instance.Name)
				{
					return npcCount - AltLibrary.NPCsToNowShowUp.Count;
				}
				return npcCount;
			});

			if (!c.TryGotoNext(i => i.MatchLdloc(1),
				i => i.MatchCallvirt(out _),
				i => i.MatchCall(out _),
				i => i.MatchStelemI4(),
				i => i.MatchDup(),
				i => i.MatchLdcI4(3)))
			{
				AltLibrary.Instance.Logger.Info("a $ 9");
				return;
			}

			c.Index += 3;
			c.Emit(OpCodes.Ldloc, 1);
			c.EmitDelegate<Func<int, Mod, int>>((tileCount, mod) =>
			{
				if (mod.Name == AltLibrary.Instance.Name)
				{
					return tileCount - AltLibrary.TilesToNowShowUp.Count;
				}
				return tileCount;
			});
		}

		private static void Image_OnUpdate(UIElement affectedElement)
		{
			UIImageFramed e = affectedElement as UIImageFramed;
			int time = 3600;
			int additionX = 0;
			if (AltLibrary.ModIconVariation == 1)
			{
				additionX = ((int)(Main.GlobalTimeWrappedHourly % 2)) * 160;
			}

			e.SetImage(ALTextureAssets.AnimatedModIcon[AltLibrary.ModIconVariation], new Rectangle(additionX, 0, 80, 80));

			if (AltLibraryServerConfig.Config.SecretFeatures && (AltLibrary.TimeHoveringOnIcon >= time + 1 || AltLibrary.HallowBunnyUnlocked))
			{
				e.SetFrame(new Rectangle(80 + additionX, 0, 80, 80));
				AltLibrary.HallowBunnyUnlocked = true;
				return;
			}

			if (e.IsMouseHovering)
			{
				float i = Main.GlobalTimeWrappedHourly % 20;
				if (i >= 0 && i < 10 || i >= 15 && i < 20)
				{
					e.SetFrame(new Rectangle(additionX, 80, 80, 80));
				}
				else
				{
					e.SetFrame(new Rectangle(additionX, 160, 80, 80));
				}
				if (AltLibraryServerConfig.Config.SecretFeatures)
				{
					if (++AltLibrary.TimeHoveringOnIcon == time)
					{
						SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen);
						e.SetFrame(new Rectangle(80 + additionX, 0, 80, 80));
						AltLibrary.TimeHoveringOnIcon = time + 1;
					}
					if (AltLibrary.TimeHoveringOnIcon >= time || AltLibrary.HallowBunnyUnlocked)
					{
						AltLibrary.HallowBunnyUnlocked = true;
					}
				}
				return;
			}

			AltLibrary.TimeHoveringOnIcon = 0;
			float index = Main.GlobalTimeWrappedHourly % 60;
			if (index >= 10 && index < 15)
			{
				e.SetFrame(new Rectangle(80 + additionX, 80, 80, 80));
			}
			else if (index >= 5 && index < 10 || index >= 15 && index < 30)
			{
				e.SetFrame(new Rectangle(additionX, 0, 80, 80));
			}
			else if (index >= 30 && index < 40 || index >= 45 && index < 60)
			{
				e.SetFrame(new Rectangle(additionX, 80, 80, 80));
			}
			else if (index >= 40 && index < 45)
			{
				e.SetFrame(new Rectangle(additionX, 160, 80, 80));
			}
		}
	}
}
