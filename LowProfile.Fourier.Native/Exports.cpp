
#include "Complex.h"
#include "FastFFT.h"
#include "BitReverse.h"
#include "TwiddleFactors.h"

extern "C"
{
	__declspec(dllexport) void FFT_F(Complex<float>* input, Complex<float>* output, Complex<float>* scratchpad, int len)
	{
		FastFFT<float>::FFT(input, output, scratchpad, len);
	}

	__declspec(dllexport) void IFFT_F(Complex<float>* input, Complex<float>* output, Complex<float>* scratchpad, int len)
	{
		FastFFT<float>::IFFT(input, output, scratchpad, len);
	}

	__declspec(dllexport) void FFT_D(Complex<double>* input, Complex<double>* output, Complex<double>* scratchpad, int len)
	{
		FastFFT<double>::FFT(input, output, scratchpad, len);
	}

	__declspec(dllexport) void IFFT_D(Complex<double>* input, Complex<double>* output, Complex<double>* scratchpad, int len)
	{
		FastFFT<double>::IFFT(input, output, scratchpad, len);
	}

	__declspec(dllexport) void Setup()
	{
		BitReverse::Setup();
		TwiddleFactors<float>::Setup();
		TwiddleFactors<double>::Setup();
	}
}

