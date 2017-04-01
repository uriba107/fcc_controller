#ifndef fcc3_helpers_h_
#define fcc3_helpers_h_

#include <avr/io.h>
#include <avr/wdt.h>
#include <avr/interrupt.h>
#include <util/atomic.h>

#include <LUFA/Drivers/USB/USB.h>

#ifdef abs
#undef abs
#endif

#define min(a,b) ((a)<(b)?(a):(b))
#define max(a,b) ((a)>(b)?(a):(b))
#define abs(x) ((x)>0?(x):-(x))

#ifndef cbi
#define cbi(sfr, bit) (_SFR_BYTE(sfr) &= ~_BV(bit))
#endif
#ifndef sbi
#define sbi(sfr, bit) (_SFR_BYTE(sfr) |= _BV(bit))
#endif
unsigned long millis (void);
void TimerInit(void);

int32_t map(int32_t InVal, int32_t in_min, int32_t in_max, int32_t out_min, int32_t out_max);
int32_t mapLargeNumbers(int32_t inVal, int32_t in_min, int32_t in_max, int32_t out_min, int32_t out_max);
uint8_t Bit_Reverse(uint8_t x );

void RebootToBootloader(void);

#endif
