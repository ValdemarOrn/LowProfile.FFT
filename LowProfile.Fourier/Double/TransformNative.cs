using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LowProfile.Fourier.Double
{
	public unsafe class TransformNative
	{
		[DllImport("LowProfile.Fourier.Native.dll", EntryPoint = "FFT_F", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		static extern void FFT_Native_F(Single.Complex* input, Single.Complex* output, Single.Complex* scratchpad, int len);

		[DllImport("LowProfile.Fourier.Native.dll", EntryPoint = "IFFT_F", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		static extern void IFFT_Native_F(Single.Complex* input, Single.Complex* output, Single.Complex* scratchpad, int len);

		[DllImport("LowProfile.Fourier.Native.dll", EntryPoint = "FFT_D", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		static extern void FFT_Native_D(Double.Complex* input, Double.Complex* output, Double.Complex* scratchpad, int len);

		[DllImport("LowProfile.Fourier.Native.dll", EntryPoint = "IFFT_D", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		static extern void IFFT_Native_D(Double.Complex* input, Double.Complex* output, Double.Complex* scratchpad, int len);

		[DllImport("LowProfile.Fourier.Native.dll", EntryPoint = "Setup", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		static extern void Setup_Native();

		public static void Setup()
		{
			Setup_Native();
		}

		private Complex[] Scratchpad;

		public TransformNative()
		{
			
		}

		public TransformNative(int length)
		{
			Scratchpad = new Complex[length];
		}

		public void FFT(Complex[] input, Complex[] output)
		{
			if (input.Length != output.Length)
				throw new ArgumentException("Input and output must have the same length");

            if (Scratchpad.Length != input.Length)
                Scratchpad = new Complex[input.Length];

            fixed (Complex* inp = input, scratch = Scratchpad, outp = output)
			{
				FFT_Native_D(inp, outp, scratch, input.Length);
			}
		}

		public void IFFT(Complex[] input, Complex[] output)
		{
			if (input.Length != output.Length)
				throw new ArgumentException("Input and output must have the same length");

            if (Scratchpad.Length != input.Length)
                Scratchpad = new Complex[input.Length];

            fixed (Complex* inp = input, scratch = Scratchpad, outp = output)
			{
				IFFT_Native_D(inp, outp, scratch, input.Length);
			}
		}
	}
}
