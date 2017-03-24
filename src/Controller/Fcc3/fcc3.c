#include "fcc3.h"

#define STICK_DEADZONE 10

// 9Kg/f
#define FORCE_9KGF 1638
// 6Kg/f
#define FORCE_6KGF 1092
// 4.5Kg/f
#define FORCE_4KGF 819
// 3Kg/f
#define FORCE_3KGF 546


// configure Global EEPROM pointers
uint16_t EEMEM NonVolatilePitchMin;
uint16_t EEMEM NonVolatilePitchMax;
uint16_t EEMEM NonVolatileRollMin;
uint16_t EEMEM NonVolatileRollMax;
uint16_t EEMEM NonVolatileUserForce;

uint8_t EEMEM NonVolatileOptions;

// Global vars to store EEPROM values for runtime
StickLimit gStickLimits;
uint8_t gOptions;
int16_t gUserDefinedForce;

// Global vars for Config modes
bool gIsConfig = false;
uint8_t gPowerLed = 255;
uint32_t gConfigTimer = 0;


/////////// Init/Setup Functions ////////////////
void InitFCC(void)
{
	SetupSPI();

	TimerInit();
	SetupLeds();
	// Now enable global interrupts
	sei();

	ReadStickZero();
	ReadMem();
}


void SetupSPI(void)
{
	// Set Grip CS (PE6 - Arduino Pin7)
	// CS should rest high
	DDRE |= (1<<DDE6);// set to output
	PORTE |= (1<<PORTE6);// pull high

	// ADC CS (PC6 - Arduino Pin5)
	DDRC |= (1<<DDC6);
	PORTC |= (1<<PORTC6);

	SPI_Init(SPI_SPEED_FCPU_DIV_4|SPI_MODE_MASTER);

	// Set PF0 and PF1 to output for Axis Zero Led
	// Done Here because ADC is check is driving those pins..
	DDRF |= (1<<PORTF0) | (1 << PORTF1);
}

void SetupLeds(void)
{
	// Set PC7 (Arduino Pin 13) For PWM
	// Set pin to output
	DDRC |= (1<<DDC7);

	// initiallize PWM4A
	// TCCR4A |= (1 << PWM4A)
	cbi(TCCR4A, COM4A0);

	// Now turn LED on for full cycle
	setStatusLed(gPowerLed);
}


void setStatusLed(uint8_t power){
	if (power == 0 ){
		cbi(TCCR4A, COM4A1);
		PORTC &= ~(1<<PORTC7);
	} else if (power == 255) {
		cbi(TCCR4A, COM4A1);
		PORTC |= (1<<PORTC7);
	} else {
		// set timers. pin is OC4A in PMW
//		TCCR4A |= (1 << PWM4A) |(1 << COM4A1);
//		TCCR4A &= ~(1 << COM4A0);
		sbi(TCCR4A, COM4A1);
		OCR4A = power;
	}

}


///////// EEPROM /////////////////////
void WriteMem(void){
	// This function will write to EEPROM any values that had been changed in runtime (to save EEPROM lifetime)
	//int16_t volatile VolatilePitchMin;
	//int16_t volatile VolatilePitchMax;
//
	//int16_t volatile VolatileRollMin;
	//int16_t volatile VolatileRollMax;

	uint8_t volatile VolatileOptions;

	int16_t volatile VolatileUserForce;

	// Read EEPROM
	//VolatileRollMax = eeprom_read_word(&NonVolatileRollMax);
	//VolatileRollMin = eeprom_read_word(&NonVolatileRollMin);
//
	//VolatilePitchMax = eeprom_read_word(&NonVolatilePitchMax);
	//VolatilePitchMin = eeprom_read_word(&NonVolatilePitchMin);

	VolatileOptions = eeprom_read_byte(&NonVolatileOptions);
    VolatileUserForce = eeprom_read_word(&NonVolatileUserForce);

	// Compare values and write back
	if (VolatileOptions != gOptions) {
		gOptions &= ~(RebootDevice); //make sure RebootFlag doesn't carry over
		eeprom_write_byte(&NonVolatileOptions, gOptions);
	}
	if (gUserDefinedForce != VolatileUserForce) {
		eeprom_write_word(&NonVolatileUserForce,gUserDefinedForce);
	}

    //if (gOptions & ForceUserDefined) {
		//if (VolatileRollMax != gStickLimits.X.max) {
			//eeprom_write_word(&NonVolatileRollMax,gStickLimits.X.max);
		//}
//
		//if (VolatileRollMin != gStickLimits.X.min) {
			//eeprom_write_word(&NonVolatileRollMin,gStickLimits.X.min);
		//}
//
		//if (VolatilePitchMax != gStickLimits.Y.max) {
			//eeprom_write_word(&NonVolatilePitchMax,gStickLimits.Y.max);
		//}
//
		//if (VolatilePitchMin != gStickLimits.Y.min) {
			//eeprom_write_word(&NonVolatilePitchMin,gStickLimits.Y.min);
		//}
	//}
}

void ReadMem(void){
	// Read config defaults from EEPROM
	//int16_t volatile VolatilePitchMin;
	//int16_t volatile VolatilePitchMax;
//
	//int16_t volatile VolatileRollMin;
	//int16_t volatile VolatileRollMax;

	uint8_t volatile VolatileOptions;
	int16_t volatile VolatileUserForce;

	// Read EEPROM
	//VolatileRollMax = eeprom_read_word(&NonVolatileRollMax);
	//VolatileRollMin = eeprom_read_word(&NonVolatileRollMin);
//
	//VolatilePitchMax = eeprom_read_word(&NonVolatilePitchMax);
	//VolatilePitchMin = eeprom_read_word(&NonVolatilePitchMin);

	VolatileOptions = eeprom_read_byte(&NonVolatileOptions);
	VolatileOptions &= ~(RebootDevice); //make sure RebootFlag doesn't carry over
    VolatileUserForce = eeprom_read_word(&NonVolatileUserForce);


	// Check and place values
	#define EEPROM_EMPTY_BYTE(b) b == 0xFF
	#define EEPROM_EMPTY_WORD(w) w == 0xFFFF

	// result = a > b ? x : y;

	gOptions = EEPROM_EMPTY_BYTE(VolatileOptions) ? Force4Kg : VolatileOptions;
	gUserDefinedForce = EEPROM_EMPTY_WORD(VolatileUserForce) ? FORCE_3KGF : VolatileUserForce;

	// do all the force calcs on init.
	ChangeSensitivity(gOptions);
}

////////////////////// Stick Config functions ////////////////////////////

void ReadStickZero(void){
	gStickLimits.X.center = (int32_t)(ReadRoll+ReadRoll+ReadRoll+ReadRoll+ReadRoll)/5;
	gStickLimits.Y.center = (int32_t)(ReadPitch+ReadPitch+ReadPitch+ReadPitch+ReadPitch)/5;
}

void ChangeSensitivity(uint8_t sensitivity){
	// Clear current force settings
	gOptions &= ~(ForceAll);
	// now set sensitivity bit
	gOptions |= (sensitivity);

	// now load correct config
	if (gOptions & Force4Kg) {
		SetCalibratedSensitivity(FORCE_4KGF);
	} else 	if (gOptions & Force6Kg) {
		SetCalibratedSensitivity(FORCE_6KGF);
	} else 	if (gOptions & Force9Kg) {
		SetCalibratedSensitivity(FORCE_9KGF);
	} else {
		SetCalibratedSensitivity(gUserDefinedForce);
//		LoadUserSensitivity();
	}
}

void SetCalibratedSensitivity(int16_t OffsetValue){
	gStickLimits.X.max = OffsetValue;
	gStickLimits.X.min = -OffsetValue;
	gStickLimits.Y.max = OffsetValue;
	gStickLimits.Y.min = -OffsetValue;

	// do all the force calcs.
	CalcForceMapping();
}

void CalcForceMapping(void) {
	// do all the force calcs.
	gStickLimits.N180 = abs(gStickLimits.Y.max);
	gStickLimits.N80 = abs(gStickLimits.N180/2.25);
}


void ReadMinMax(void){
	if (gOptions & ForceUserDefined) {
		int16_t Roll = ReadRoll - gStickLimits.X.center;
		int16_t Pitch = ReadPitch - gStickLimits.Y.center;

		gStickLimits.X.max = max(Roll,gStickLimits.X.max);
		gStickLimits.X.min = min(Roll,gStickLimits.X.min);

		gStickLimits.Y.max = max(Pitch,gStickLimits.Y.max);
		gStickLimits.Y.min = min(Pitch,gStickLimits.Y.min);

		if (gStickLimits.N180 != gStickLimits.Y.max) {
			CalcForceMapping();
		}
	}
}

void LoadUserSensitivity(){
	// Read config defaults from EEPROM
	int16_t volatile VolatilePitchMin;
	int16_t volatile VolatilePitchMax;

	int16_t volatile VolatileRollMin;
	int16_t volatile VolatileRollMax;

		// Read EEPROM
	VolatileRollMax = eeprom_read_word(&NonVolatileRollMax);
	VolatileRollMin = eeprom_read_word(&NonVolatileRollMin);

	VolatilePitchMax = eeprom_read_word(&NonVolatilePitchMax);
	VolatilePitchMin = eeprom_read_word(&NonVolatilePitchMin);

	#define BlankUserDefined STICK_DEADZONE+1
	gStickLimits.X.max = EEPROM_EMPTY_WORD(VolatileRollMax) ? BlankUserDefined : VolatileRollMax;
	gStickLimits.X.min = EEPROM_EMPTY_WORD(VolatileRollMin) ? -BlankUserDefined : VolatileRollMin;
	gStickLimits.Y.max = EEPROM_EMPTY_WORD(VolatilePitchMax) ? BlankUserDefined : VolatilePitchMax;
	gStickLimits.Y.min = EEPROM_EMPTY_WORD(VolatilePitchMin) ? -BlankUserDefined : VolatilePitchMin;

	CalcForceMapping();
}

void WipeStoredLimits(void){
	int16_t volatile VolatilePitchMin;
	int16_t volatile VolatilePitchMax;

	int16_t volatile VolatileRollMin;
	int16_t volatile VolatileRollMax;

	VolatileRollMax = eeprom_read_word(&NonVolatileRollMax);
	VolatileRollMin = eeprom_read_word(&NonVolatileRollMin);

	VolatilePitchMax = eeprom_read_word(&NonVolatilePitchMax);
	VolatilePitchMin = eeprom_read_word(&NonVolatilePitchMin);

	#define BlankUserDefined STICK_DEADZONE+1
			if (VolatileRollMax != BlankUserDefined) {
				eeprom_write_word(&NonVolatileRollMax,BlankUserDefined);
			}

			if (VolatileRollMin != -BlankUserDefined) {
				eeprom_write_word(&NonVolatileRollMin,-BlankUserDefined);
			}

			if (VolatilePitchMax != BlankUserDefined) {
				eeprom_write_word(&NonVolatilePitchMax,BlankUserDefined);
			}

			if (VolatilePitchMin != -BlankUserDefined) {
				eeprom_write_word(&NonVolatilePitchMin,-BlankUserDefined);
			}
}

///////////// Actual Stick stuff ////////
int16_t readSPIADC(uint8_t adcChannel){
	PORTC &= ~(1<<PINC6); // Pull shift register CS low
	SPI_SendByte(0x01); //send start
	int16_t result = (((SPI_TransferByte(0xa0 | (adcChannel << 6)) & ~(0xF0)) << 8) | (SPI_ReceiveByte())) - 2048;
	PORTC |= (1<<PINC6); // Pull Shift register CS high

	// Add a very not streamlined thing here (might be better to move to a different place later).
	// If output is exactly 0 (i.e entered voltage, light up the correct LED on PORTF - PF0 for pitch, PF1 for roll)
	if (abs(result) - STICK_DEADZONE*2 <= 0) {
		PORTF |= (1<<adcChannel);// pull high (led on)
	}
	else
	{
		PORTF &= ~(1<<adcChannel);// pull low (led off)
	}

	return result;
}

void ReadStick(AxisStore* AxisData)
{
	static AxisStore StickHistory;
	AxisStore NewData;
	static bool isForceMapped;
	isForceMapped = (gOptions & ForceMap)?true:false;

	//store current value (not sure it works for now)
	NewData.X = ReadRoll - gStickLimits.X.center;
	NewData.Y = ReadPitch - gStickLimits.Y.center;
	if (gOptions & RotatedSensors) {
		RotateFlcs(&NewData);
	}

	MapStick(&NewData,isForceMapped);


	AxisData->X = (int32_t)(((int32_t)((NewData.X)*2)+(int32_t)(StickHistory.X))/3);
	AxisData->Y = (int32_t)(((int32_t)((NewData.Y)*2)+(int32_t)(StickHistory.Y))/3);

	StickHistory.Y = AxisData->Y;
	StickHistory.X = AxisData->X;
}

void RotateFlcs(AxisStore* AxisData)
{
	// F-16 FLCS sensor assembly is rotated 12.5 degrees clockwise from axis.
	// This function receives raw stick position data and returns rotated values to simulate the rotation
	//
	// x'=x*cos(theta)-y*sin(theta)
	// y'=x*sin(theta)+y*cos(theta)
	// because the sensors are rotated clockwise the output should be rotated counter clockwise.
	// cos(-12.5) = 0.97629600712
	// sin(-12.5) = -0.21643961393
	//
	// int16_t X = (fcc.X*0.97629600712)-(fcc.Y*-0.21643961393);
	// int16_t Y = (fcc.X*-0.21643961393)+(fcc.Y*0.97629600712);

	// And now in integers because ATMEL
	#define ROT_COS 976
	#define ROT_SIN -216
	#define ROT_FACTOR 1000
	// Cast everything to 32 bit numbers to avoid overflow on high values (512*)
	static AxisStore InData;
	InData.X = AxisData->X;
	InData.Y = AxisData->Y;

	AxisData->X = (int32_t)((((int32_t)InData.X*ROT_COS)-((int32_t)InData.Y*ROT_SIN))/ROT_FACTOR);
	AxisData->Y = (int32_t)((((int32_t)InData.X*ROT_SIN)+((int32_t)InData.Y*ROT_COS))/ROT_FACTOR);
}

void MapStick(AxisStore* AxisData, bool isForceMapped) {
	AxisData->X = MapAxis(AxisData->X,false,isForceMapped);
	AxisData->Y = MapAxis(AxisData->Y,true,isForceMapped);
}


int16_t MapAxis(int16_t InValue, bool isPitch, bool isForceMapped) {
	static int16_t inMax;
	static int16_t inMin;
	static int16_t outMax;
	static int16_t outMin;

	// if in deadzone, just return 0
	if (abs(InValue) < STICK_DEADZONE) {
		return 0;
	}
	// if value is above center set all the values you can
	else if (InValue > 0) {
		outMax = StickAllowedMax;
		outMin = 0;
		inMin = 0;
		} else { // InValue < center - so set all the values you can
		outMax = 0;
		outMin = StickAllowedMin;
		inMax = 0;
	}
	// and now of the missing variables..
	if (isPitch) {
		if (isForceMapped) {
			// if ForceMapping is in use, use Center+Newton
			if (InValue > 0) {
				inMax = gStickLimits.N180;
				} else {
				inMin = -gStickLimits.N80;
			}
			} else {
			// if "plain" mapping, use recorded limits
			if (InValue > 0) {
				inMax = gStickLimits.Y.max;
				} else {
				inMin = gStickLimits.Y.min;
			}
		}
		// Now do the same for roll
		} else {
		if (isForceMapped) {
			// if forceMapped use Center+Newton for input range
			if (InValue > 0) {
				inMax = gStickLimits.N80;
				} else {
				inMin = -gStickLimits.N80;
			}
			} else {
			// for plain mapping use preset min/max for input range
			if (InValue > 0) {
				inMax = gStickLimits.X.max;
				} else {
				inMin = gStickLimits.X.min;
			}
		}
	}
	// Once all this logic is out of the way, return the output of the map function
	return mapLargeNumbers(InValue,inMin,inMax,outMin,outMax);
}

////////// Grip Stuff ///////////////

uint32_t ReadGrip(void) {
	uint32_t buffer;
	uint8_t SR1;
	uint8_t SR2;
	uint8_t SR3;

	//Read Grip
	//pull cs low
	PORTE &= ~(1 << PORTE6);
	// Poll Shift registers
	SR1 = Bit_Reverse(~(SPI_ReceiveByte()));
	SR2 = Bit_Reverse(~(SPI_ReceiveByte()));
	SR3 = Bit_Reverse(~(SPI_ReceiveByte()));

	//Bring CS back high
	PORTE |= (1 << PORTE6);

	// Reorder bytes to match windows buttons
	// 19 bits buttons first, 1 bit space, then 4 bits for HAT.
	if ((SR1 & Blank) != Blank) {
		buffer = (SR1 & Trigger1st) | ((SR1 >> 3) << 1) | ((uint16_t)(SR2 & TmsAll) << 2) | ((uint32_t)SR3 << 10) | ((uint32_t)(SR1 & CmsPush) << 17) | ((uint32_t)(MapHat(SR2 & TrimAll)) << 20);
		}else {
		buffer = 0;
	}
	return buffer;
}

uint8_t MapHat(uint8_t HatSwitch) {
	uint8_t RetVal = 8;
	switch (HatSwitch) {
		//case 0:  RetVal = 8; break; // uldr : center : hatButtons = B00000000
		case 1:  RetVal = 0; break; // Uldr : 0      : hatButtons = B00000001
		case 3:  RetVal = 1; break; // ULdr : 45     : hatButtons = B00000011
		case 2:  RetVal = 2; break; // uLdr : 90     : hatButtons = B00000010
		case 6:  RetVal = 3; break; // uLDr : 135    : hatButtons = B00000110
		case 4:  RetVal = 4; break; // ulDr : 180    : hatButtons = B00000100
		case 12: RetVal = 5; break; // ulDR : 225    : hatButtons = B00001100
		case 8:  RetVal = 6; break; // uldR : 270    : hatButtons = B00001000
		case 9:  RetVal = 7; break; // UldR : 315    : hatButtons = B00001001
	}
	return RetVal;


}

void FccSettings(uint32_t Buttons){
	// COnfig Options for the stick
	//Check if we need to go into config
	if (gIsConfig) { // if we are in config mode
	    gPowerLed -= 2; // will dim PC7 every round, to have a pulsing light to indicate config mode
	    setStatusLed(gPowerLed);

		//reset to defaults
		if (Buttons & (GripTriggerFirstDetent | GripTriggerSecondDetent)) {
			gOptions = Force4Kg;
			WipeStoredLimits();
			ChangeSensitivity(gOptions);
			exitConfig();
		}

		if (Buttons & GripDmsFwd) {
			//reset center
			ReadStickZero();
			exitConfig();
		}

		if (Buttons & GripDmsAft) {
			//Exit immediately
			exitConfig();
		}

		if (Buttons & GripTmsRight) {
			// Activate "Sensor rotation" will shift output as if sensore assembly is rotated 12.5 degrees clockwise
			gOptions |= RotatedSensors;
			exitConfig();
		}

		if (Buttons & GripTmsLeft) {
			// Shut down "Sensor rotation"
			gOptions &= ~(RotatedSensors);
			exitConfig();
		}

		if (Buttons & GripTmsFwd) {
			// Activate "Force Mapping". Max Pitch will now be considered as "180N" and FLCS 180N/80N rules will apply on range mapping
			gOptions |= ForceMap;
			CalcForceMapping();
			exitConfig();
		}

		if (Buttons & GripTmsAft) {
			// Turn off "Force Mapping"
			gOptions &= ~(ForceMap);
			exitConfig();
		}

		// if (Buttons & GripPinkie) {
		// 	// Set MINS and MAX
		// 	ReadMinMax();
		// }

		if (Buttons & GripCmsAft) {
			// Set user sensitivity
			// ChangeSensitivity(ForceUserDefined);
			// 3kg/f
			ChangeSensitivity(ForceUserDefined);
			exitConfig();
		}
		// Force Calibrations
		if (Buttons & GripCmsLeft) {
			// 9kgf
			ChangeSensitivity(Force9Kg);
			exitConfig();
		}

		if (Buttons & GripCmsFwd) {
			// 6kgf
			ChangeSensitivity(Force6Kg);
			exitConfig();
		}

		if (Buttons & GripCmsRight) {
			// 4.5kgf
			ChangeSensitivity(Force4Kg);
			exitConfig();
		}
		// Check for config timeout
		if (millis() - gConfigTimer >= 15000) {
			exitConfig();
		}
	} else { // if we are not in config mode
		if (gConfigTimer == 0) {
		  if (Buttons & (GripMissleStep | GripPaddle)) {
		    gConfigTimer = millis();
		  }
		} else if (millis() - gConfigTimer >= 1500) {
			  gIsConfig = true;
			  gConfigTimer = millis();
		} else {
		  if (!(Buttons & (GripMissleStep | GripPaddle))) {
		    gConfigTimer = 0;
		  }
		}
	}
}

void exitConfig(void) {
	WriteMem(); // Save all the changes to EEPROM
	gIsConfig = false;
	gConfigTimer = 0;
	gPowerLed = 255;
	setStatusLed(gPowerLed);
}

void processStickOut(uint8_t inOptions, int16_t inUserForce)
{
  bool isReboot = (inOptions & RebootDevice)? true:false;
	bool isCenter = inOptions & CenterStick? true:false;

	inOptions &= ~(CenterStick | RebootDevice); //make sure RebootFlag doesn't carry over)

  if (isReboot) {
		// then reboot.. no need to do anything as ram will be wiped.
	  RebootToBootloader();
  }

	if (isCenter) {
		// if centering, then do just that, don't try and change config
		ReadStickZero();
	} else {
			gOptions = inOptions & ~(RebootDevice | CenterStick); //really really make sure that isn't saved
		  if (gUserDefinedForce != inUserForce) {
		  	gUserDefinedForce = inUserForce;
		  }
		  ChangeSensitivity(gOptions);
		  exitConfig();
	}
}
