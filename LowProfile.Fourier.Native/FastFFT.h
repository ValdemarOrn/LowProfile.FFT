#ifndef FASTFFT
#define FASTFFT

#include "Default.h"
#include "Complex.h"

template <class TVal>
class FastFFT
{
public:
	static void IFFT(Complex<TVal>* input, Complex<TVal>* output, Complex<TVal>* scratchpad, const int len);
	static void FFT(Complex<TVal>* input, Complex<TVal>* output, Complex<TVal>* scratchpad, const int len);

	static void Swap(Complex<TVal>** inp, Complex<TVal>** outp);
	static void Butterfly2(Complex<TVal>* inp, Complex<TVal>* outp, const int len);
	static void Butterfly4(Complex<TVal>* inp, Complex<TVal>* outp, const int len);
	static void Butterfly8(Complex<TVal>* inp, Complex<TVal>* outp, const int len);
	static void Butterfly(Complex<TVal>* inp, Complex<TVal>* outp, const int stageSize, const int len);
};

// ------------ Implementation ------------

#include "BitReverse.h"
#include "TwiddleFactors.h"

template <class TVal>
__alwaysinline void FastFFT<TVal>::IFFT(Complex<TVal>* input, Complex<TVal>* output, Complex<TVal>* scratchpad, const int len)
{
	TVal scale = (TVal)(1.0 / len);
	TVal scaleNeg = -scale;

	TVal* scratchPtr = (TVal*)scratchpad;
	TVal* inputPtr = (TVal*)input;

	//for(int i = 0; i < len; i++)
	for(int i = 0; i < len * 2; i+=2)
	{
		scratchPtr[i] = inputPtr[i];
		scratchPtr[i + 1] = inputPtr[i + 1];

		//scratchPtr[i] *= scale;
		//scratchPtr[i] *= scaleNeg;

		//scratchpad[i] = input[i] * scale;
		//scratchpad[i].Imag = -scratchpad[i].Imag;
	}

	FFT(scratchpad, output, scratchpad, len);
}

template <class TVal>
__alwaysinline void FastFFT<TVal>::FFT(Complex<TVal>* input, Complex<TVal>* output, Complex<TVal>* scratchpad, const int len)
{
	// we use the output buffer and another buffer called scratchpad to work on the
	// signals. Input signal never gets modified
	Complex<TVal>* A = output;
	Complex<TVal>* B = scratchpad;

	int* bitMap = BitReverse::Tables[len];

	for (int i = 0; i < len; i++)
		A[i] = input[bitMap[i]];
			
	Butterfly2(A, B, len);

	Swap(&A, &B);
	Butterfly4(A, B, len);

	Swap(&A, &B);
	Butterfly8(A, B, len);

	int butterflySize = 8;

	while(true)
	{
		butterflySize *= 2;

		if (butterflySize > len)
			break;

		Swap(&A, &B);
		Butterfly(A, B, butterflySize, len);
	}

	// copy output to the correct buffer
	if (B != output)
	{
		for (int i = 0; i < len; i++)
			output[i] = B[i];
	}
}

template <class TVal>
__alwaysinline void FastFFT<TVal>::Swap(Complex<TVal>** inp, Complex<TVal>** outp)
{
	Complex<TVal>* temp = *inp;
	*inp = *outp;
	*outp = temp;
}

template <class TVal>
__alwaysinline void FastFFT<TVal>::Butterfly2(Complex<TVal>* inp, Complex<TVal>* outp, const int len)
{
	for(int i = 0; i < len; i += 2)
	{
		outp[i] = inp[i];
		outp[i + 1] = inp[i];

		outp[i] += inp[i + 1];
		outp[i + 1] -= inp[i + 1];
	}
}

template <class TVal>
__alwaysinline void FastFFT<TVal>::Butterfly4(Complex<TVal>* inp, Complex<TVal>* outp, const int len)
{
	Complex<TVal>* w = TwiddleFactors<TVal>::Factors[4];

	for (int i = 0; i < len; i += 4)
	{
		outp[i] = inp[i];
		outp[i + 2] = inp[i];

		outp[i + 1] = inp[i + 1];
		outp[i + 3] = inp[i + 1];

		Complex<TVal> x0, x1;
		Complex<TVal>::Multiply(x0, inp[i + 2], w[0]);
		Complex<TVal>::Multiply(x1, inp[i + 3], w[1]);

		//Complex<TVal> x0 = inp[i + 2] * w[0];
		//Complex<TVal> x1 = inp[i + 3] * w[1];

		outp[i] += x0;
		outp[i + 1] += x1;

		outp[i + 2] -= x0;
		outp[i + 3] -= x1;
	}
}

template <class TVal>
__alwaysinline void FastFFT<TVal>::Butterfly8(Complex<TVal>* inp, Complex<TVal>* outp, const int len)
{
	Complex<TVal>* w = TwiddleFactors<TVal>::Factors[8];

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

		Complex<TVal> x0, x1, x2, x3;
		Complex<TVal>::Multiply(x0, inp[i + 4], w[0]);
		Complex<TVal>::Multiply(x1, inp[i + 5], w[1]);
		Complex<TVal>::Multiply(x2, inp[i + 6], w[2]);
		Complex<TVal>::Multiply(x3, inp[i + 7], w[3]);
		
		//Complex<TVal> x0 = inp[i + 4] * w[0];
		//Complex<TVal> x1 = inp[i + 5] * w[1];
		//Complex<TVal> x2 = inp[i + 6] * w[2];
		//Complex<TVal> x3 = inp[i + 7] * w[3];
		
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

template <class TVal>
__alwaysinline void FastFFT<TVal>::Butterfly(Complex<TVal>* inp, Complex<TVal>* outp, const int stageSize, const int len)
{
	Complex<TVal>* w = TwiddleFactors<TVal>::Factors[stageSize];
	const int s2 = stageSize / 2;

	for (int n = 0; n < len; n += stageSize)
	{
		Complex<TVal>* lower = &inp[n];
		Complex<TVal>* upper = &inp[n + s2];
		Complex<TVal>* outLower = &outp[n];
		Complex<TVal>* outhigher = &outp[n + s2];

		TVal* outLowerFloat = (TVal*)outLower;
		TVal* outhigherFloat = (TVal*)outhigher;
		TVal* wFloat = (TVal*)w;
		TVal* upperFloat = (TVal*)upper;

		for(int i = 0; i < s2; i++)
		{
			outLower[i] = lower[i];
			outhigher[i] = lower[i];
		}

		for (int i = 0; i < s2; i++)
		{
			//upper[i] *= w[i];
			Complex<TVal>::Multiply(upper[i], upper[i], w[i]);
		}

		//for (int i = 0; i < s2; i++)
		for (int i = 0; i < s2 * 2; i+=2)
		{
			//outLower[i] += upper[i];
			outLowerFloat[i] = outLowerFloat[i] + upperFloat[i];
			outLowerFloat[i + 1] = outLowerFloat[i + 1] + upperFloat[i + 1];

			//outhigher[i] -= upper[i];
			outhigherFloat[i] = outhigherFloat[i] - upperFloat[i];
			outhigherFloat[i + 1] = outhigherFloat[i + 1] - upperFloat[i + 1];
		}
	}
}


#endif