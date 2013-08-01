using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LowProfile.Fourier.Single
{
	[StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
	public struct Complex
	{
		public float Real;
		public float Imag;

		public Complex(float real, float imag)
		{
			Real = real;
			Imag = imag;
		}

		public static Complex operator -(Complex c1)
		{
			return new Complex(-c1.Real, -c1.Imag);
		}

		public static Complex operator +(Complex c1)
		{
			return new Complex(c1.Real, c1.Imag);
		}

		public static Complex operator +(Complex c1, Complex c2)
		{
			return new Complex(c1.Real + c2.Real, c1.Imag + c2.Imag);
		}

		public static Complex operator +(Complex c1, float c2)
		{
			return new Complex(c1.Real + c2, c1.Imag);
		}

		public static Complex operator +(Complex c1, int c2)
		{
			return new Complex(c1.Real + c2, c1.Imag);
		}

		public static Complex operator -(Complex c1, Complex c2)
		{
			return new Complex(c1.Real - c2.Real, c1.Imag - c2.Imag);
		}

		public static Complex operator *(Complex c1, Complex c2)
		{
			var r = c1.Real * c2.Real - c1.Imag * c2.Imag;
			var i = c1.Real * c2.Imag + c1.Imag * c2.Real;
			return new Complex(r, i);
		}

		public static Complex operator *(Complex c1, float c2)
		{
			return new Complex(c1.Real * c2, c1.Imag * c2);
		}

		public static Complex operator *(Complex c1, int c2)
		{
			return new Complex(c1.Real * c2, c1.Imag * c2);
		}

		public static Complex operator /(Complex c1, float c2)
		{
			return new Complex(c1.Real / c2, c1.Imag / c2);
		}

		public static Complex operator /(Complex c1, int c2)
		{
			return new Complex(c1.Real / c2, c1.Imag / c2);
		}

		public static implicit operator Complex(float rhs)
		{
			return new Complex(rhs, 0);
		}

		public override string ToString()
		{
			var r = Real;
			var i = Imag;
			if (Math.Abs(r) % 1.0 < 0.000000000001)
				r = (float)Math.Round(r);
			if (Math.Abs(i) % 1.0 < 0.000000000001)
				i = (float)Math.Round(i);

			if (i == 0)
				return r.ToString();
			else
				return "(" + r + ", " + i + ")";
		}

		public static Complex I = new Complex(0, 1);

		public static Complex CExp(float phase)
		{
			var x = Math.Cos(phase);
			var y = Math.Sin(phase);
			return new Complex((float)x, (float)y);
		}
	}
}
