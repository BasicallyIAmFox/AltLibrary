using AltLibrary.Common.Solutions;
using AltLibrary.Core.LogicGates;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.ModLoader;

namespace AltLibrary.Common.Conversion;

public sealed class ConversionData {
	public delegate bool SuccessCheckDelegate(int tileType);
	public delegate ConversionRunCodeValues PreConversionDelegate(Tile tile, int i, int j);
	public delegate void OnConversionDelegate(Tile tile, int oldTileType, int i, int j);

	private readonly List<Data> conversions = new();
	private readonly int type;
	private Data _cacheConversion;

	private Data CacheConversion => _cacheConversion ??= new();

	internal ConversionData(ModSolution solution) => type = solution.Type;

	private void CreateSuccessCheck(SuccessCheckDelegate checkDelegate) {
		var del = CacheConversion.SuccessCheckDelegate;
		if (del == null) {
			CacheConversion.SuccessCheckDelegate = checkDelegate;
			return;
		}
		CacheConversion.SuccessCheckDelegate = (int tileType) => del(tileType) | checkDelegate(tileType);
	}

	public ConversionData From<T>() where T : ModBlockType => From(ModContent.GetInstance<T>().Type);
	public ConversionData From(int type) => From((int tileType) => type == tileType);
	public ConversionData From(bool[] boolArray) => From((int tileType) => boolArray[tileType]);
	public ConversionData From(SuccessCheckDelegate checkDelegate) {
		CreateSuccessCheck(checkDelegate);
		return this;
	}

	public ConversionData To<T>() where T : ModBlockType => To(ModContent.GetInstance<T>().Type);
	public ConversionData To(int type) {
		CacheConversion.ConvertsTo = type;
		return this;
	}

	public ConversionData BeforeConversion(PreConversionDelegate preConversionDelegate) {
		var v = CacheConversion.PreConversionDelegate;
		if (v == null) {
			CacheConversion.PreConversionDelegate = preConversionDelegate;
		}
		else {
			CacheConversion.PreConversionDelegate = (Tile tile, int i, int j) => {
				var result1 = v(tile, i, j);
				if (result1 != ConversionRunCodeValues.Run) {
					return result1;
				}
				return preConversionDelegate(tile, i, j);
			};
		}
		return this;
	}

	public ConversionData OnConversion(OnConversionDelegate conversionDelegate) {
		var v = CacheConversion.OnConversionDelegate;
		if (v == null) {
			CacheConversion.OnConversionDelegate = conversionDelegate;
		}
		else {
			CacheConversion.OnConversionDelegate = Delegate.Combine(v, conversionDelegate).As<OnConversionDelegate>();
		}
		return this;
	}

	private void EnsureData() {
		if (_cacheConversion.SuccessCheckDelegate == null && _cacheConversion.PreConversionDelegate == null) {
			throw new ArgumentException($"Unable to register {_cacheConversion.Type} because there no target tile!");
		}
		if (_cacheConversion.ConvertsTo < ConversionHandler.Break) {
			throw new ArgumentOutOfRangeException($"Unable to have target tile that is lesser than {ConversionHandler.Break}!");
		}
		if (_cacheConversion.ConvertsTo >= TileLoader.TileCount) {
			throw new ArgumentOutOfRangeException($"Unable to have target tile that is greater than {TileLoader.TileCount}!");
		}
	}

	public ConversionData RegisterTile() {
		CacheConversion.Type = ConversionType.Tile;
		EnsureData();
		conversions.Add(_cacheConversion);
		_cacheConversion = null;
		return this;
	}

	public ConversionData RegisterWall() {
		CacheConversion.Type = ConversionType.Wall;
		EnsureData();
		conversions.Add(_cacheConversion);
		_cacheConversion = null;
		return this;
	}

	internal void Fill(Data[] data) {
		conversions.ForEach(conversion => {
			if (conversion.Type == ConversionType.Tile) {
				for (int i = 0; i < TileLoader.TileCount; i++) {
					if (i != conversion.ConvertsTo && !(conversion.SuccessCheckDelegate?.Invoke(i) ?? true)) {
						continue;
					}
					data[ConversionHandler.TileIndex(type, i)] = conversion;
				}
			}
			else if (conversion.Type == ConversionType.Wall) {
				for (int i = 0; i < WallLoader.WallCount; i++) {
					if (i != conversion.ConvertsTo && !(conversion.SuccessCheckDelegate?.Invoke(i) ?? true)) {
						continue;
					}
					data[ConversionHandler.WallIndex(type, i)] = conversion;
				}
			}
		});
	}

	internal enum ConversionType : byte {
		Tile,
		Wall,
	}
	internal sealed class Data {
		public ConversionType Type { get; set; }
		public int ConvertsTo { get; set; } = ConversionHandler.Keep;
		public SuccessCheckDelegate SuccessCheckDelegate { get; set; }
		public PreConversionDelegate PreConversionDelegate { get; set; }
		public OnConversionDelegate OnConversionDelegate { get; set; }
	}
}
