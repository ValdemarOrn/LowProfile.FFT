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
		Complex[] input;
		Complex[] fft;
		Complex[] ifft;

		double[] inputReal;
		double[] outputReal;
		double[] outputImag;

		private void Setup()
		{
			int len = input.Length;
			FFTn = new Single.TransformNative(len);
			fft = new Single.Complex[len];
			ifft = new Single.Complex[len];
			

			inputReal = input.Select(x => (double)x.Real).ToArray();
			outputReal = new double[len];
			outputImag = new double[len];
		}

		public void TestSingle()
		{
			input = new Complex[] { 42 };
			Setup();

			SimpleDFT.DFT(inputReal, outputReal, outputImag);
			FFTn.FFT(input, fft);
			FFTn.IFFT(fft, ifft);

			CompareFFT();
			CompareIFFT();
		}

		public void TestDouble()
		{
			input = new Complex[] { 12, 99 };
			Setup();

			SimpleDFT.DFT(inputReal, outputReal, outputImag);
			FFTn.FFT(input, fft);
			FFTn.IFFT(fft, ifft);

			CompareFFT();
			CompareIFFT();
		}

		public void TestStep8()
		{
			input = new Complex[] { 1, 1, 1, 1, 0, 0, 0, 0 };
			Setup();

			SimpleDFT.DFT(inputReal, outputReal, outputImag);
			FFTn.FFT(input, fft);
			FFTn.IFFT(fft, ifft);

			CompareFFT();
			CompareIFFT();
		}

		public void TestRamp4()
		{
			input = MakeInputRamp(4);
			Setup();
			SimpleDFT.DFT(inputReal, outputReal, outputImag);
			FFTn.FFT(input, fft);
			FFTn.IFFT(fft, ifft);
			CompareFFT();
			CompareIFFT();
		}

		public void TestRamp8()
		{
			input = MakeInputRamp(8);
			Setup();
			SimpleDFT.DFT(inputReal, outputReal, outputImag);
			FFTn.FFT(input, fft);
			FFTn.IFFT(fft, ifft);
			CompareFFT();
			CompareIFFT();
		}

		public void TestRamp16()
		{
			input = MakeInputRamp(16);
			Setup();
			SimpleDFT.DFT(inputReal, outputReal, outputImag);
			FFTn.FFT(input, fft);
			FFTn.IFFT(fft, ifft);
			CompareFFT();
			CompareIFFT();
		}

		public void TestRamp32()
		{
			input = MakeInputRamp(32);
			Setup();
			SimpleDFT.DFT(inputReal, outputReal, outputImag);
			FFTn.FFT(input, fft);
			FFTn.IFFT(fft, ifft);
			CompareFFT();
			CompareIFFT();
		}

		public void TestRamp1024()
		{
			input = MakeInputRamp(1024);
			Setup();
			SimpleDFT.DFT(inputReal, outputReal, outputImag);
			FFTn.FFT(input, fft);
			FFTn.IFFT(fft, ifft);
			CompareFFT();
			CompareIFFT();
		}

		public void TestNoise4()
		{
			input = MakeInputRandom(4);
			Setup();
			SimpleDFT.DFT(inputReal, outputReal, outputImag);
			FFTn.FFT(input, fft);
			FFTn.IFFT(fft, ifft);
			CompareFFT();
			CompareIFFT();
		}

		public void TestNoise8()
		{
			input = MakeInputRandom(8);
			Setup();
			SimpleDFT.DFT(inputReal, outputReal, outputImag);
			FFTn.FFT(input, fft);
			FFTn.IFFT(fft, ifft);
			CompareFFT();
			CompareIFFT();
		}

		public void TestNoise16()
		{
			input = MakeInputRandom(16);
			Setup();
			SimpleDFT.DFT(inputReal, outputReal, outputImag);
			FFTn.FFT(input, fft);
			FFTn.IFFT(fft, ifft);
			CompareFFT();
			CompareIFFT();
		}

		public void TestNoise32()
		{
			input = MakeInputRandom(32);
			Setup();
			SimpleDFT.DFT(inputReal, outputReal, outputImag);
			FFTn.FFT(input, fft);
			FFTn.IFFT(fft, ifft);
			CompareFFT();
			CompareIFFT();
		}

		public void TestNoise1024()
		{
			input = MakeInputRandom(1024);
			Setup();
			SimpleDFT.DFT(inputReal, outputReal, outputImag);
			FFTn.FFT(input, fft);
			FFTn.IFFT(fft, ifft);
			CompareFFT();
			CompareIFFT();
		}


		private void CompareFFT()
		{
			for (int i = 0; i < ifft.Length; i++)
			{
				if (Math.Abs(fft[i].Real - outputReal[i]) > 0.0001)
					throw new Exception();

				if (Math.Abs(fft[i].Imag - outputImag[i]) > 0.0001)
					throw new Exception();
			}

		}

		private void CompareIFFT()
		{
			for(int i = 0; i < ifft.Length; i++)
			{
				if (Math.Abs(ifft[i].Real - input[i].Real) > 0.0001)
					throw new Exception();

				if (Math.Abs(ifft[i].Imag) > 0.0001)
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
