#ifndef fcc3_h
#define fcc3_h

//#define FAKE_WARTHOG

#pragma once

#include <stdlib.h>
#include <avr/io.h>
#include <avr/eeprom.h>
#include <avr/interrupt.h>
//#include <LUFA/Drivers/Peripheral/ADC.h>
#include <LUFA/Drivers/Peripheral/SPI.h>


#include "fcc3_helpers.h"

#define ROLL_CH 0
#define PITCH_CH 1

#define ReadPitch readSPIADC(PITCH_CH)
#define ReadRoll readSPIADC(ROLL_CH)
#define StickAllowedMax 1023
#define StickAllowedMin -1024

// Structs
typedef struct  {
  int16_t center:12;
  int16_t min:12;
  int16_t max:12;
} AxisParams;

typedef struct  {
  AxisParams X;
  AxisParams Y;
  int16_t N180:12;
  int16_t N80:12;
} StickLimit;

typedef struct  {
    int16_t X:12;
    int16_t Y:12;
} AxisStore;

enum ConfigOptions {
    RotatedSensors = 0x01,
    ForceMap = 0x02,
    CenterStick = 0x04,
    RebootDevice = 0x08,
    Force4Kg = 0x10,
    Force6Kg = 0x20,
    Force9Kg = 0x40,
    ForceUserDefined = 0x80,
    ForceAll = 0xF0,
};


  #define GripTriggerFirstDetent 0x01
  #define GripPickle 0x02
  #define GripPinkie 0x04
  #define GripPaddle 0x08
  #define GripMissleStep 0x10
  #define GripTriggerSecondDetent 0x20
  #define GripTmsFwd 0x40
  #define GripTmsRight 0x80
  #define GripTmsAft 0x100
  #define GripTmsLeft 0x200
  #define GripDmsFwd 0x400
  #define GripDmsRight 0x800
  #define GripDmsAft 0x1000
  #define GripDmsLeft 0x2000
  #define GripCmsFwd 0x4000
  #define GripCmsRight 0x8000
  #define GripCmsAft 0x10000
  #define GripCmsLeft 0x20000
  #define GripCmsDown 0x40000
  #define GripBlank 0x80000
  #define GripTrimFwd 0x100000
  #define GripTrimRight 0x200000
  #define GripTrimAft 0x400000
  #define GripTrimLeft 0x800000

  // Mapping from Grip to proper bytes
  //Byte 1:
  #define Trigger1st 0x01
  #define CmsPush 0x02
  #define Blank 0x04
  #define WpnRel 0x08
  #define Pinky 0x10
  #define Paddle 0x20
  #define MslStp 0x40
  #define Trigger2nd 0x80
  // Byte 2:
  #define TrimAll 0x0F
  #define TmsAll 0xF0

  #define TrimFwd 0x01
  #define TrimRight 0x02
  #define TrimAft 0x04
  #define TrimLeft 0x08
  #define TmsFwd 0x10
  #define TmsRight 0x20
  #define TmsAft 0x40
  #define TmsLeft 0x80
  // Byte 3:
  #define DmsAll 0x0F
  #define CmsAll 0xF0

  #define DmsFwd 0x01
  #define DmsRight 0x02
  #define DmsAft 0x04
  #define DmsLeft 0x08
  #define CmsFwd 0x10
  #define CmsRight 0x20
  #define CmsAft 0x40
  #define CmsLeft 0x80

// configure Global EEPROM pointers
//extern uint16_t EEMEM NonVolatilePitchMin;
//extern uint16_t EEMEM NonVolatilePitchMax;
//extern uint16_t EEMEM NonVolatileRollMin;
//extern uint16_t EEMEM NonVolatileRollMax;
extern uint16_t EEMEM NonVolatileUserForce;

extern uint8_t EEMEM NonVolatileOptions;

// Global vars to store EEPROM values for runtime
extern StickLimit gStickLimits;
extern uint8_t gOptions;
extern int16_t gUserDefinedForce;

extern bool gIsConfig;
extern uint32_t gConfigTimer;
extern uint8_t gPowerLed;
// Global var to store history for smoothing

//Function decelerations
// Hardware
void SetupSPI(void);
void SetupLeds(void);
void setStatusLed(uint8_t power);
int16_t readSPIADC(uint8_t adcChannel);

// Generic stuff
void InitFCC(void);

//EEPROM
void WriteMem(void);
void ReadMem(void);

// Stick
void ReadStickZero(void);
void ChangeSensitivity(uint8_t sensitivity);
void SetCalibratedSensitivity(int16_t OffsetValue);
void CalcForceMapping (void);
//void LoadUserSensitivity();
void WipeStoredLimits(void);
//void ReadMinMax(void);


void ReadStick(AxisStore* AxisData);
void RotateFlcs(AxisStore* AxisData);
int16_t MapAxis(int16_t InValue, bool isPitch, bool isForceMapped);
void MapStick(AxisStore* AxisData, bool isForceMapped);

// Grip
uint32_t ReadGrip(void);
uint8_t MapHat(uint8_t HatSwitch);

// Config
void FccSettings(uint32_t Buttons);
void exitConfig(void);
void processStickOut(uint8_t inOptions, int16_t inUserForce);
#endif
