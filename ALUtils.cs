using AltLibrary.Common.AltBiomes;
using AltLibrary.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace AltLibrary {
	internal static class ALUtils {
		public class SpriteBatchData {
			private delegate SpriteSortMode GetSortModeDelegate(SpriteBatch batch);
			private delegate void SetSortModeDelegate(SpriteBatch batch, SpriteSortMode sortMode);
			private delegate BlendState GetBlendStateDelegate(SpriteBatch batch);
			private delegate void SetBlendStateDelegate(SpriteBatch batch, BlendState blendState);
			private delegate SamplerState GetSamplerStateDelegate(SpriteBatch batch);
			private delegate void SetSamplerStateDelegate(SpriteBatch batch, SamplerState samplerState);
			private delegate DepthStencilState GetDepthStencilStateDelegate(SpriteBatch batch);
			private delegate void SetDepthStencilStateDelegate(SpriteBatch batch, DepthStencilState depthStencilState);
			private delegate RasterizerState GetRasterizerStateDelegate(SpriteBatch batch);
			private delegate void SetRasterizerStateDelegate(SpriteBatch batch, RasterizerState depthStencilState);
			private delegate Matrix GetMatrixDelegate(SpriteBatch batch);
			private delegate void SetMatrixDelegate(SpriteBatch batch, Matrix matrix);
			private delegate Effect GetEffectDelegate(SpriteBatch batch);
			private delegate void SetEffectDelegate(SpriteBatch batch, Effect effect);

			private static readonly GetSortModeDelegate _getSortModeDelegate;
			private static readonly SetSortModeDelegate _setSortModeDelegate;
			private static readonly GetBlendStateDelegate _getBlendStateDelegate;
			private static readonly SetBlendStateDelegate _setBlendStateDelegate;
			private static readonly GetSamplerStateDelegate _getSamplerStateDelegate;
			private static readonly SetSamplerStateDelegate _setSamplerStateDelegate;
			private static readonly GetDepthStencilStateDelegate _getDepthStencilStateDelegate;
			private static readonly SetDepthStencilStateDelegate _setDepthStencilStateDelegate;
			private static readonly GetRasterizerStateDelegate _getRasterizerStateDelegate;
			private static readonly SetRasterizerStateDelegate _setRasterizerStateDelegate;
			private static readonly GetMatrixDelegate _getMatrixDelegate;
			private static readonly SetMatrixDelegate _setMatrixDelegate;
			private static readonly GetEffectDelegate _getEffectDelegate;
			private static readonly SetEffectDelegate _setEffectDelegate;

			private readonly SpriteBatch batch;

			public SpriteSortMode SortMode {
				get => _getSortModeDelegate(batch);
				set => _setSortModeDelegate(batch, value);
			}
			public BlendState BlendState {
				get => _getBlendStateDelegate(batch);
				set => _setBlendStateDelegate(batch, value);
			}
			public SamplerState SamplerState {
				get => _getSamplerStateDelegate(batch);
				set => _setSamplerStateDelegate(batch, value);
			}
			public DepthStencilState DepthStencilState {
				get => _getDepthStencilStateDelegate(batch);
				set => _setDepthStencilStateDelegate(batch, value);
			}
			public RasterizerState RasterizerState {
				get => _getRasterizerStateDelegate(batch);
				set => _setRasterizerStateDelegate(batch, value);
			}
			public Matrix TransformationMatrix {
				get => _getMatrixDelegate(batch);
				set => _setMatrixDelegate(batch, value);
			}
			public Effect Effect {
				get => _getEffectDelegate(batch);
				set => _setEffectDelegate(batch, value);
			}

			public SpriteBatchData(SpriteBatch batch) {
				this.batch = batch;
			}

			static SpriteBatchData() {
				_getSortModeDelegate = CreateGetDelegate<GetSortModeDelegate, SpriteSortMode>("sortMode");
				_setSortModeDelegate = CreateSetDelegate<SetSortModeDelegate, SpriteSortMode>("sortMode");
				_getBlendStateDelegate = CreateGetDelegate<GetBlendStateDelegate, BlendState>("blendState");
				_setBlendStateDelegate = CreateSetDelegate<SetBlendStateDelegate, BlendState>("blendState");
				_getSamplerStateDelegate = CreateGetDelegate<GetSamplerStateDelegate, SamplerState>("samplerState");
				_setSamplerStateDelegate = CreateSetDelegate<SetSamplerStateDelegate, SamplerState>("samplerState");
				_getDepthStencilStateDelegate = CreateGetDelegate<GetDepthStencilStateDelegate, DepthStencilState>("depthStencilState");
				_setDepthStencilStateDelegate = CreateSetDelegate<SetDepthStencilStateDelegate, DepthStencilState>("depthStencilState");
				_getRasterizerStateDelegate = CreateGetDelegate<GetRasterizerStateDelegate, RasterizerState>("rasterizerState");
				_setRasterizerStateDelegate = CreateSetDelegate<SetRasterizerStateDelegate, RasterizerState>("rasterizerState");
				_getMatrixDelegate = CreateGetDelegate<GetMatrixDelegate, Matrix>("transformMatrix");
				_setMatrixDelegate = CreateSetDelegate<SetMatrixDelegate, Matrix>("transformMatrix");
				_getEffectDelegate = CreateGetDelegate<GetEffectDelegate, Effect>("customEffect");
				_setEffectDelegate = CreateSetDelegate<SetEffectDelegate, Effect>("customEffect");

				static T CreateGetDelegate<T, T2>(string fieldName) where T : Delegate {
					ParameterExpression param = Expression.Parameter(typeof(SpriteBatch));
					Expression field = Expression.Field(param, typeof(SpriteBatch), fieldName);

					return Expression.Lambda<T>(field, param).Compile();
				}
				static T CreateSetDelegate<T, T2>(string fieldName) where T : Delegate {
					ParameterExpression param = Expression.Parameter(typeof(SpriteBatch));
					ParameterExpression param2 = Expression.Parameter(typeof(T2));

					Expression field = Expression.Field(param, typeof(SpriteBatch), fieldName);
					Expression ass = Expression.Assign(field, param2);

					return Expression.Lambda<T>(ass, param, param2).Compile();
				}
			}
		}

		public static int AddVariable(this ILContext context, Type type) {
			context.Body.Variables.Add(new(context.Import(type)));
			return context.Body.Variables.Count - 1;
		}

		internal static ulong SteamID() {
			if (AltLibrary._steamId.HasValue)
				return AltLibrary._steamId.Value;

			try {
				var steamId = SteamAPI.IsSteamRunning() && SteamUser.BLoggedOn() ? SteamUser.GetSteamID() : default;
				AltLibrary._steamId = steamId.IsValid() ? steamId.m_SteamID : 0UL;
			}
			catch {
				AltLibrary._steamId = 0ul;
			}
			return AltLibrary._steamId.Value;
		}

		internal static int AdvancedGetSizeOfCategory(string key, out LocalizedText[] texts) {
			int num = 0;
			List<LocalizedText> localizedTexts = new();
			for (int i = 0; i < 420; i++) {
				if (!Language.Exists(key + "." + i.ToString())) {
					break;
				}
				localizedTexts.Add(Language.GetText(key + "." + i.ToString()));
				num++;
			}
			texts = localizedTexts.ToArray();
			return num;
		}

		internal static void ReplaceIDs<T>(ILContext il, T id, Func<T, T> replace, Func<T, bool> check, Func<T, T> replaceCheck = null) where T : IConvertible {
			ILCursor c = new(il);
			switch (id) {
				case true when id is short:
				case true when id is ushort:
				case true when id is int: {
						int a = id.ToInt32(null);
						while (c.TryGotoNext(i => i.MatchLdcI4(a) && i.Offset != 0)) {
							c.Index++;
							c.EmitDelegate<Func<T, T>>(orig => check(orig) ? replace(orig) : (replaceCheck == null ? orig : replaceCheck(orig)));
						}
						break;
					}
				default:
					throw new ArgumentException("Invalid type: " + typeof(T).Name, nameof(id));
			}
		}

		public static bool IsWorldValid(UIWorldListItem self) {
			GetWorldData(self._data, out Dictionary<string, AltLibraryConfig.WorldDataValues> tempDict, out string path2);
			bool valid = true;
			if (!tempDict.ContainsKey(path2)) {
				return valid;
			}

			if (tempDict[path2].worldHallow != "" && !ModContent.TryFind<AltBiome>(tempDict[path2].worldHallow, out _)) {
				valid = false;
			}
			if (tempDict[path2].worldEvil != "" && !ModContent.TryFind<AltBiome>(tempDict[path2].worldEvil, out _)) {
				valid = false;
			}
			if (tempDict[path2].worldHell != "" && !ModContent.TryFind<AltBiome>(tempDict[path2].worldHell, out _)) {
				valid = false;
			}
			if (tempDict[path2].worldJungle != "" && !ModContent.TryFind<AltBiome>(tempDict[path2].worldJungle, out _)) {
				valid = false;
			}
			if (tempDict[path2].drunkEvil != "Terraria/Corruption" && tempDict[path2].drunkEvil != "Terraria/Crimson" && tempDict[path2].drunkEvil != "" && !ModContent.TryFind<AltBiome>(tempDict[path2].drunkEvil, out _)) {
				valid = false;
			}
			return valid;
		}

		public static void GetWorldData(WorldFileData _data, out Dictionary<string, AltLibraryConfig.WorldDataValues> tempDict, out string path2) {
			path2 = Path.ChangeExtension(_data.Path, ".twld");
			tempDict = AltLibraryConfig.Config.GetWorldData();
			if (tempDict.ContainsKey(path2) || !FileUtilities.Exists(path2, _data.IsCloudSave)) {
				return;
			}

			byte[] buf = FileUtilities.ReadAllBytes(path2, _data.IsCloudSave);
			if (buf[0] != 31 || buf[1] != 139) {
				return;
			}
			var stream = new MemoryStream(buf);
			var tag = TagIO.FromStream(stream);
			bool containsMod = false;
			if (!tag.ContainsKey("modData")) {
				return;
			}
			foreach (TagCompound modDataTag in tag.GetList<TagCompound>("modData").Skip(2)) {
				if (modDataTag.Get<string>("mod") != AltLibrary.Instance.Name) {
					continue;
				}
				TagCompound dataTag = modDataTag.Get<TagCompound>("data");
				AltLibraryConfig.WorldDataValues worldData;
				worldData.worldEvil = dataTag.Get<string>("AltLibrary:WorldEvil");
				worldData.worldHallow = dataTag.Get<string>("AltLibrary:WorldHallow");
				worldData.worldHell = dataTag.Get<string>("AltLibrary:WorldHell");
				worldData.worldJungle = dataTag.Get<string>("AltLibrary:WorldJungle");
				worldData.drunkEvil = dataTag.Get<string>("AltLibrary:DrunkEvil");
				tempDict[path2] = worldData;
				containsMod = true;
				break;
			}
			if (!containsMod) {
				AltLibraryConfig.WorldDataValues worldData;
				worldData.worldHallow = "";
				worldData.worldEvil = "";
				worldData.worldHell = "";
				worldData.worldJungle = "";
				worldData.drunkEvil = "";
				tempDict[path2] = worldData;
			}
			AltLibraryConfig.Config.SetWorldData(tempDict);
			AltLibraryConfig.Save(AltLibraryConfig.Config);
		}

		internal static void RemoveUntilInstruction(ILCursor c, Func<Instruction, bool> predicate) {
			while (!predicate.Invoke(c.Next)) {
				c.Remove();
			}
		}

		internal static void DrawBoxedCursorTooltip(SpriteBatch spriteBatch, string text) {
			string[] array = Utils.WordwrapString(text, FontAssets.MouseText.Value, 460,
				10, out int lineAmount);
			lineAmount++;
			float num7 = 0f;
			for (int l = 0; l < lineAmount; l++) {
				float x = FontAssets.MouseText.Value.MeasureString(array[l]).X;
				if (num7 < x) {
					num7 = x;
				}
			}

			if (num7 > 460f) {
				num7 = 460f;
			}

			Vector2 vector = new Vector2(Main.mouseX, Main.mouseY) + new Vector2(16f);
			vector += new Vector2(8f, 2f);
			if (vector.Y > Main.screenHeight - 30 * lineAmount) {
				vector.Y = Main.screenHeight - 30 * lineAmount;
			}

			if (vector.X > Main.screenWidth - num7) {
				vector.X = Main.screenWidth - num7;
			}

			var color = new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor,
				Main.mouseTextColor);
			color = Color.Lerp(color, Color.White, 1f);
			const int num8 = 10;
			const int num9 = 5;
			Utils.DrawInvBG(
				spriteBatch,
				new Rectangle((int)vector.X - num8, (int)vector.Y - num9, (int)num7 + num8 * 2,
					30 * lineAmount + num9 + num9 / 2), new Color(23, 25, 81, 255) * 0.925f * 0.85f);
			for (int m = 0; m < lineAmount; m++) {
				Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, array[m], vector.X,
					vector.Y + m * 30, color, Color.Black, Vector2.Zero);
			}
		}

		internal static Vector2 ToNearestPixel(this Vector2 vector) => new((int)vector.X, (int)vector.Y);

		internal static FieldInfo[] cacheBatch = null;

		public static SpriteBatchData GetData(this SpriteBatch spriteBatch) => new(spriteBatch);

		internal static void GetParameters(this SpriteBatch spriteBatch, out SpriteSortMode sortMode, out BlendState blendState, out SamplerState samplerState, out DepthStencilState depthStencilState, out RasterizerState rasterizerState, out Effect effect, out Matrix transformationMatrix) {
			var data = spriteBatch.GetData();
			sortMode = data.SortMode;
			blendState = data.BlendState;
			samplerState = data.SamplerState;
			depthStencilState = data.DepthStencilState;
			rasterizerState = data.RasterizerState;
			effect = data.Effect;
			transformationMatrix = data.TransformationMatrix;
		}

		internal static bool IsNotEmptyAndNull(this string str) => str != null && str != string.Empty;
	}
}
