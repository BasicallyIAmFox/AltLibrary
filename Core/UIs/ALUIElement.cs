﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;

namespace AltLibrary.Core.UIs
{
	internal abstract class ALUIElement : UIElement
	{
		public readonly Queue<UIElement> ElementsForRemoval = new();

		private bool mouseWasOver;

		public delegate void ExxoUIElementEventHandler(ALUIElement sender, EventArgs e);

		public event MouseEvent OnFirstMouseOver;
		public event MouseEvent OnLastMouseOut;

		public event MouseEvent OnMouseHovering;
		public event ExxoUIElementEventHandler OnRecalculateFinish;
		public bool IsVisible => Active && !Hidden && GetOuterDimensions().Width > 0 && GetOuterDimensions().Height > 0;
		public int ElementCount => Elements.Count;

		public abstract bool IsDynamicallySized
		{
			get;
		}

		public bool Active { get; set; } = true;
		public bool Hidden { get; set; }
		public bool IsRecalculating { get; private set; }
		public string Tooltip { get; set; } = "";

		public static void BeginDefaultSpriteBatch(SpriteBatch spriteBatch) =>
			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, DepthStencilState.None, null, null,
				Main.UIScaleMatrix);

		public sealed override void Update(GameTime gameTime)
		{
			if (!Active)
			{
				return;
			}

			if (IsMouseHovering)
			{
				OnMouseHovering?.Invoke(new UIMouseEvent(this, UserInterface.ActiveInstance.MousePosition), this);
			}

			UpdateSelf(gameTime);
			base.Update(gameTime);
			while (ElementsForRemoval.Count > 0)
			{
				RemoveChild(ElementsForRemoval.Dequeue());
			}
		}

		public override bool ContainsPoint(Vector2 point) => IsVisible && base.ContainsPoint(point);

		public sealed override void Draw(SpriteBatch spriteBatch)
		{
			if (IsVisible)
			{
				base.Draw(spriteBatch);

				if (IsMouseHovering && !string.IsNullOrEmpty(Tooltip))
				{
					ALUtils.DrawBoxedCursorTooltip(spriteBatch, Tooltip);
				}
			}
		}

		public sealed override void Recalculate()
		{
			IsRecalculating = true;
			PreRecalculate();
			RecalculateSelf();
			RecalculateChildren();
			PostRecalculate();
			if (Parent is not ALUIElement)
			{
				RecalculateFinish();
			}

			IsRecalculating = false;
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			if (!mouseWasOver)
			{
				mouseWasOver = true;
				FirstMouseOver(evt);
			}
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			if (!ContainsPoint(evt.MousePosition))
			{
				mouseWasOver = false;
				LastMouseOut(evt);
			}
		}

		// Optimised method that only moves positions, only to be used if the elements have already previously been recalculated
		public void RecalculateChildrenSelf()
		{
			RecalculateSelf();
			foreach (UIElement element in Elements)
			{
				if (element is ALUIElement exxoElement)
				{
					exxoElement.RecalculateChildrenSelf();
				}
				else
				{
					element.Recalculate();
				}
			}
		}

		// This works because the UIChanges.ILUIElementRecalculate hook doesn't recalulate children if the element is an ExxoUIElement
		public void RecalculateSelf() => base.Recalculate();

		protected virtual void UpdateSelf(GameTime gameTime)
		{
		}

		protected virtual void PreRecalculate()
		{
		}

		protected virtual void PostRecalculate()
		{
		}

		protected virtual void FirstMouseOver(UIMouseEvent evt) => OnFirstMouseOver?.Invoke(evt, this);

		protected virtual void LastMouseOut(UIMouseEvent evt) => OnLastMouseOut?.Invoke(evt, this);

		private void RecalculateFinish()
		{
			foreach (UIElement element in Elements)
			{
				if (element is ALUIElement exxoElement)
				{
					exxoElement.RecalculateFinish();
				}
			}

			OnRecalculateFinish?.Invoke(this, EventArgs.Empty);
		}
	}
}