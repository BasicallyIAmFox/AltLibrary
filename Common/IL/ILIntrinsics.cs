using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AltLibrary.Common.IL;

public interface ILIntrinsicMethodImpl {
	static abstract void Emit(ILCursor cursor, EmitOptions options, ref Instruction instruction, int index);
}
public static class ILIntrinsics {
	private delegate void EmitDelegate(ILCursor cursor, EmitOptions options, ref Instruction instruction, int index);
	public sealed class BodyImpl : ILIntrinsicMethodImpl {
		public static void Emit(ILCursor cursor, EmitOptions options, ref Instruction instruction, int index) {
			options.BodyIntrinsics?[index](cursor);
		}
	}
	public sealed class GoToImpl : ILIntrinsicMethodImpl {
		public static void Emit(ILCursor cursor, EmitOptions options, ref Instruction instruction, int index) {
			instruction.OpCode = OpCodes.Br_S;
			instruction.Operand = options.LabelIntrinsics[index].Target;
		}
	}
	public sealed class PopImpl : ILIntrinsicMethodImpl {
		public static void Emit(ILCursor cursor, EmitOptions options, ref Instruction instruction, int index) {
			instruction.OpCode = OpCodes.Pop;
			instruction.Operand = null;
		}
	}
	public sealed class NopImpl : ILIntrinsicMethodImpl {
		public static void Emit(ILCursor cursor, EmitOptions options, ref Instruction instruction, int index) {
			instruction.OpCode = OpCodes.Nop;
			instruction.Operand = null;
		}
	}

	private static readonly EmitOptions emptyEmitOptions = new();

	public static ILCursor EmitDelegateBody(this ILCursor c, Delegate delegateBody, EmitOptions options = null) {
		options ??= emptyEmitOptions;

		var instMap = new Dictionary<Instruction, Instruction>();
		var indexes = new Dictionary<int, int>();

		var def = new DynamicMethodDefinition(delegateBody.Method);
		var body = def.GetILProcessor().Body;

		var instrs = new List<Instruction>(body.Instructions);
		var skipNext = false;

		var i = 0;
		for (; i < instrs.Count; i++) {
			if (instrs[i].OpCode == OpCodes.Ret) {
				instMap[instrs[i]] = c.Next;
				continue;
			}
			else if (skipNext) {
				skipNext = false;
				continue;
			}

			#region Locals
			int index;
			if (instrs[i].MatchLdarg(out int ldargIndex) || instrs[i].MatchStarg(out ldargIndex)) {
				var prm = options.ParameterTypes[ldargIndex - 1];
				var code = instrs[i].MatchLdarg(out _)
					? prm.type == ParamRef.TargetLocal ? OpCodes.Ldloc : OpCodes.Ldarg
					: prm.type == ParamRef.TargetLocal ? OpCodes.Stloc : OpCodes.Starg;
				index = prm.index;
				c.Emit(code, index);
				instMap[instrs[i]] = c.Prev;
				continue;
			}
			else if (instrs[i].MatchLdarga(out ldargIndex)) {
				var prm = options.ParameterTypes[ldargIndex - 1];
				c.Emit(OpCodes.Ldarga, prm.index);
				instMap[instrs[i]] = c.Prev;
				continue;
			}
			else if (instrs[i].MatchLdloc(out int ldlocIndex) || instrs[i].MatchStloc(out ldlocIndex) || instrs[i].MatchLdloca(out ldargIndex)) {
				bool v = instrs[i].MatchLdloc(out _);
				var code = v ? OpCodes.Ldloc : OpCodes.Stloc;
				if (instrs[i].MatchLdloca(out _)) {
					code = OpCodes.Ldloca;
				}
				if (!indexes.TryGetValue(ldlocIndex, out index)) {
					index = indexes[ldlocIndex] = c.Context.AddVariable(body.Variables[ldlocIndex].VariableType.GetElementType());
				}
				c.Emit(code, index);
				instMap[instrs[i]] = c.Prev;
				continue;
			}
			#endregion

			#region Intrinsic Methods
			if (instrs[i].Next.IsIntrinsic(out IntrisicType type, out MethodReference intrinsicMethod, out EmitDelegate fastReflectionDelegate)) {
				Instruction ind;
				if (type == IntrisicType.Indexed) {
					instrs[i - 1].MatchLdcI4(out index);
					instMap[instrs[i]] = c.Prev;

					ind = instrs[i].Next;
					fastReflectionDelegate(c, options, ref ind, index);
					instrs[i + 1] = ind;
					skipNext = true;
					continue;
				}
				else if (type == IntrisicType.Valued) {
					c.Emit(instrs[i].OpCode, instrs[i].Operand);
					skipNext = true;
				}

				ind = instrs[i].Next;
				fastReflectionDelegate(c, options, ref ind, -1);
				instrs[i + 1] = ind;
				instrs[i].Next = ind;
				continue;
			}
			#endregion

			if (instrs[i].Operand == null) {
				c.Emit(instrs[i].OpCode);
				instMap[instrs[i]] = c.Prev;
				continue;
			}
			c.Emit(instrs[i].OpCode, instrs[i].Operand);
			instMap[instrs[i]] = c.Prev;
		}
		foreach (var inst in instMap.Values) {
			if (inst.Operand is Instruction[] oldTargets) {
				var array = new Instruction[oldTargets.Length];
				i = 0;
				foreach (var oldTarget in oldTargets) {
					array[i++] = instMap[oldTarget];
				}
				inst.Operand = array;
			}
			else if (inst.Operand is Instruction oldTarget) {
				inst.Operand = instMap[oldTarget];
			}
		}

		return c;
	}

	private static bool IsIntrinsic(this Instruction instruction, out IntrisicType type, out MethodReference intrinsicMethod, out EmitDelegate fastReflectionDelegate) {
		fastReflectionDelegate = null;
		intrinsicMethod = null;
		type = IntrisicType.None;

		if (instruction?.OpCode == OpCodes.Call && (instruction.Operand is MethodReference || instruction.Operand is GenericInstanceMethod)) {
			intrinsicMethod = (MethodReference)instruction.Operand;

			var attribute = intrinsicMethod?.ResolveReflection().CustomAttributes.FirstOrDefault(a => {
				return a.AttributeType.IsGenericType && a.AttributeType.GetGenericTypeDefinition() == typeof(ILIntrinsicMethodAttribute<>);
			});
			if (attribute != null) {
				type = (IntrisicType)attribute.ConstructorArguments[0].Value;
				fastReflectionDelegate = attribute.AttributeType.GenericTypeArguments[0].FindMethod(nameof(ILIntrinsicMethodImpl.Emit)).CreateDelegate<EmitDelegate>();
				return true;
			}
		}
		return false;
	}

	[ILIntrinsicMethod<BodyImpl>(IntrisicType.Indexed)]
	public static void Body(int index) {
	}

	[ILIntrinsicMethod<GoToImpl>(IntrisicType.Indexed)]
	public static void GoTo(int index) {
	}

	[ILIntrinsicMethod<PopImpl>(IntrisicType.None)]
	public static void Pop() {
	}

	[ILIntrinsicMethod<NopImpl>(IntrisicType.Valued)]
	public static void Push<T>(T value) => throw new NotSupportedException();

	[ILIntrinsicMethod<NopImpl>(IntrisicType.None)]
	public static T Pop<T>() => throw new NotSupportedException();

	[ILIntrinsicMethod<NopImpl>(IntrisicType.Valued)]
	public static void PushRef<T>(ref T value) => throw new NotSupportedException();

	[ILIntrinsicMethod<NopImpl>(IntrisicType.None)]
	public static ref T PopRef<T>() => throw new NotSupportedException();
}
