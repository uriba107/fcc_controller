## FCC3 controller board
This Arduino Based project, is meant to allow FCC3 force sensing mod for Cougar, to be used with Thrustmaster Warthog without the need to use the old TM Cougar Electronics

This software is distributed under MIT license, and is provided as is and without any warranty what so ever. Further information can be found in "license.txt" which belongs to the LUFA framework on which controller code is based.


### Table Of Content
- [FCC3 controller board](#fcc3-controller-board)
  - [Features](#features)
  - [What settings are recommended](#what-settings-are-recommended)
  - [FLCS in F-16](#flcs-in-f-16)
      - [Analog FLCS](#analog-flcs)
      - [Digital FLCS](#digital-flcs)
  - [Cheat sheet](#cheat-sheet)
  - [BOM](#bom)


### Features
  * 11 bit output on Pitch and Roll.
  * Support for both Cougar and Warthog grips.
  * On the fly configuration changes:
    * Multiple force presets (9, 6 & 4.5 Kg/f), in addition to one user configurable sensitivity setting.
    * User toggleable, FLCS Based, realistic force ratios.  
    * User toggleable FLCS sensor rotation. In later blocks, the stick sensor is rotated clockwise to reduce accidental roll on pitch commands. Activating this option will simulate that.
    * on the fly re-centering (just in case).
  * Bi-Directional Control software:
    * Allows Joystick settings change (same as available via the grip).
    * Allows User configurable sensitivity setting to be set.
    * Handles Firmware upgrades for the stick.

### What settings are recommended
  FLCS and SSC config had changed over the years, so depending on what type of F-16 you want to model, settings will vary.

<b><u><i> Blocks 30/32 and before: </b></u></i>
  * Analog FLCS
  * Sensor Rotation -  Disabled

<b><u><i> Block 40/42 and after: </b></u></i>
  * Digital FLCS
  * Sensor Rotation - Enabled

### FLCS in F-16
There are two types of FLCS installed in F-16s.The controller attempts to emulate both system operational logic.</br>

In both cases, force ratio is maintained by the controller. The selected force setting is used as full pitch up command (which is the maximum required force), while pitch down and roll are proportional according to the FLCS version emulated.

#### Analog FLCS
  <b>Real life:</b><br>
   Full pitch up command is 180N (about 18Kgf), while full pitch down and full roll are 80N (or about 8Kgf).

  <b>Controller example:</b> in 9Kg/f mode, full pitch up will be 9kg/f for full deflection, but on the other axis, only 4kg/f will be required.

#### Digital FLCS
  <b>Real life:</b><br>
  full pitch up command is 25lbf, while full pitch down is 16lbf and full roll is 17lbf.

  <b>Controller example:</b><br>
  In 9kgf mode, Pitch up command will be  9kgf for full deflection. full pitch down will require 5.76kgf and full roll will require 6.12kgf.

### Cheat sheet
![Image of fcc_controller button layout](https://github.com/uriba107/fcc_controller/raw/master/misc/FCC_controller_cheatsheet.png)


### BOM
* Atmega 32U4 based board ([Arduino pro-micro](http://www.ebay.com/sch/i.html?_from=R40&_trksid=p2050601.m570.l1313.TR0.TRC0.H0.Xarduino+pro-micro.TRS0&_nkw=arduino+pro-micro&_sacat=0) or an [Arduino Micro](https://www.arduino.cc/en/Main/arduinoBoardMicro) are recommended)
* [MCP3202](http://www.ebay.com/sch/i.html?_odkw=arduino+pro-micro&_osacat=0&_from=R40&_trksid=p2045573.m570.l1313.TR0.TRC0.H0.XMCP3202.TRS0&_nkw=MCP3202&_sacat=0) - 12 Bit external ADC
* A small protoboard (cheap dual sided 4*6 will be good enough)
* For Pro-micro - A momentary push button and 10+ Kohm resistor as the board has no reset switch.
* A few LEDs and resistors - for status.
* Header pins, two 3 pin headers and one 5 pins header, for grip and analog sensors. Generic 0.1" headers will do the trick. But correct header will be the [ JST EH series connectors](http://www.jst-belgium.be/productSeries.php?pid=46).
* Female Headers for the Arduino (optional)
* DIP8 socket for the MCP3202 (optional)
