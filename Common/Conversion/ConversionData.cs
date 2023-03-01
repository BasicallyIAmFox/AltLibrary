using AltLibrary.Common.Solutions;
using AltLibrary.Core.LogicGates;
using System;
using System.Collections.Generic;
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

	private void CreateSuccessCheck<T>(SuccessCheckDelegate checkDelegate) where T : ILogicGate {
		var del = CacheConversion.SuccessCheckDelegate;
		if (del == null) {
			CacheConversion.SuccessCheckDelegate = checkDelegate;
			return;
		}
		CacheConversion.SuccessCheckDelegate = (int tileType) => T.Perform(del(tileType), checkDelegate(tileType));
	}

	public ConversionData From<T>() where T : ModBlockType => OrFrom(ModContent.GetInstance<T>().Type);
	public ConversionData From(int type) => OrFrom((int tileType) => type == tileType);
	public ConversionData From(bool[] boolArray) => OrFrom((int tileType) => boolArray[tileType]);
	public ConversionData From(SuccessCheckDelegate checkDelegate) => OrFrom(checkDelegate);

	#region 'Logic Gate' 'From' methods
	public ConversionData AndFrom<T>() where T : ModBlockType => AndFrom(ModContent.GetInstance<T>().Type);
	public ConversionData AndFrom(int type) => AndFrom((int tileType) => type == tileType);
	public ConversionData AndFrom(bool[] boolArray) => AndFrom((int tileType) => boolArray[tileType]);
	public ConversionData AndFrom(SuccessCheckDelegate checkDelegate) {
		CreateSuccessCheck<AndLogicGate>(checkDelegate);
		return this;
	}

	public ConversionData OrFrom<T>() where T : ModBlockType => OrFrom(ModContent.GetInstance<T>().Type);
	public ConversionData OrFrom(int type) => OrFrom((int tileType) => type == tileType);
	public ConversionData OrFrom(bool[] boolArray) => OrFrom((int tileType) => boolArray[tileType]);
	public ConversionData OrFrom(SuccessCheckDelegate checkDelegate) {
		CreateSuccessCheck<OrLogicGate>(checkDelegate);
		return this;
	}

	public ConversionData NandFrom<T>() where T : ModBlockType => NandFrom(ModContent.GetInstance<T>().Type);
	public ConversionData NandFrom(int type) => NandFrom((int tileType) => type == tileType);
	public ConversionData NandFrom(bool[] boolArray) => NandFrom((int tileType) => boolArray[tileType]);
	public ConversionData NandFrom(SuccessCheckDelegate checkDelegate) {
		CreateSuccessCheck<NandLogicGate>(checkDelegate);
		return this;
	}

	public ConversionData NorFrom<T>() where T : ModBlockType => NorFrom(ModContent.GetInstance<T>().Type);
	public ConversionData NorFrom(int type) => NorFrom((int tileType) => type == tileType);
	public ConversionData NorFrom(bool[] boolArray) => NorFrom((int tileType) => boolArray[tileType]);
	public ConversionData NorFrom(SuccessCheckDelegate checkDelegate) {
		CreateSuccessCheck<NorLogicGate>(checkDelegate);
		return this;
	}

	public ConversionData XorFrom<T>() where T : ModBlockType => XorFrom(ModContent.GetInstance<T>().Type);
	public ConversionData XorFrom(int type) => XorFrom((int tileType) => type == tileType);
	public ConversionData XorFrom(bool[] boolArray) => XorFrom((int tileType) => boolArray[tileType]);
	public ConversionData XorFrom(SuccessCheckDelegate checkDelegate) {
		CreateSuccessCheck<XorLogicGate>(checkDelegate);
		return this;
	}

	public ConversionData XnorFrom<T>() where T : ModBlockType => XnorFrom(ModContent.GetInstance<T>().Type);
	public ConversionData XnorFrom(int type) => XnorFrom((int tileType) => type == tileType);
	public ConversionData XnorFrom(bool[] boolArray) => XnorFrom((int tileType) => boolArray[tileType]);
	public ConversionData XnorFrom(SuccessCheckDelegate checkDelegate) {
		CreateSuccessCheck<OrLogicGate>(checkDelegate);
		return this;
	}
	#endregion

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
			throw new ArgumentOutOfRangeException($"Unable to have target tile that is less than {ConversionHandler.Break}!");
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
					if (!conversion.SuccessCheckDelegate(i)) {
						continue;
					}
					data[ConversionHandler.TileIndex(type, i)] = conversion;
				}
			}
			else if (conversion.Type == ConversionType.Wall) {
				for (int i = 0; i < WallLoader.WallCount; i++) {
					if (!conversion.SuccessCheckDelegate(i)) {
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
