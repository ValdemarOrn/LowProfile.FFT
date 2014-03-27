using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LowProfile.Fourier
{
	class Program
	{
		unsafe static void Main(string[] args)
		{
			Single.Transform.Setup();
			Single.TransformNative.Setup();

			//RunUnitTests();
			RunSpeedTests();
			//Console.ReadLine();
		}

		private static void RunUnitTests()
		{
			try
			{
				var t = new Tests();
				t.TestSingle();
				t.TestDouble();
				t.TestStep8();

				t.TestRamp4();
				t.TestRamp8();
				t.TestRamp16();
				t.TestRamp32();
				t.TestRamp1024();

				t.TestNoise4();
				t.TestNoise8();
				t.TestNoise16();
				t.TestNoise32();
				t.TestNoise1024();

				Console.WriteLine("Unit tests completed successfully");
			}
			catch(Exception)
			{
				Console.WriteLine("UNIT TESTS FAILED");
				throw;
			}
		}

		private static void RunSpeedTests()
		{
			/*Console.WriteLine("\n------------ Warmup Run ------------\n");

			ExocortexSpeedTest(256);
			TransformSpeedTest(256);
			TransformNativeSpeedTest(256);
			KissSpeedTest(256);


			Console.WriteLine("\n------------ Starting tests ------------\n");

			ExocortexSpeedTest(256);
			ExocortexSpeedTest(512);
			ExocortexSpeedTest(4096);

			Console.WriteLine("");*/

			TransformSpeedTest(256);
			TransformSpeedTest(512);
			TransformSpeedTest(4096);
			TransformSpeedTest(32768);
			TransformSpeedTest(65536);

			/*Console.WriteLine("");

			TransformNativeSpeedTest(256);
			TransformNativeSpeedTest(512);
			TransformNativeSpeedTest(4096);
			TransformNativeSpeedTest(32768);
			TransformNativeSpeedTest(65536);

			Console.WriteLine("");

			KissSpeedTest(256);
			KissSpeedTest(512);
			KissSpeedTest(4096);
			KissSpeedTest(32768);
			KissSpeedTest(65536);*/
		}

		private static void KissSpeedTest(int bufferSize)
		{
			var cfg = KissFFT.Alloc(bufferSize, 0);

			var input = MakeData(bufferSize);
			var output = new Single.Complex[bufferSize];

			double sum = 0;
			long count = 0;
			var start = DateTime.Now;
			while ((DateTime.Now - start).TotalMilliseconds < 1000)
			{
				KissFFT.FFT(cfg, input, output);
				sum += output[0].Real;
				count++;
			}
			sum = sum / count;
			Console.WriteLine("KISS; FFT; {0}; {1}; {2}", bufferSize, count, sum);

			cfg = KissFFT.Alloc(bufferSize, 1);

			sum = 0;
			count = 0;
			start = DateTime.Now;
			while ((DateTime.Now - start).TotalMilliseconds < 1000)
			{
				KissFFT.FFT(cfg, output, input);
				sum += input[0].Real;
				count++;
			}
			sum = sum / count;
			Console.WriteLine("KISS; IFFT; {0}; {1}; {2}", bufferSize, count, sum);
		}

		static void TransformSpeedTest(int bufferSize)
		{
			var input = MakeData(bufferSize);
			var output = new Single.Complex[bufferSize];
			var fft = new Single.Transform(bufferSize);

			double sum = 0;
			long count = 0;
			var start = DateTime.Now;
			while ((DateTime.Now - start).TotalMilliseconds < 1000)
			{
				fft.FFT(input, output);
				sum += output[0].Real;
				count++;
			}
			sum = sum / count;
			Console.WriteLine("Managed; FFT; {0}; {1}; {2}", bufferSize, count, sum);

			sum = 0;
			count = 0;
			start = DateTime.Now;
			while ((DateTime.Now - start).TotalMilliseconds < 1000)
			{
				fft.IFFT(output, input);
				sum += input[0].Real;
				count++;
			}
			sum = sum / count;
			Console.WriteLine("Managed; IFFT; {0}; {1}; {2}", bufferSize, count, sum);
		}

		static void TransformNativeSpeedTest(int bufferSize)
		{
			var input = MakeData(bufferSize);
			var output = new Single.Complex[bufferSize];
			var fft = new Single.TransformNative(bufferSize);

			double sum = 0;
			long count = 0;
			var start = DateTime.Now;
			while ((DateTime.Now - start).TotalMilliseconds < 1000)
			{
				fft.FFT(input, output);
				sum += output[0].Real;
				count++;
			}
			sum = sum / count;
			Console.WriteLine("Native; FFT; {0}; {1}; {2}", bufferSize, count, sum);

			sum = 0;
			count = 0;
			start = DateTime.Now;
			while ((DateTime.Now - start).TotalMilliseconds < 1000)
			{
				fft.IFFT(output, input);
				sum += input[0].Real;
				count++;
			}
			sum = sum / count;
			Console.WriteLine("Native; IFFT; {0}; {1}; {2}", bufferSize, count, sum);
		}

		static Single.Complex[] MakeData(int bufferSize)
		{
			var arr = new Single.Complex[bufferSize];
			for (int j = 0; j < bufferSize; j++)
				arr[j].Real = j / (float)bufferSize;
			return arr;
		}

		static Double.Complex[] MakeDataDouble(int bufferSize)
		{
			var arr = new Double.Complex[bufferSize];
			for (int j = 0; j < bufferSize; j++)
				arr[j].Real = j / (double)bufferSize;
			return arr;
		}
	}
}


