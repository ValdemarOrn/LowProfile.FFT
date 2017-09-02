using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LowProfile.Fourier.Single;

namespace LowProfile.Fourier
{
	public class Tests
	{
		TransformNative FFTn;
		Transform FFTm;

		Complex[] input;
		Complex[] fftn;
		Complex[] ifftn;

		Complex[] fftm;
		Complex[] ifftm;

		double[] inputReal;
		double[] outputReal;
		double[] outputImag;

		private void Setup()
		{
			int len = input.Length;
			FFTn = new TransformNative(len);
			FFTm = new Transform(len);

			fftn = new Complex[len];
			ifftn = new Complex[len];

			fftm = new Complex[len];
			ifftm = new Complex[len];

			inputReal = input.Select(x => (double)x.Real).ToArray();
			outputReal = new double[len];
			outputImag = new double[len];
		}

		private void RunTest()
		{
			Setup();

			SimpleDFT.DFT(inputReal, outputReal, outputImag);
			FFTn.FFT(input, fftn);
			FFTn.IFFT(fftn, ifftn);

			FFTm.FFT(input, fftm);
			FFTm.IFFT(fftm, ifftm);

			CompareFFTn();
			CompareIFFTn();
			CompareFFTm();
			CompareIFFTm();
		}

		public void TestSingle()
		{
			input = new Complex[] { 42 };
			RunTest();
		}

		public void TestDouble()
		{
			input = new Complex[] { 12, 99 };
			RunTest();
		}

		public void TestStep8()
		{
			input = new Complex[] { 1, 1, 1, 1, 0, 0, 0, 0 };
			RunTest();
		}

		public void TestRamp4()
		{
			input = MakeInputRamp(4);
			RunTest();
		}

		public void TestRamp8()
		{
			input = MakeInputRamp(8);
			RunTest();
		}

		public void TestRamp16()
		{
			input = MakeInputRamp(16);
			RunTest();
		}

		public void TestRamp32()
		{
			input = MakeInputRamp(32);
			RunTest();
		}

		public void TestRamp1024()
		{
			input = MakeInputRamp(1024);
			RunTest();
		}

		public void TestNoise4()
		{
			input = MakeInputRandom(4);
			RunTest();
		}

		public void TestNoise8()
		{
			input = MakeInputRandom(8);
			RunTest();
		}

		public void TestNoise16()
		{
			input = MakeInputRandom(16);
			RunTest();
		}

		public void TestNoise32()
		{
			input = MakeInputRandom(32);
			RunTest();
		}

		public void TestNoise1024()
		{
			input = MakeInputRandom(1024);
			RunTest();
		}

        public void TestComplexArg()
        {
            for (int i = 0; i < 100; i++)
            {
                var phase = Math.PI * 2 * i / 100.0;
                var complex = Double.Complex.CExp(phase);
                var argBefore = complex.Arg;
                complex.Arg = complex.Arg;
                var argAfter = complex.Arg;

                if (Math.Abs(argBefore - argAfter) > 0.00001)
                    throw new Exception();

                var pp = (phase <= Math.PI) ? phase : phase - Math.PI * 2;

                if (Math.Abs(argBefore - pp) > 0.00001)
                    throw new Exception();
            }
        }

		private void CompareFFTn()
		{
			for (int i = 0; i < ifftn.Length; i++)
			{
				if (Math.Abs(fftn[i].Real - outputReal[i]) > 0.0001)
					throw new Exception();

				if (Math.Abs(fftn[i].Imag - outputImag[i]) > 0.0001)
					throw new Exception();
			}

		}

		private void CompareIFFTn()
		{
			for(int i = 0; i < ifftn.Length; i++)
			{
				if (Math.Abs(ifftn[i].Real - input[i].Real) > 0.0001)
					throw new Exception();

				if (Math.Abs(ifftn[i].Imag) > 0.0001)
					throw new Exception();
			}
		}

		private void CompareFFTm()
		{
			for (int i = 0; i < ifftn.Length; i++)
			{
				if (Math.Abs(fftm[i].Real - outputReal[i]) > 0.0001)
					throw new Exception();

				if (Math.Abs(fftm[i].Imag - outputImag[i]) > 0.0001)
					throw new Exception();
			}

		}

		private void CompareIFFTm()
		{
			for (int i = 0; i < ifftn.Length; i++)
			{
				if (Math.Abs(ifftm[i].Real - input[i].Real) > 0.0001)
					throw new Exception();

				if (Math.Abs(ifftm[i].Imag) > 0.0001)
					throw new Exception();
			}
		}

		private Complex[] MakeInputRamp(int bufferSize)
		{
			var arr = new Complex[bufferSize];
			for (int j = 0; j < bufferSize; j++)
				arr[j].Real = j / (float)bufferSize;
			return arr;
		}

		private Complex[] MakeInputRandom(int bufferSize)
		{
			var arr = new Complex[bufferSize];
			var rand = new Random(42);
			for (int j = 0; j < bufferSize; j++)
				arr[j].Real = (float)(rand.NextDouble() * 10.0 - 5.0);
			return arr;
		}


	}
}
