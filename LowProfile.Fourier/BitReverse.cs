using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LowProfile.Fourier
{
    class BitReverse
    {
        public static readonly Dictionary<int, int[]> Tables = new Dictionary<int, int[]>();
        private static readonly Dictionary<int, int> Bitsize = new Dictionary<int, int>() 
        {
            {4, 2},
            {8, 3},
            {16, 4},
            {32, 5},
            {64, 6},
            {128, 7},
            {256, 8},
            {512, 9},
            {1024, 10},
            {2048, 11},
            {4096, 12},
            {8192, 13},
            {16384, 14},
            {32768, 15},
            {65536, 16}
        };

        public static void Setup()
        {
            int i = 4;
            while (i <= 65536)
            {
                Generate(i);
                i = i * 2;
            }
        }

        private static void Generate(int size)
        {
            var bits = Bitsize[size];
            var table = new int[size];
            Tables[size] = table;

            for(int i = 0; i < size; i++)
            {
                table[i] = Reverse(i, bits);
            }
        }

        private static int Reverse(int input, int bits)
        {
            int output = 0;
            for(int i = 0; i < bits; i++)
            {
                var mask = 1 << i;
                var outmask = 1 << bits - 1 - i;
                if ((mask & input) > 0)
                    output += outmask;
            }

            return output;
        }
    }
}
