using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LowProfile.Fourier
{
	unsafe class KissFFT
	{
		[DllImport("KissFFT.dll", EntryPoint = "KISS_Alloc", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr Alloc(int nfft, int inverse_fft);

		[DllImport("KissFFT.dll", EntryPoint = "KISS_FFT", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		static extern void Kiss_FFT(IntPtr cfg, Single.Complex* fin, Single.Complex* fout);

		[DllImport("KissFFT.dll", EntryPoint = "KISS_Cleanup", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern void Cleanup();

		[DllImport("KissFFT.dll", EntryPoint = "KISS_NextFastSize", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern int NextFastSize(int n);

		public static void FFT(IntPtr cfg, Single.Complex[] fin, Single.Complex[] fout)
		{
			fixed(Single.Complex* inp = fin, outp = fout)
			{
				Kiss_FFT(cfg, inp, outp);
			}
		}
	}
}
