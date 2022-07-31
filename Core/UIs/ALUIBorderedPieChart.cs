using Microsoft.Xna.Framework;
using Terraria.UI;

namespace AltLibrary.Core.UIs
{
	internal class ALUIBorderedPieChart : ALUIElement
	{
		private readonly ALUICircle backingCircle;

		private int borderWidth = 5;

		public ALUIBorderedPieChart()
		{
			backingCircle = new ALUICircle();
			backingCircle.Width.Set(0, 1);
			backingCircle.Height.Set(0, 1);
			Append(backingCircle);
			PieChart = new ALUIPieChart();
			PieChart.Width.Set(-(BorderWidth * 2), 1);
			PieChart.Height.Set(-(BorderWidth * 2), 1);
			PieChart.VAlign = UIAlign.Center;
			PieChart.HAlign = UIAlign.Center;
			backingCircle.Append(PieChart);
		}

		public override bool IsDynamicallySized => false;
		public ALUIPieChart PieChart { get; }

		public Color BorderColor
		{
			get => backingCircle.Color;
			set => backingCircle.Color = value;
		}

		public int BorderWidth
		{
			get => borderWidth;
			set
			{
				borderWidth = value;
				PieChart.Width.Set(-(BorderWidth * 2), 1);
				PieChart.Height.Set(-(BorderWidth * 2), 1);
			}
		}

		public override bool ContainsPoint(Vector2 point) => backingCircle.ContainsPoint(point);
	}
}