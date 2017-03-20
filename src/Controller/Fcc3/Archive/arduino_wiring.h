/*
 * arduino_wiring.h
 *
 * Created: 04/03/2017 19:55:25
 *  Author: me
 */ 


#ifndef ARDUINO_WIRING_H_
#define ARDUINO_WIRING_H_

#include <avr/io.h>
#include <avr/interrupt.h>

#ifndef cbi
#define cbi(sfr, bit) (_SFR_BYTE(sfr) &= ~_BV(bit))
#endif
#ifndef sbi
#define sbi(sfr, bit) (_SFR_BYTE(sfr) |= _BV(bit))
#endif

#ifdef abs
#undef abs
#endif

#define min(a,b) ((a)<(b)?(a):(b))
#define max(a,b) ((a)>(b)?(a):(b))
#define abs(x) ((x)>0?(x):-(x))

unsigned long millis(void);
void TimerInit(void);

int32_t map(int32_t InVal, int32_t in_min, int32_t in_max, int32_t out_min, int32_t out_max);
int32_t mapLargeNumbers(int32_t inVal, int32_t in_min, int32_t in_max, int32_t out_min, int32_t out_max);




#endif /* ARDUINO_WIRING_H_ */