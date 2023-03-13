using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace AltLibrary;

internal static partial class LibUtils {
	/// <summary>
	/// Requires <seealso cref="Avx.IsSupported"/> and <seealso cref="Avx2.IsSupported"/> checks before using this.
	/// </summary>
	/// <param name="left"></param>
	/// <param name="right"></param>
	/// <returns></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool EqualsAny(in Vector256<ushort> left, Vector256<ushort> right) {
		right = Avx2.CompareEqual(left, right);
		return !Avx.TestZ(right, right);
	}
}
