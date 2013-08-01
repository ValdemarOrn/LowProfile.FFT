using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LowProfile.Fourier.Single
{
    class TwiddleFactors
    {
        public static readonly Dictionary<int, Complex[]> Factors = new Dictionary<int, Complex[]>();

        public static void Setup()
        {
            int N = 65536;
            Complex[] masterArray = new Complex[N];

            for(int i = 0; i < N/2; i++)
            {
                masterArray[i] = Complex.CExp((float)(-i * 2 * Math.PI / N));
            }

            Factors[N] = masterArray;

            int hop = 1;
            while (N > 2)
            {
                hop = hop * 2;
                N = N / 2;

                var arr = new Complex[N];
                Factors[N] = arr;

                for (int i = 0; i < N/2; i++)
                    arr[i] = masterArray[i * hop];
            }
        }
    }
}
