

#if defined _MSC_VER

#define __alwaysinline __forceinline

// Note: These are untested.
#elif defined __GNUC__

#define __alwaysinline __attribute__((always_inline))

#endif