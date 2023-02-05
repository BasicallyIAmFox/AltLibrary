using System;
using System.Collections.Generic;

namespace AltLibrary.Common.Data;

public interface IDataHandler {
	void Add<T>(T data) where T : IAltData;
	V Get<V>() where V : IAltData;
}

public abstract class DataHandler : IDataHandler {
	private readonly Dictionary<Type, IAltData> database = new();

	public void Add<T>(T data) where T : IAltData {
		database[typeof(T)] = data;
	}

	public T Get<T>() where T : IAltData {
		return database.TryGetValue(typeof(T), out IAltData data) ? (T)data : default;
	}
}

public interface IAltData { }
public interface IOreData : IAltData { }
public interface IBiomeData : IAltData { }
public sealed class OreDataHandler : DataHandler {
}
public sealed class BiomeDataHandler : DataHandler {
}
