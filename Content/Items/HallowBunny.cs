using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace AltLibrary.Content.Items
{
	internal class HallowBunny : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => AltLibraryServerConfig.Config.SecretFeatures;
		public override void Load() => AltLibrary.ItemsToNowShowUp.Add(Type);
		public override string Texture => "AltLibrary/Assets/HallowBunnyItem";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hallow Bunny");
			Tooltip.SetDefault("An illusive bunny... Yet so deadly.");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Bunny);
			Item.rare = ModContent.RarityType<CustomRarity>();
			Item.makeNPC = (short)ModContent.NPCType<NPCs.HallowBunny>();
		}

		public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
		{
			if (line.Name == "ItemName" && line.Mod == "Terraria")
			{
				ChatManager.DrawColorCodedString(Main.spriteBatch, FontAssets.MouseText.Value, line.Text, new Vector2(line.X, line.Y) + Utils.RotatedBy(new Vector2(4f, 0f), Main.GlobalTimeWrappedHourly), new Color(200, 255, 248), line.Rotation, line.Origin, line.BaseScale);
				ChatManager.DrawColorCodedString(Main.spriteBatch, FontAssets.MouseText.Value, line.Text, new Vector2(line.X, line.Y) + Utils.RotatedBy(new Vector2(4f, 0f), Main.GlobalTimeWrappedHourly + MathHelper.Log2E), new Color(255, 175, 245), line.Rotation, line.Origin, line.BaseScale);
				ChatManager.DrawColorCodedString(Main.spriteBatch, FontAssets.MouseText.Value, line.Text, new Vector2(line.X, line.Y) + Utils.RotatedBy(new Vector2(4f, 0f), Main.GlobalTimeWrappedHourly + MathHelper.E), new Color(255, 249, 59), line.Rotation, line.Origin, line.BaseScale);
				ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, line.Text, new Vector2(line.X, line.Y), line.OverrideColor ?? line.Color, line.Rotation, line.Origin, line.BaseScale);
				return false;
			}
			return base.PreDrawTooltipLine(line, ref yOffset);
		}

		internal class CustomRarity : ModRarity
		{
			public override bool IsLoadingEnabled(Mod mod) => AltLibraryServerConfig.Config.SecretFeatures;
			public override Color RarityColor
			{
				get
				{
					Color[] itemNameCycleColors = new Color[]{
						new Color(200, 255, 248),
						new Color(255, 175, 245),
						new Color(255, 249, 59)
					};
					float fade = Main.GameUpdateCount % 60 / 60f;
					int index = (int)(Main.GameUpdateCount / 60 % itemNameCycleColors.Length);
					return Color.Lerp(itemNameCycleColors[index], itemNameCycleColors[(index + 1) % itemNameCycleColors.Length], fade);
				}
			}
		}
	}
}
