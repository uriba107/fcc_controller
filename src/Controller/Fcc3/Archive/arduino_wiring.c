/*
 * arduino_wiring.c
 *
 * Created: 04/03/2017 19:47:34
 *  Author: me
 */ 

 #include "arduino_wiring.h"
 

#define interrupts() sei()
#define noInterrupts() cli()

#define clockCyclesPerMicrosecond() ( F_CPU / 1000000L )
#define clockCyclesToMicroseconds(a) ( (a) / clockCyclesPerMicrosecond() )
#define microsecondsToClockCycles(a) ( (a) * clockCyclesPerMicrosecond() )

 // the prescaler is set so that timer0 ticks every 64 clock cycles, and the
 // the overflow handler is called every 256 ticks.
 #define MICROSECONDS_PER_TIMER0_OVERFLOW (clockCyclesToMicroseconds(64 * 256))

 // the whole number of milliseconds per timer0 overflow
 #define MILLIS_INC (MICROSECONDS_PER_TIMER0_OVERFLOW / 1000)

 // the fractional number of milliseconds per timer0 overflow. we shift right
 // by three to fit these numbers into a byte. (for the clock speeds we care
 // about - 8 and 16 MHz - this doesn't lose precision.)
 #define FRACT_INC ((MICROSECONDS_PER_TIMER0_OVERFLOW % 1000) >> 3)
 #define FRACT_MAX (1000 >> 3)

 volatile unsigned long timer0_overflow_count = 0;
 volatile unsigned long timer0_millis = 0;
 static unsigned char timer0_fract = 0;


 ISR(TIMER0_OVF_vect)
 {
	 // copy these to local variables so they can be stored in registers
	 // (volatile variables must be read from memory on every access)
	 unsigned long m = timer0_millis;
	 unsigned char f = timer0_fract;

	 m += MILLIS_INC;
	 f += FRACT_INC;
	 if (f >= FRACT_MAX) {
		 f -= FRACT_MAX;
		 m += 1;
	 }

	 timer0_fract = f;
	 timer0_millis = m;
	 timer0_overflow_count++;
 }

 unsigned long millis()
 {
	 unsigned long m;
	 uint8_t oldSREG = SREG;

	 // disable interrupts while we read timer0_millis or we might get an
	 // inconsistent value (e.g. in the middle of a write to timer0_millis)
	 cli();
	 m = timer0_millis;
	 SREG = oldSREG;

	 return m;
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
		 int ratio = ((((float)out_max - (float)out_min) / ((float)in_max - (float)in_min))*FACTOR);
		 return (((inVal - in_min) * (ratio))/FACTOR) + out_min;
	 }
 }

 void TimerInit(void)
 {
	 // this needs to be called before setup() or some functions won't
	 // work there
	 sei();
	 
	 // on the ATmega168, timer 0 is also used for fast hardware pwm
	 // (using phase-correct PWM would mean that timer 0 overflowed half as often
	 // resulting in different millis() behavior on the ATmega8 and ATmega168)
	 #if defined(TCCR0A) && defined(WGM01)
	 sbi(TCCR0A, WGM01);
	 sbi(TCCR0A, WGM00);
	 #endif

	 // set timer 0 prescale factor to 64
	 #if defined(__AVR_ATmega128__)
	 // CPU specific: different values for the ATmega128
	 sbi(TCCR0, CS02);
	 #elif defined(TCCR0) && defined(CS01) && defined(CS00)
	 // this combination is for the standard atmega8
	 sbi(TCCR0, CS01);
	 sbi(TCCR0, CS00);
	 #elif defined(TCCR0B) && defined(CS01) && defined(CS00)
	 // this combination is for the standard 168/328/1280/2560
	 sbi(TCCR0B, CS01);
	 sbi(TCCR0B, CS00);
	 #elif defined(TCCR0A) && defined(CS01) && defined(CS00)
	 // this combination is for the __AVR_ATmega645__ series
	 sbi(TCCR0A, CS01);
	 sbi(TCCR0A, CS00);
	 #else
	 #error Timer 0 prescale factor 64 not set correctly
	 #endif

	 // enable timer 0 overflow interrupt
	 #if defined(TIMSK) && defined(TOIE0)
	 sbi(TIMSK, TOIE0);
	 #elif defined(TIMSK0) && defined(TOIE0)
	 sbi(TIMSK0, TOIE0);
	 #else
	 #error	Timer 0 overflow interrupt not set correctly
	 #endif

	 // timers 1 and 2 are used for phase-correct hardware pwm
	 // this is better for motors as it ensures an even waveform
	 // note, however, that fast pwm mode can achieve a frequency of up
	 // 8 MHz (with a 16 MHz clock) at 50% duty cycle

	 #if defined(TCCR1B) && defined(CS11) && defined(CS10)
	 TCCR1B = 0;

	 // set timer 1 prescale factor to 64
	 sbi(TCCR1B, CS11);
	 #if F_CPU >= 8000000L
	 sbi(TCCR1B, CS10);
	 #endif
	 #elif defined(TCCR1) && defined(CS11) && defined(CS10)
	 sbi(TCCR1, CS11);
	 #if F_CPU >= 8000000L
	 sbi(TCCR1, CS10);
	 #endif
	 #endif
	 // put timer 1 in 8-bit phase correct pwm mode
	 #if defined(TCCR1A) && defined(WGM10)
	 sbi(TCCR1A, WGM10);
	 #endif

	 // set timer 2 prescale factor to 64
	 #if defined(TCCR2) && defined(CS22)
	 sbi(TCCR2, CS22);
	 #elif defined(TCCR2B) && defined(CS22)
	 sbi(TCCR2B, CS22);
	 //#else
	 // Timer 2 not finished (may not be present on this CPU)
	 #endif

	 // configure timer 2 for phase correct pwm (8-bit)
	 #if defined(TCCR2) && defined(WGM20)
	 sbi(TCCR2, WGM20);
	 #elif defined(TCCR2A) && defined(WGM20)
	 sbi(TCCR2A, WGM20);
	 //#else
	 // Timer 2 not finished (may not be present on this CPU)
	 #endif

	 #if defined(TCCR3B) && defined(CS31) && defined(WGM30)
	 sbi(TCCR3B, CS31);		// set timer 3 prescale factor to 64
	 sbi(TCCR3B, CS30);
	 sbi(TCCR3A, WGM30);		// put timer 3 in 8-bit phase correct pwm mode
	 #endif

	 #if defined(TCCR4A) && defined(TCCR4B) && defined(TCCR4D) /* beginning of timer4 block for 32U4 and similar */
	 sbi(TCCR4B, CS42);		// set timer4 prescale factor to 64
	 sbi(TCCR4B, CS41);
	 sbi(TCCR4B, CS40);
	 sbi(TCCR4D, WGM40);		// put timer 4 in phase- and frequency-correct PWM mode
	 sbi(TCCR4A, PWM4A);		// enable PWM mode for comparator OCR4A
	 sbi(TCCR4C, PWM4D);		// enable PWM mode for comparator OCR4D
	 #else /* beginning of timer4 block for ATMEGA1280 and ATMEGA2560 */
	 #if defined(TCCR4B) && defined(CS41) && defined(WGM40)
	 sbi(TCCR4B, CS41);		// set timer 4 prescale factor to 64
	 sbi(TCCR4B, CS40);
	 sbi(TCCR4A, WGM40);		// put timer 4 in 8-bit phase correct pwm mode
	 #endif
	 #endif /* end timer4 block for ATMEGA1280/2560 and similar */

	 #if defined(TCCR5B) && defined(CS51) && defined(WGM50)
	 sbi(TCCR5B, CS51);		// set timer 5 prescale factor to 64
	 sbi(TCCR5B, CS50);
	 sbi(TCCR5A, WGM50);		// put timer 5 in 8-bit phase correct pwm mode
	 #endif
}
