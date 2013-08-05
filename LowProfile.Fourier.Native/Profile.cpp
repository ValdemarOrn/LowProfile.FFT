
#include "FastFFT.h"
#include <iostream>

Complex<float>* MakeData(int bufferSize)
{
    Complex<float>* arr = new Complex<float>[bufferSize];
    for (int j = 0; j < bufferSize; j++)
		arr[j].Real = j / (float)bufferSize;
    return arr;
}

static void TransformNativeSpeedTest(const int bufferSize)
{
	Complex<float>* input = MakeData(bufferSize);
	Complex<float>* output = new Complex<float>[bufferSize];
	Complex<float>* scratch = new Complex<float>[bufferSize];

	int count = 5000000 / bufferSize;
	double sum = 0;
	for (int i=0; i < count; i++)
	{
		FastFFT<float>::FFT(input, output, scratch, bufferSize);
		sum += output[0].Real;
	}
    sum = sum / count;
    std::cout << "Native; FFT; " << bufferSize << " " << count << " " << sum << "\n";

	sum = 0;
	for (int i=0; i < count; i++)
	{
		FastFFT<float>::FFT(output, input, scratch, bufferSize);
		sum += input[0].Real;
	}
    sum = sum / count;
    std::cout << "Native; IFFT; " << bufferSize << " " << count << " " << sum << "\n";
}

int main()
{
	BitReverse::Setup();
	TwiddleFactors<float>::Setup();
	TwiddleFactors<double>::Setup();

	TransformNativeSpeedTest(256);
	TransformNativeSpeedTest(512);
	TransformNativeSpeedTest(4096);
	TransformNativeSpeedTest(32768);
	TransformNativeSpeedTest(65536);

	return 0;
}