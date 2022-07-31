using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace AltLibrary.Content.Tiles
{
	internal class HallowBunnyCage : ModTile
	{
		public override bool IsLoadingEnabled(Mod mod) => AltLibraryServerConfig.Config.SecretFeatures;
		public override void Load() => AltLibrary.TilesToNowShowUp.Add(Type);
		public override string Texture => "AltLibrary/Assets/HallowBunnyCage";

		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 18 };
			TileObjectData.addTile(Type);
			AnimationFrameHeight = 54;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hallow Bunny Cage");
			AddMapEntry(new Color(185, 23, 115), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 32, ModContent.ItemType<Items.HallowBunnyCage>());
		}

		public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
		{
			Tile tile = Main.tile[i, j];
			Main.critterCage = true;
			int left = i - tile.TileFrameX / 18;
			int top = j - tile.TileFrameY / 18;
			int offset = left / 3 * (top / 3);
			offset %= Main.cageFrames;
			frameYOffset = Main.bunnyCageFrame[offset] * AnimationFrameHeight;
		}
	}
}
