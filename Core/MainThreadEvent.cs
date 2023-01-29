using System;
using Terraria;

namespace AltLibrary.Core {
	public sealed class MainThreadEvent : IDisposable {
		private byte flags = 0b00;

		private bool DisposedValue {
			get => (flags & 0b00) == 1;
			set {
				Console.WriteLine(Convert.ToString(flags, toBase: 2));
				flags = (byte)((flags & ~0b00) | value.ToInt());
				Console.WriteLine(Convert.ToString(flags, toBase: 2));
			}
		}
		private bool FinishedRunning {
			get => (flags & 0b10) == 1;
			set {
				Console.WriteLine(Convert.ToString(flags, toBase: 2));
				flags = (byte)((flags & ~0b10) | (value.ToInt() << 1));
				Console.WriteLine(Convert.ToString(flags, toBase: 2));
			}
		}

		public MainThreadEvent() {
		}

		public void Wait() {
			while (!FinishedRunning) ;
		}

		public void Set() {
			FinishedRunning = true;
		}

		private void Dispose(bool disposing) {
			if (!DisposedValue) {
				if (disposing) {
					FinishedRunning = false;
				}
				DisposedValue = true;
			}
		}

		~MainThreadEvent() {
			Dispose(disposing: false);
		}

		public void Dispose() {
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
