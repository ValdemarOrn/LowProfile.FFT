using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LowProfile.Fourier
{
	public sealed class SimpleDFT
	{
		/// <summary>
		/// Provides the Discrete Fourier Transform for a real-valued input signal
		/// </summary>
		/// <param name="inputReal">the signal to transform</param>
		/// <param name="partials">the maximum number of partials to calculate. If no value is given it defaults to input.length / 2</param>
		/// <returns>The real and imaginary part of each partial</returns>
		public static void DFT(double[] inputReal, double[] outputReal, double[] outputImag)
		{
			int len = inputReal.Length;

			double[] cosDFT = outputReal;
			double[] sinDFT = outputImag;

			for (int n = 0; n < len; n++)
			{
				double cos = 0.0;
				double sin = 0.0;

				for (int i = 0; i < len; i++)
				{
					cos += inputReal[i] * Math.Cos(2 * Math.PI * n / (double)len * i);
					sin -= inputReal[i] * Math.Sin(2 * Math.PI * n / (double)len * i);
				}

				cosDFT[n] = cos;
				sinDFT[n] = sin;
			}
		}

		/// <summary>
		/// Convert cartesian coordinates into polar coordinates
		/// </summary>
		/// <param name="cos"></param>
		/// <param name="sin"></param>
		/// <returns></returns>
		//private static Pair<double[], double[]> ToPolar(double[] cos, double[] sin)
		//{
		//	var length = new double[cos.Length];
		//	var phase = new double[cos.Length];

		//	for (int i = 0; i < length.Length; i++)
		//	{
		//		length[i] = Math.Sqrt(cos[i] * cos[i] + sin[i] * sin[i]);
		//		phase[i] = Math.Atan2(sin[i], cos[i]);
		//	}

		//	return new Pair<double[], double[]>(length, phase);
		//}

		/// <summary>
		/// Adds up all the waves to create a new waveform
		/// </summary>
		/// <param name="partials">Tuple containing real and imaginary components</param>
		/// <returns>the real-valued time-domain signal</returns>
		public static void IDFT(double[] inputReal, double[] inputImag, double[] outputReal)
		{
			if (inputReal.Length != inputImag.Length || inputImag.Length != outputReal.Length)
				throw new ArgumentException("All input arrays must have the same length");

			var len = inputReal.Length;

			var output = outputReal;
			double lenDouble = len;

			for (int k = 0; k < len; k++)
			{
				for (int i = 0; i < len; i++)
				{
					output[i] += inputReal[k] * Math.Cos(2 * Math.PI * i / lenDouble * k)
							   - inputImag[k] * Math.Sin(2 * Math.PI * i / lenDouble * k);
				}
			}

			// divide by N
			var scale = 1.0 / len;
			for (int i = 0; i < len; i++)
				output[i] *= scale;
		}
	}
}
