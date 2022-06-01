using AltLibrary.Common;
using AltLibrary.Common.Systems;
using AltLibrary.Content.NPCs;
using AltLibrary.Core.UIs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace AltLibrary.Core.States
{
    internal class ALPieChartState : UIState
    {
        private ALUIBorderedPieChart pieChart;
        private bool hasDone;

        public override void OnInitialize()
        {
            pieChart = new();
            pieChart.Width.Set(182, 0f);
            pieChart.Height.Set(60, 0f);
            pieChart.Left.Set(800, 0f);
            pieChart.Top.Set(15, 0f);
            pieChart.OnUpdate += PieChart_OnUpdate;
            Append(pieChart);
            hasDone = false;
        }

        private void PieChart_OnUpdate(UIElement affectedElement)
        {
            (affectedElement as ALUIBorderedPieChart).Hidden = !Main.LocalPlayer.GetModPlayer<ALPlayer>().IsAnalysing;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (WorldBiomeManager.AltBiomeData != null && !hasDone)
            {
                for (int i = 0; i < AltLibrary.Biomes.Count + 4; i++)
                {
                    pieChart.PieChart.RegisterData(WorldBiomeManager.AltBiomeData[i]);
                }
                hasDone = true;
            }

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
