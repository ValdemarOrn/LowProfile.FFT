using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LowProfile.Fourier.Double
{
	unsafe class FastFFT
	{
		public static void IFFT(Complex* input, Complex* output, Complex* scratchpad, int len)
		{
			var scale = 1.0 / len;

			for(int i = 0; i < len; i++)
			{
				scratchpad[i] = input[i] * scale;
				scratchpad[i].Imag = -scratchpad[i].Imag;
			}

			FFT(scratchpad, output, scratchpad, len);
		}

		public static void FFT(Complex* input, Complex* output, Complex* scratchpad, int len)
		{
			// we use the output buffer and another buffer called scratchpad to work on the
			// signals. Input signal never gets modified
			Complex* A = output;
			Complex* B = scratchpad;

			var bitMap = BitReverse.Tables[len];

			for (int i = 0; i < len; i++)
				A[i] = input[bitMap[i]];
			
			Butterfly2(A, B, len);

			Swap(ref A, ref B);
			Butterfly4(A, B, len);

			Swap(ref A, ref B);
			Butterfly8(A, B, len);

			int butterflySize = 8;

			while(true)
			{
				butterflySize *= 2;

				if (butterflySize > len)
					break;

				Swap(ref A, ref B);
				Butterfly(A, B, butterflySize, len);
			}

			// copy output to the correct buffer
			if (B != output)
			{
				for (int i = 0; i < len; i++)
					output[i] = B[i];
			}
		}

		private static void Swap(ref Complex* inp, ref Complex* outp)
		{
			var temp = inp;
			inp = outp;
			outp = temp;
		}

		private static void Butterfly2(Complex* inp, Complex* outp, int len)
		{
			for(int i = 0; i < len; i += 2)
			{
				outp[i] = inp[i];
				outp[i + 1] = inp[i];

				outp[i] += inp[i + 1];
				outp[i + 1] -= inp[i + 1];
			}
		}

		private static void Butterfly4(Complex* inp, Complex* outp, int len)
		{
			var w = TwiddleFactors.Factors[4];

			for (int i = 0; i < len; i += 4)
			{
				outp[i] = inp[i];
				outp[i + 2] = inp[i];

				outp[i + 1] = inp[i + 1];
				outp[i + 3] = inp[i + 1];

				var x0 = inp[i + 2] * w[0];
				var x1 = inp[i + 3] * w[1];

				outp[i] += x0;
				outp[i + 1] += x1;

				outp[i + 2] -= x0;
				outp[i + 3] -= x1;
			}
		}

		private static void Butterfly8(Complex* inp, Complex* outp, int len)
		{
			var w = TwiddleFactors.Factors[8];

			for (int i = 0; i < len; i += 8)
			{
				outp[i] = inp[i];
				outp[i + 1] = inp[i + 1];
				outp[i + 2] = inp[i + 2];
				outp[i + 3] = inp[i + 3];

				outp[i + 4] = inp[i];
				outp[i + 5] = inp[i + 1];
				outp[i + 6] = inp[i + 2];
				outp[i + 7] = inp[i + 3];

				var x0 = inp[i + 4] * w[0];
				var x1 = inp[i + 5] * w[1];
				var x2 = inp[i + 6] * w[2];
				var x3 = inp[i + 7] * w[3];

				outp[i] += x0;
				outp[i + 1] += x1;
				outp[i + 2] += x2;
				outp[i + 3] += x3;

				outp[i + 4] -= x0;
				outp[i + 5] -= x1;
				outp[i + 6] -= x2;
				outp[i + 7] -= x3;
			}
		}

		private static void Butterfly(Complex* inp, Complex* outp, int stageSize, int len)
		{
			var w = TwiddleFactors.Factors[stageSize];
			var s2 = stageSize / 2;

			for (int n = 0; n < len; n += stageSize)
			{
				var lower = &inp[n];
				var upper = &inp[n + s2];
				var outLower = &outp[n];
				var outhigher = &outp[n + s2];

				for(int i = 0; i < stageSize / 2; i++)
				{
					outLower[i] = lower[i];
					outhigher[i] = lower[i];
				}

				for (int i = 0; i < stageSize / 2; i++)
				{
					upper[i] *= w[i];
				}

				for (int i = 0; i < stageSize / 2; i++)
				{
					outLower[i] += upper[i];
					outhigher[i] -= upper[i];
				}
			}
		}
	}
}
