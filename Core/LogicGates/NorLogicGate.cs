namespace AltLibrary.Core.LogicGates;

internal struct NorLogicGate : ILogicGate {
	public static bool Perform(bool b1, bool b2) => !(b1 | b2);
	public static byte Perform(byte b1, byte b2) => (byte)~(byte)(b1 | b2);
	public static sbyte Perform(sbyte b1, sbyte b2) => (sbyte)~(sbyte)(b1 | b2);
	public static char Perform(char b1, char b2) => (char)~(char)(b1 | b2);
	public static int Perform(int b1, int b2) => ~(b1 | b2);
	public static uint Perform(uint b1, uint b2) => ~(b1 | b2);
	public static nint Perform(nint b1, nint b2) => ~(b1 | b2);
	public static nuint Perform(nuint b1, nuint b2) => ~(b1 | b2);
	public static long Perform(long b1, long b2) => ~(b1 | b2);
	public static ulong Perform(ulong b1, ulong b2) => ~(b1 | b2);
	public static short Perform(short b1, short b2) => (short)~(short)(b1 | b2);
	public static ushort Perform(ushort b1, ushort b2) => (ushort)~(ushort)(b1 | b2);
}
