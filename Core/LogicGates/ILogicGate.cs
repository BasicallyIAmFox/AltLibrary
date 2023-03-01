namespace AltLibrary.Core.LogicGates;

internal interface ILogicGate {
	static abstract bool Perform(bool b1, bool b2);
	static abstract byte Perform(byte b1, byte b2);
	static abstract sbyte Perform(sbyte b1, sbyte b2);
	static abstract char Perform(char b1, char b2);
	static abstract int Perform(int b1, int b2);
	static abstract uint Perform(uint b1, uint b2);
	static abstract nint Perform(nint b1, nint b2);
	static abstract nuint Perform(nuint b1, nuint b2);
	static abstract long Perform(long b1, long b2);
	static abstract ulong Perform(ulong b1, ulong b2);
	static abstract short Perform(short b1, short b2);
	static abstract ushort Perform(ushort b1, ushort b2);
}
