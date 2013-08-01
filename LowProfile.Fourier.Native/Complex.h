
#ifndef COMPLEX
#define COMPLEX

#include "Default.h"

#pragma pack(push, 4)
template <class TVal>
class Complex
{
public:
	TVal Real;
	TVal Imag;

	Complex();
	Complex(const TVal real, const TVal imag);

	Complex operator - ();
	Complex operator + ();

	Complex operator + (const Complex);
	Complex operator + (const TVal);
	Complex operator + (const int);

	Complex& operator += (const Complex);
	Complex& operator -= (const Complex);
	Complex& operator *= (const Complex);

	Complex operator - (const Complex);

	Complex operator * (const Complex);
	Complex operator * (const TVal);
	Complex operator * (const int);

	Complex operator / (const TVal);
	Complex operator / (const int);

	static Complex CExp(const TVal phase);
	static void Multiply(Complex& dest, Complex& c1, Complex& c2);
	static const Complex I;
};
#pragma pack(pop)

// ------------ Implementation ------------

#include<cmath>

template <class TVal>
const Complex<TVal> Complex<TVal>::I = Complex<TVal>(0, 1);

template <class TVal>
__alwaysinline Complex<TVal>::Complex()
{
	Real = 0;
	Imag = 0;
}

template <class TVal>
__alwaysinline Complex<TVal>::Complex(const TVal real, const TVal imag)
{
	Real = real;
	Imag = imag;
}

template <class TVal>
__alwaysinline Complex<TVal> Complex<TVal>::operator - ()
{
	return Complex<TVal>(-Real, -Imag);
}

template <class TVal>
__alwaysinline Complex<TVal> Complex<TVal>::operator + ()
{
	return Complex<TVal>(Real, Imag);
}

template <class TVal>
__alwaysinline Complex<TVal> Complex<TVal>::operator + (const Complex c2)
{
	return Complex<TVal>(Real + c2.Real, Imag + c2.Imag);
}

template <class TVal>
__alwaysinline Complex<TVal> Complex<TVal>::operator + (const TVal r)
{
	return Complex<TVal>(Real + r, Imag);
}

template <class TVal>
__alwaysinline Complex<TVal> Complex<TVal>::operator + (const int r)
{
	return Complex<TVal>(Real + r, Imag);
}

template <class TVal>
__alwaysinline Complex<TVal>& Complex<TVal>::operator += (const Complex c2)
{
	Real = Real + c2.Real;
	Imag = Imag + c2.Imag;
	return *this;
}

template <class TVal>
__alwaysinline Complex<TVal>& Complex<TVal>::operator -= (const Complex c2)
{
	Real = Real - c2.Real;
	Imag = Imag - c2.Imag;
	return *this;
}

template <class TVal>
__alwaysinline Complex<TVal>& Complex<TVal>::operator *= (const Complex c2)
{
	TVal r = Real * c2.Real - Imag * c2.Imag;
	TVal i = Real * c2.Imag + Imag * c2.Real;
	Real = r;
	Imag = i;
	return *this;
}

template <class TVal>
__alwaysinline Complex<TVal> Complex<TVal>::operator - (const Complex c2)
{
	return Complex<TVal>(Real - c2.Real, Imag - c2.Imag);
}

template <class TVal>
__alwaysinline Complex<TVal> Complex<TVal>::operator * (const Complex c2)
{
	TVal r = Real * c2.Real - Imag * c2.Imag;
	TVal i = Real * c2.Imag + Imag * c2.Real;
	return Complex<TVal>(r, i);
}

template <class TVal>
__alwaysinline Complex<TVal> Complex<TVal>::operator * (const TVal r)
{
	return Complex<TVal>(Real * r, Imag * r);
}

template <class TVal>
__alwaysinline Complex<TVal> Complex<TVal>::operator * (const int r)
{
	return Complex<TVal>(Real * r, Imag * r);
}

template <class TVal>
__alwaysinline Complex<TVal> Complex<TVal>::operator / (const TVal r)
{
	return Complex<TVal>(Real / r, Imag / r);
}

template <class TVal>
__alwaysinline Complex<TVal> Complex<TVal>::operator / (const int r)
{
	return Complex<TVal>(Real / r, Imag / r);
}

template <class TVal>
__alwaysinline Complex<TVal> Complex<TVal>::CExp(const TVal phase)
{
	TVal x = cos(phase);
	TVal y = sin(phase);
	return Complex<TVal>(x, y);
}


// ------------- fast operations -------------

template <class TVal>
__alwaysinline void Complex<TVal>::Multiply(Complex<TVal>& dest, Complex<TVal>& c1, Complex<TVal>& c2)
{
	TVal r = c1.Real * c2.Real - c1.Imag * c2.Imag;
	TVal i = c1.Real * c2.Imag + c1.Imag * c2.Real;
	dest.Real = r;
	dest.Imag = i;
}


#endif
