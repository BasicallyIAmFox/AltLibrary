using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using AltLibrary.Core.UIs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.UI;
using static AltLibrary.Core.UIs.ALUIPieChart;

namespace AltLibrary.Core.States
{
	internal class ALPieChartState : UIState
	{
		private ALUIBorderedPieChart pieChart;

		public override void OnInitialize()
		{
			List<PieData> pieDatas = new();
			pieDatas.Clear();
			WorldBiomeManager.AltBiomePercentages = new float[AltLibrary.Biomes.Count + 5];
			pieDatas.Add(new PieData("Purity", Color.LawnGreen, () => WorldBiomeManager.AltBiomePercentages[0]));
			pieDatas.Add(new PieData("Corruption", Color.MediumPurple, () => WorldBiomeManager.AltBiomePercentages[1]));
			pieDatas.Add(new PieData("Crimson", Color.IndianRed, () => WorldBiomeManager.AltBiomePercentages[2]));
			pieDatas.Add(new PieData("Hallow", Color.HotPink, () => WorldBiomeManager.AltBiomePercentages[3]));
			for (int i = 0; i < AltLibrary.Biomes.Count; i++)
			{
				AltBiome biome = AltLibrary.Biomes[i];
				if (biome.BiomeType == BiomeType.Evil || biome.BiomeType == BiomeType.Hallow)
				{
					pieDatas.Add(new PieData(biome.DisplayName.GetTranslation(Language.ActiveCulture), biome.NameColor, () => WorldBiomeManager.AltBiomePercentages[i + 4]));
				}
			}
			WorldBiomeManager.AltBiomeData = pieDatas.ToArray();

			pieChart = new();
			pieChart.Width.Set(200, 0f);
			pieChart.Height.Set(200, 0f);
			pieChart.Left.Set(0, 0.425f);
			pieChart.Top.Set(0, 0.25f);
			pieChart.BorderColor = Color.Black;
			foreach (PieData data in WorldBiomeManager.AltBiomeData)
			{
				pieChart.PieChart.RegisterData(data);
			}
			Append(pieChart);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}
	}
}
