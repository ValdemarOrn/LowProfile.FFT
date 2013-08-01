

#include "kiss_fft.h"

extern "C"
{
	__declspec(dllexport) kiss_fft_cfg KISS_Alloc(int nfft, int inverse_fft)
	{
		kiss_fft_cfg cfg = kiss_fft_alloc(nfft, inverse_fft, NULL, NULL);
		return cfg;
	}

	__declspec(dllexport) void KISS_FFT(kiss_fft_cfg cfg, const kiss_fft_cpx* fin, kiss_fft_cpx* fout)
	{
		kiss_fft(cfg, fin, fout);
	}

	__declspec(dllexport) void KISS_Cleanup()
	{
		kiss_fft_cleanup();
	}

	__declspec(dllexport) int KISS_NextFastSize(int n)
	{
		return kiss_fft_next_fast_size(n);
	}
}