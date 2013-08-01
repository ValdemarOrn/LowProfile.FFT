using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LowProfile.Fourier.Single
{
	public class Transform
	{
		public static void Setup()
		{
			BitReverse.Setup();
			TwiddleFactors.Setup();
		}

		private Complex[] Scratchpad;

		public Transform()
		{
			
		}

		public Transform(int length)
		{
			Scratchpad = new Complex[length];
		}

		public void FFT(Complex[] input, Complex[] output)
		{
			if (input.Length != output.Length)
				throw new ArgumentException("Input and output must have the same length");

			if (Scratchpad.Length != input.Length)
				Scratchpad = new Complex[input.Length];

			unsafe
			{
				fixed (Complex* inp = input, scratch = Scratchpad, outp = output)
				{
					FastFFT.FFT(inp, outp, scratch, input.Length);
				}
			}
		}

		public void IFFT(Complex[] input, Complex[] output)
		{
			if (input.Length != output.Length)
				throw new ArgumentException("Input and output must have the same length");

			if (Scratchpad.Length != input.Length)
				Scratchpad = new Complex[input.Length];

			unsafe
			{
				fixed (Complex* inp = input, scratch = Scratchpad, outp = output)
				{
					FastFFT.IFFT(inp, outp, scratch, input.Length);
				}
			}
		}
	}
}
