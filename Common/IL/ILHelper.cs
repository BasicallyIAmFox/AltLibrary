using AltLibrary.Common.Attributes;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using MonoMod.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace AltLibrary.Common.IL;

[LoadableContent(ContentOrder.Init, nameof(Init))]
[LoadableContent(ContentOrder.EarlyContent, nameof(Load), UnloadName = nameof(Unload))]
[LoadableContent(ContentOrder.PostContent, nameof(PostLoad))]
public static class ILHelper {
	private static List<(MethodInfo, Delegate, bool, bool)> IlsAndDetours = new();
	private static MethodInfo ilcursor__insert;
	private static int stackCode = 0;

	private static ILCursor ILCursor__Insert(Func<ILCursor, Instruction, ILCursor> orig, ILCursor self, Instruction instr) {
		stackCode++;
		return orig(self, instr);
	}

	public static void Init() {
		ilcursor__insert = typeof(ILCursor).FindMethod("_Insert");
		if (ilcursor__insert != null) {
			HookEndpointManager.Add(ilcursor__insert, ILCursor__Insert);
		}
	}

	public static void Load() {
		HookUp(
			(e, m) => $"Failed to modify method {m.DeclaringType.Namespace} {m.Name}!",
			HookEndpointManager.Add,
			HookEndpointManager.Modify,
			load => load
		);
	}

	public static void PostLoad() {
		HookUp(
			(e, m) => $"Failed to late-modify method {m.DeclaringType.Namespace} {m.Name}!",
			HookEndpointManager.Add,
			HookEndpointManager.Modify,
			load => !load
		);
	}

	public static void Unload() {
		HookUp(
			(e, m) => $"Failed to unmodify method {m.DeclaringType.Namespace} {m.Name}!",
			HookEndpointManager.Remove,
			HookEndpointManager.Unmodify,
			load => false
		);

		if (ilcursor__insert != null) {
			HookEndpointManager.Remove(ilcursor__insert, ILCursor__Insert);
		}
		IlsAndDetours.Clear();
		IlsAndDetours = null;
	}

	#region Hooking
	private static void HookUp(Func<Exception, MethodInfo, string> errorFunc, Action<MethodInfo, Delegate> actionOn, Action<MethodInfo, Delegate> actionIL, Func<bool, bool> shouldLateLoad) {
		int i = 0;
		stackCode = 0;
		foreach ((MethodInfo method, Delegate callback, bool isDetour, bool lateLoad) in IlsAndDetours) {
			if (shouldLateLoad(lateLoad)) {
				continue;
			}
			try {
				if (isDetour) {
					actionOn(method, callback);
					continue;
				}
				actionIL(method, callback);
			}
			catch (Exception e) {
				AltLibrary.Instance.Logger.Error($"Error Code: {i}-{stackCode:X4}");
				AltLibrary.Instance.Logger.Warn(errorFunc(e, method));
			}
			stackCode = 0;
			i++;
		}
	}

	public static void IL<T>(string methodName, ILContext.Manipulator manipulator, bool lateLoading = false) => IL(typeof(T), methodName, manipulator, lateLoading);
	public static void IL(Type type, string methodName, ILContext.Manipulator manipulator, bool lateLoading = false) => IL(type.FindMethod(methodName), manipulator, lateLoading);
	public static void IL(MethodInfo method, ILContext.Manipulator manipulator, bool lateLoading = false) => IlsAndDetours.Add((method, manipulator, false, lateLoading));

	public static void On<T>(string methodName, Delegate del, bool lateLoading = false) => On(typeof(T), methodName, del, lateLoading);
	public static void On(Type type, string methodName, Delegate del, bool lateLoading = false) => On(type.FindMethod(methodName), del, lateLoading);
	public static void On(MethodInfo method, Delegate del, bool lateLoading = false) => IlsAndDetours.Add((method, del, true, lateLoading));
	#endregion

	#region Extensions
	public static int AddVariable<T>(this ILCursor c) => c.Context.AddVariable<T>();
	public static int AddVariable(this ILCursor c, Type type) => c.Context.AddVariable(type);
	public static int AddVariable(this ILCursor c, TypeReference typeDefinition) => c.Context.AddVariable(typeDefinition);
	public static int AddVariable(this ILCursor c, VariableDefinition variableDefinition) => c.Context.AddVariable(variableDefinition);

	public static int AddVariable<T>(this ILContext context) => context.AddVariable(typeof(T));
	public static int AddVariable(this ILContext context, Type type) => context.AddVariable(new VariableDefinition(context.Import(type)));
	public static int AddVariable(this ILContext context, TypeReference typeDefinition) => context.AddVariable(new VariableDefinition(typeDefinition));
	public static int AddVariable(this ILContext context, VariableDefinition variableDefinition) {
		context.Body.Variables.Add(variableDefinition);
		return context.Body.Variables.Count - 1;
	}
	#endregion

#if DEBUG
	public static void EnableMonoModDump() {
		if (!AltLibrary.MonoModDumbps) {
			return;
		}

		var di = new DirectoryInfo("MonoModDump");
		if (di.Exists) {
			foreach (var fileInfo in di.GetFiles()) {
				fileInfo.Delete();
			}
		}

		Environment.SetEnvironmentVariable("MONOMOD_DMD_TYPE", "cecil"); // auto
		Environment.SetEnvironmentVariable("MONOMOD_DMD_DEBUG", "1");
		string dumpDir = Path.GetFullPath("MonoModDump");
		Directory.CreateDirectory(dumpDir);
		Environment.SetEnvironmentVariable("MONOMOD_DMD_DUMP", dumpDir);
	}

	public static void DisableMonModDump() {
		if (!AltLibrary.MonoModDumbps) {
			return;
		}

		Environment.SetEnvironmentVariable("MONOMOD_DMD_TYPE", "");
		Environment.SetEnvironmentVariable("MONOMOD_DMD_DEBUG", "0");
		Environment.SetEnvironmentVariable("MONOMOD_DMD_DUMP", "");
	}
#endif

	#region https://github.com/blushiemagic/MagicStorage/blob/1.4/Edits/ILHelper.cs
	public static bool LogILEdits { get; set; } =
#if DEBUG
		true
#else
		false
#endif
		;

	public static void CompleteLog(Mod mod, ILCursor c, bool beforeEdit = false) {
		if (!LogILEdits)
			return;

		int index = 0;

		//Get the method name
		string method = c.Method.Name;
		if (!method.Contains("ctor"))
			method = method[(method.LastIndexOf(':') + 1)..];
		else
			method = method[method.LastIndexOf('.')..];

		if (beforeEdit)
			method += " - Before";
		else
			method += " - After";

		//And the storage path
		string path = Program.SavePath;

		//Use the mod type's namespace start
		//Can't use "Mod.Name" since that uses "Mod.File" which might be null
		string modName = mod.GetType().Namespace!;

		if (modName.Contains('.'))
			modName = modName[..modName.IndexOf('.')];

		path = Path.Combine(path, "AltLibrary IL Output", modName);
		Directory.CreateDirectory(path);

		//Get the class name
		string type = c.Method.Name;
		type = type[..type.IndexOf(':')];
		type = type[(type.LastIndexOf('.') + 1)..];

		FileStream file = File.Open(Path.Combine(path, $"{type}.{method}.txt"), FileMode.Create);

		using StreamWriter writer = new(file);

		writer.WriteLine(DateTime.Now.ToString("'['ddMMMyyyy '-' HH:mm:ss']'"));
		writer.WriteLine($"// ILCursor: {c.Method.Name}\n");

		writer.WriteLine("// Arguments:");

		var args = c.Method.Parameters;
		if (args.Count == 0)
			writer.WriteLine($"{"none",8}");
		else {
			foreach (var arg in args) {
				string argIndex = $"[{arg.Index}]";
				writer.WriteLine($"{argIndex,8} {arg.ParameterType.FullName} {arg.Name}");
			}
		}

		writer.WriteLine();

		writer.WriteLine("// Locals:");

		if (!c.Body.HasVariables)
			writer.WriteLine($"{"none",8}");
		else {
			foreach (var local in c.Body.Variables) {
				string localIndex = $"[{local.Index}]";
				writer.WriteLine($"{localIndex,8} {local.VariableType.FullName} V_{local.Index}");
			}
		}

		writer.WriteLine();

		writer.WriteLine("// Body:");
		do {
			PrepareInstruction(c.Instrs[index], out string offset, out string opcode, out string operand);

			writer.WriteLine($"{offset,-10}{opcode,-12} {operand}");
			index++;
		} while (index < c.Instrs.Count);
	}

	public static void UpdateInstructionOffsets(ILCursor c) {
		if (!LogILEdits)
			return;

		var instrs = c.Instrs;
		int curOffset = 0;

		static Instruction[] ConvertToInstructions(ILLabel[] labels) {
			Instruction[] ret = new Instruction[labels.Length];

			for (int i = 0; i < labels.Length; i++)
				ret[i] = labels[i].Target;

			return ret;
		}

		foreach (var ins in instrs) {
			ins.Offset = curOffset;

			if (ins.OpCode != OpCodes.Switch)
				curOffset += ins.GetSize();
			else {
				//'switch' opcodes don't like having the operand as an ILLabel[] when calling GetSize()
				//thus, this is required to even let the mod compile

				Instruction copy = Instruction.Create(ins.OpCode, ConvertToInstructions((ILLabel[])ins.Operand));
				curOffset += copy.GetSize();
			}
		}
	}

	private static void PrepareInstruction(Instruction instr, out string offset, out string opcode, out string operand) {
		offset = $"IL_{instr.Offset:X5}:";

		opcode = instr.OpCode.Name;

		if (instr.Operand is null)
			operand = "";
		else if (instr.Operand is ILLabel label)  //This label's target should NEVER be null!  If it is, the IL edit wouldn't load anyway
			operand = $"IL_{label.Target.Offset:X5}";
		else if (instr.OpCode == OpCodes.Switch)
			operand = "(" + string.Join(", ", (instr.Operand as ILLabel[])!.Select(l => $"IL_{l.Target.Offset:X5}")) + ")";
		else
			operand = instr.Operand.ToString()!;
	}
	#endregion
}