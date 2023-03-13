using System;
using System.Reflection.Emit;
using System.Reflection;
using Terraria.ModLoader;

namespace AltLibrary.Core;

internal static class SourceAccess {
	//public static readonly Lazy<Func<string>> Program_test = new(() => GenerateField<Func<string>>(typeof(Program), typeof(string), "test", BindingFlags.Static | BindingFlags.NonPublic));
	
	public static readonly Lazy<Action<GlobalType, ushort>> GlobalType_set_Index = new(()
		=> GenerateMethod<Action<GlobalType, ushort>>(typeof(GlobalType), typeof(void), "set_Index", new Type[] { typeof(ushort) }, BindingFlags.Instance | BindingFlags.NonPublic));

	private static T GenerateField<T>(Type declaringType, Type returnType, string fieldName, BindingFlags flags) where T : Delegate {
		var isStatic = flags.HasFlag(BindingFlags.Static);

		var dm = new DynamicMethod($"<>.Field_{declaringType.FullName}::{returnType.FullName}//{fieldName}",
			returnType, isStatic ? Array.Empty<Type>() : new Type[] { declaringType });
		var il = dm.GetILGenerator();

		if (isStatic) {
			il.Emit(OpCodes.Ldsfld, declaringType.GetField(fieldName, flags)!);
		}
		else {
			il.Emit(OpCodes.Ldarg, 0);
			il.Emit(OpCodes.Ldfld, declaringType.GetField(fieldName, flags)!);
		}
		il.Emit(OpCodes.Ret);

		return dm.CreateDelegate<T>();
	}

	private static T GenerateMethod<T>(Type declaringType, Type returnType, string methodName, Type[] parameters, BindingFlags flags) where T : Delegate {
		return declaringType.GetMethod(methodName, flags, parameters)!.CreateDelegate<T>();
	}
}