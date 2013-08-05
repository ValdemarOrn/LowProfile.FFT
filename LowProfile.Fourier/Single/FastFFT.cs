using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TVal = System.Single;

namespace LowProfile.Fourier.Single
{
	unsafe class FastFFT
	{
		public static void IFFT(Complex* input, Complex* output, Complex* scratchpad, int len)
		{
			var scale = 1.0f / len;
			var scaleNeg = -scale;

			TVal* scratchPtr = (TVal*)scratchpad;
			TVal* inputPtr = (TVal*)input;

			for (int i = 0; i < len * 2; i += 2)
			{
				scratchPtr[i] = inputPtr[i] * scale;
				scratchPtr[i + 1] = inputPtr[i + 1] * scaleNeg;
			}

			FFT(scratchpad, output, scratchpad, len);
		}

		public static void FFT(Complex* input, Complex* output, Complex* scratchpad, int len)
		{
			if(len == 1)
			{
				output[0] = input[0];
				return;
			}
			else if(len == 2)
			{
				output[0] = input[0];
				output[1] = input[0];

				Complex.Add(ref output[0], ref input[1]);
				Complex.Subtract(ref output[1], ref input[1]);
				return;
			}

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

			if (len == 4)
				goto end;

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

end:

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

				outp[i].Real += inp[i + 1].Real;
				outp[i].Imag += inp[i + 1].Imag;
				outp[i + 1].Real -= inp[i + 1].Real;
				outp[i + 1].Imag -= inp[i + 1].Imag;
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

				var x0 = inp[i + 2];// *w[0];
				var x1 = inp[i + 3] * w[1];

				outp[i].Real += x0.Real;
				outp[i].Imag += x0.Imag;
				outp[i + 1].Real += x1.Real;
				outp[i + 1].Imag += x1.Imag;
				outp[i + 2].Real -= x0.Real;
				outp[i + 2].Imag -= x0.Imag;
				outp[i + 3].Real -= x1.Real;
				outp[i + 3].Imag -= x1.Imag;
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

				var x0 = inp[i + 4];// *w[0];
				var x1 = inp[i + 5] * w[1];
				var x2 = inp[i + 6] * w[2];
				var x3 = inp[i + 7] * w[3];

				outp[i].Real += x0.Real;
				outp[i].Imag += x0.Imag;
				outp[i + 1].Real += x1.Real;
				outp[i + 1].Imag += x1.Imag;
				outp[i + 2].Real += x2.Real;
				outp[i + 2].Imag += x2.Imag;
				outp[i + 3].Real += x3.Real;
				outp[i + 3].Imag += x3.Imag;

				outp[i + 4].Real -= x0.Real;
				outp[i + 4].Imag -= x0.Imag;
				outp[i + 5].Real -= x1.Real;
				outp[i + 5].Imag -= x1.Imag;
				outp[i + 6].Real -= x2.Real;
				outp[i + 6].Imag -= x2.Imag;
				outp[i + 7].Real -= x3.Real;
				outp[i + 7].Imag -= x3.Imag;
			}
		}

		private static void Butterfly(Complex* inp, Complex* outp, int stageSize, int len)
		{
			var w = TwiddleFactors.Factors[stageSize];
			var s2 = stageSize / 2;
			Complex x = new Complex();

			for (int n = 0; n < len; n += stageSize)
			{
				var lower = &inp[n];
				var upper = &inp[n + s2];
				var outLower = &outp[n];
				var outhigher = &outp[n + s2];

				for(int i = 0; i < s2; i++)
				{
					outLower[i] = lower[i];
					outhigher[i] = lower[i];
				}

				for(int i = 0; i < s2; i++)
				{
					Complex.Multiply(ref x, ref upper[i], ref w[i]);

					Complex.Add(ref outLower[i], ref x);
					Complex.Subtract(ref outhigher[i], ref x);
				}
			}
		}
	}
}
