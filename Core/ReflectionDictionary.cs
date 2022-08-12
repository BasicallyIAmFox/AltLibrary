using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace AltLibrary.Core
{
	public static class ReflectionDictionary
	{
		private static Dictionary<string, Type> Types = new();

		private static Dictionary<string, ReflectionAsset<ConstructorInfo>> Constructors = new();
		private static Dictionary<string, ReflectionAsset<EventInfo>> Events = new();
		private static Dictionary<string, ReflectionAsset<FieldInfo>> Fields = new();
		private static Dictionary<string, ReflectionAsset<MethodInfo>> Methods = new();
		private static Dictionary<string, ReflectionAsset<PropertyInfo>> Properties = new();

		private static readonly BindingFlags[] possibleFlags = new BindingFlags[]
				{
					BindingFlags.NonPublic | BindingFlags.Instance,
					BindingFlags.NonPublic | BindingFlags.Static,

					BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static,
					BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,

					BindingFlags.Public | BindingFlags.Instance,
					BindingFlags.Public | BindingFlags.Static,
				};

		internal static void Unload()
		{
			Types = null;
			Constructors = null;
			Events = null;
			Fields = null;
			Methods = null;
			Properties = null;
		}

		private static void GetClassUsingName([NotNullWhen(true)] string className, out Type classType)
		{
			if (Types.ContainsKey(className))
			{
				classType = Types[className];
				return;
			}

			Type t = !className.StartsWith("Terraria.") ? ModLoader.GetMod(className.Split('.')[0]).GetType() : typeof(Main);
			classType = t.Assembly.GetType(className);
			if (classType == null)
				throw new ArgumentNullException(nameof(className));

			Types.Add(className, classType);
		}

		public static Type GetClass([NotNullWhen(true)] string className)
		{
			GetClassUsingName(className, out Type classType);
			return classType;
		}

		public static ReflectionAsset<ConstructorInfo> GetConstructor([NotNullWhen(true)] string className, [NotNullWhen(true)] string constructorName, params Type[] parameters)
		{
			if (parameters is null)
				parameters = Array.Empty<Type>();

			string param = string.Empty;
			foreach (Type t in parameters)
				param += t.ToString();

			string param2 = param == string.Empty ? string.Empty : '[' + param + ']';

			if (Constructors.ContainsKey(className + '~' + constructorName + param2))
				goto returnit;

			GetClassUsingName(className, out Type classResult);

			if (!Constructors.ContainsKey(className + '~' + constructorName + param2))
			{
				ConstructorInfo t;
				foreach (BindingFlags flag in possibleFlags)
				{
					t = classResult.GetConstructor(flag, parameters);
					if (t != null)
					{
						ReflectionAsset<ConstructorInfo> asset = new(t);
						Constructors.Add(className + '~' + constructorName + param2, asset);
						return asset;
					}
				}
				throw new ArgumentNullException(nameof(constructorName));
			}

		returnit:
			return Constructors[className + '~' + constructorName + param2];
		}

		public static ReflectionAsset<EventInfo> GetEvent([NotNullWhen(true)] string className, [NotNullWhen(true)] string eventName)
		{
			if (Events.ContainsKey(className + '~' + eventName))
				goto returnit;

			GetClassUsingName(className, out Type classResult);

			if (!Events.ContainsKey(className + '~' + eventName))
			{
				EventInfo t;
				foreach (BindingFlags flag in possibleFlags)
				{
					t = classResult.GetEvent(className, flag);
					if (t != null)
					{
						ReflectionAsset<EventInfo> asset = new(t);
						Events.Add(className + '~' + eventName, asset);
						return asset;
					}
				}
				throw new ArgumentNullException(nameof(eventName));
			}

		returnit:
			return Events[className + '~' + eventName];
		}

		public static ReflectionAsset<FieldInfo> GetField([NotNullWhen(true)] string className, [NotNullWhen(true)] string fieldName)
		{
			if (Fields.ContainsKey(className + '~' + fieldName))
				goto returnit;

			GetClassUsingName(className, out Type classResult);

			if (!Fields.ContainsKey(className + '~' + fieldName))
			{
				FieldInfo t;
				foreach (BindingFlags flag in possibleFlags)
				{
					t = classResult.GetField(fieldName, flag);
					if (t != null)
					{
						ReflectionAsset<FieldInfo> asset = new(t);
						Fields.Add(className + '~' + fieldName, asset);
						return asset;
					}
				}
				throw new ArgumentNullException(nameof(fieldName));
			}

		returnit:
			return Fields[className + '~' + fieldName];
		}

		public static ReflectionAsset<MethodInfo> GetMethod([NotNullWhen(true)] string className, [NotNullWhen(true)] string methodName, params Type[] parameters)
		{
			if (parameters is null)
				parameters = Array.Empty<Type>();

			string param = string.Empty;
			foreach (Type t in parameters)
				param += t.ToString();

			string param2 = param == string.Empty ? string.Empty : '[' + param + ']';

			if (Methods.ContainsKey(className + '~' + methodName + param2))
				goto returnit;

			GetClassUsingName(className, out Type classResult);

			if (!Methods.ContainsKey(className + '~' + methodName + param2))
			{
				MethodInfo t;
				bool hasParamIn = parameters.Length > 0;
				foreach (BindingFlags flag in possibleFlags)
				{
					t = !hasParamIn ? classResult.GetMethod(methodName, flag) : classResult.GetMethod(methodName, flag, parameters);
					if (t != null)
					{
						ReflectionAsset<MethodInfo> asset = new(t);
						Methods.Add(className + '~' + methodName + param2, asset);
						return asset;
					}
				}
				throw new ArgumentNullException(nameof(methodName));
			}

		returnit:
			return Methods[className + '~' + methodName + param2];
		}

		public static ReflectionAsset<PropertyInfo> GetProperty([NotNullWhen(true)] string className, [NotNullWhen(true)] string propertyName)
		{
			if (Properties.ContainsKey(className + '~' + propertyName))
				goto returnit;

			GetClassUsingName(className, out Type classResult);

			if (!Properties.ContainsKey(className + '~' + propertyName))
			{
				PropertyInfo t;
				foreach (BindingFlags flag in possibleFlags)
				{
					t = classResult.GetProperty(propertyName, flag);
					if (t != null)
					{
						ReflectionAsset<PropertyInfo> asset = new(t);
						Properties.Add(className + '~' + propertyName, asset);
						return asset;
					}
				}
				throw new ArgumentNullException(nameof(propertyName));
			}

		returnit:
			return Properties[className + '~' + propertyName];
		}
	}
}

