using System;
using System.Collections.Generic;

namespace AltLibrary.Common.Data;

public interface IAltData { }
public interface IDataHandler {
	void Add<T>(T data) where T : struct, IAltData;
	T Get<T>() where T : struct, IAltData;
}

public sealed class DataHandler : IDataHandler {
	private readonly Dictionary<Type, IAltData> database = new();

	public void Add<T>(T data) where T : struct, IAltData {
		database[typeof(T)] = data;
	}

	public T Get<T>() where T : struct, IAltData {
		return database.TryGetValue(typeof(T), out IAltData data) ? (T)data : new T();
	}
}