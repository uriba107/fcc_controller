#include "fcc3_helpers.h"
// Millis implementation is taken from here
// https://gist.github.com/adnbr/2439125

// Calculate the value needed for
// the CTC match value in OCR1A.
#define CTC_MATCH_OVERFLOW ((F_CPU / 1000UL) / 8UL)

volatile unsigned long timer1_millis;

ISR (TIMER1_COMPA_vect)
{
	timer1_millis++;
}

unsigned long millis (void)
{
	unsigned long millis_return;

	// Ensure this cannot be disrupted
	ATOMIC_BLOCK(ATOMIC_FORCEON) {
		millis_return = timer1_millis;
	}
	return millis_return;
}

void TimerInit(void)
{
	// CTC mode, Clock/8
	TCCR1B |= (1 << WGM12) | (1 << CS11);

	// Load the high byte, then the low byte
	// into the output compare
	OCR1AH = (CTC_MATCH_OVERFLOW >> 8);
	OCR1AL = CTC_MATCH_OVERFLOW;

	// Enable the compare match interrupt
	TIMSK1 |= (1 << OCIE1A);

	// CTC mode, Clock/8
	TCCR1B |= (1 << WGM12) | (1 << CS11);

	// Load the high byte, then the low byte
	// into the output compare
	OCR1AH = (CTC_MATCH_OVERFLOW >> 8);
	OCR1AL = CTC_MATCH_OVERFLOW;

	// Enable the compare match interrupt
	TIMSK1 |= (1 << OCIE1A);
}


int32_t map(int32_t InVal, int32_t in_min, int32_t in_max, int32_t out_min, int32_t out_max)
{
	// check limits to avoid out of bound results
	if (InVal <= in_min) {
		return out_min;
		} else if (InVal >= in_max) {
		return out_max;
		} else {
		// if input checks out, do the math
		return (InVal - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
	}
}

int32_t mapLargeNumbers(int32_t inVal, int32_t in_min, int32_t in_max, int32_t out_min, int32_t out_max)
{
	#define FACTOR 10000
	if (inVal <= in_min) {
		return out_min;
		} else if (inVal >= in_max) {
		return out_max;
		} else {
		int32_t ratio = ((((float)out_max - (float)out_min) / ((float)in_max - (float)in_min))*FACTOR);
		return (((inVal - in_min) * (ratio))/FACTOR) + out_min;
	}
}

uint8_t Bit_Reverse(uint8_t x )
{
	x = ((x >> 1) & 0x55) | ((x << 1) & 0xaa);
	x = ((x >> 2) & 0x33) | ((x << 2) & 0xcc);
	x = ((x >> 4) & 0x0f) | ((x << 4) & 0xf0);
	return x;
}


void RebootToBootloader(void)
{
	uint16_t bootKey = 0x7777;
	uint16_t *const bootKeyPtr = (uint16_t *)0x0800;

	// Stash the magic key
	*bootKeyPtr = bootKey;
	USB_Detach();
	// Set a watchdog timer
	wdt_enable(WDTO_500MS);

	while(1) {} // This infinite loop ensures nothing else
	// happens before the watchdog reboots us
}
