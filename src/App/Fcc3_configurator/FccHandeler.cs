using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HidSharp;
using HexHelper;

namespace Fcc3_configurator
{
    public class FccHandeler
    {
        public const decimal KgInLb = 2.20462262M;

        private ushort VID;
        private ushort PID;
        private volatile HidDevice device;


        private Int16 CurrentCustomForce;
        private Int16 RequestedCustomeForce;

        // FCC model true is new
        private bool CurrentFccGain;
        private bool RequestedFccGain;


        private ConfigOptions RunTimeOptions;
        private ConfigOptions RequestedOptions;

        #region properties
        public bool isConnected
        {
            get
            {
                if (device != null)
                {
                    return true;
                }
                    return false;
            }
        }
        public string FirmwareVersion
        {
            get
            {
                if (isConnected)
                {
                    return BdcDecode(device.ProductVersion);
                    //return BdcDecode(device.Attributes.Version);
                }
                else
                {
                    return null;
                }
            }
        }

        public bool isForceMapped
        {
            get
            {
                if ((RunTimeOptions & ConfigOptions.ForceMap) != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                };
            }
            set
            {
                if (value)
                {
                    RequestedOptions |= ConfigOptions.ForceMap;
                }
                else
                {
                    RequestedOptions &= ~(ConfigOptions.ForceMap);
                }
            }
        }
        public bool isSensorRotated
        {
            get
            {
                if ((RunTimeOptions & ConfigOptions.RotatedSensors) != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                };
            }
            set
            {
                if (value)
                {
                    RequestedOptions |= ConfigOptions.RotatedSensors;
                }
                else
                {
                    RequestedOptions &= ~(ConfigOptions.RotatedSensors);
                }
            }
        }
        public bool Use4KgForce
        {
            get
            {
                if ((RunTimeOptions & ConfigOptions.Force4Kg) != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                };
            }
            set
            {
                if (value)
                {
                    RequestedOptions &= ~(ConfigOptions.ForceAll);
                    RequestedOptions |= ConfigOptions.Force4Kg;
                }
            }
        }
        public bool Use6KgForce
        {
            get
            {
                if ((RunTimeOptions & ConfigOptions.Force6Kg) != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                };
            }
            set
            {
                if (value)
                {
                    RequestedOptions &= ~(ConfigOptions.ForceAll);
                    RequestedOptions |= ConfigOptions.Force6Kg;
                }
            }
        }
        public bool Use9KgForce
        {
            get
            {
                if ((RunTimeOptions & ConfigOptions.Force9Kg) != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                };
            }
            set
            {
                if (value)
                {
                    RequestedOptions &= ~(ConfigOptions.ForceAll);
                    RequestedOptions |= ConfigOptions.Force9Kg;
                }
            }
        }
        public bool UseCustomForce
        {
            get
            {
                if ((RunTimeOptions & ConfigOptions.ForceUserDefined) != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                };
            }
            set
            {
                if (value)
                {
                    RequestedOptions &= ~(ConfigOptions.ForceAll);
                    RequestedOptions |= ConfigOptions.ForceUserDefined;
                }
            }
        }

        public bool isUseNewFccGain
        {
          get {
              return CurrentFccGain;
          }
          set{
            RequestedFccGain = value;
          }
        }
        #endregion
        [Flags]
        public enum ConfigOptions
        {
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

        public FccHandeler(ushort _VID = 0x1029, ushort _PID = 0xF16C)
        {
            VID = _VID;
            PID = _PID;
        }



        public bool Connect()
        {
            var loader = new HidDeviceLoader();
            if (loader.GetDevices(VID, PID).Count() != 1)
            {
                return false;
            }
            try
            {
                HidDevice tempDevice = new HidDeviceLoader().GetDevices(VID, PID).First();
                device = tempDevice;
                return isConnected;
            }
            catch
            {
                return false;
            }
        }

        public bool Disconnect()
        {
            device = null;
            return isConnected;

        }
        public bool Update()
        {

            if (isConnected)
            {
                HidStream stream;
                byte[] buff = new byte[device.MaxInputReportLength];
                if (device.TryOpen(out stream))
                {
                    stream.Read(buff);
                    RunTimeOptions = (ConfigOptions)buff[7];
                    CurrentCustomForce = (Int16)((buff[9] << 8) | buff[8]);
                    short test = (short)(CurrentCustomForce & 0x8000);
                    if ((CurrentCustomForce & 0x8000) != 0) {
                      CurrentFccGain = true;
                        unchecked {
                            CurrentCustomForce &= ~((Int16)0x8000);
                        }
                    } else {
                      CurrentFccGain = false;
                    }
                    return true;
                }
                device = null;
            }
            return false;
        }

        private bool SendToStick(byte options, Int16 forceSetting)
        {
            byte upper = (byte)(forceSetting >> 8);
            byte lower = (byte)(forceSetting & 0xff);

            try
            {
                if (isConnected)
                {
                    byte[] OutData = new byte[device.MaxOutputReportLength];
                    OutData[0] = 0; // Report ID
                    OutData[1] = options;
                    OutData[2] = lower;
                    OutData[3] = upper;
                    HidStream stream;
                    device.TryOpen(out stream);
                    stream.Write(OutData);
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        public void Center()
        {
            SendToStick((byte)ConfigOptions.CenterStick, 0);
        }

        public void Reboot()
        {
            SendToStick((byte)ConfigOptions.RebootDevice, 0);
        }

        public bool ApplyChanges()
        {
            if (isConnected)
            {
                return SendToStick((byte)RequestedOptions, RequestedCustomeForce);
            }
            return false;
        }

        public void SetCustomForce(decimal RequestedForce, bool UseKg = true)
        {
            // Hardware: 9kg/f = 2Volt Delta
            // ADC: 5V = 4096
            // Middle Voltage: 2.5V

            float DeltaVol = 1.5F;
            if (RequestedFccGain)
            {
                DeltaVol = 2.0F;
            }
            //const float DeltaVol = 2.0F;
            const float ForceRef = 9.0F;
            const float AdcMax = 4095.0F;
            const float Vref = 5.0F;
            const float BaseVoltage = 2.5F;

            if (!UseKg)
            {
                RequestedForce /= KgInLb;
            }
            float voltage = ((DeltaVol / ForceRef) * (float)RequestedForce) + BaseVoltage;

            float Retval = ((AdcMax / Vref) * voltage) - (AdcMax / 2);

            RequestedCustomeForce = (Int16)Retval;
            if (RequestedFccGain) {
              RequestedCustomeForce = (short)(RequestedCustomeForce | 0x8000);
            }

        }

        public decimal GetCurrentForce(bool UseKg = true)
        {
            // Hardware: 9kg/f = 2Volt Delta
            // ADC: 5V = 4096
            // Middle Voltage: 2.5V


            float DeltaVol = 1.5F;
            if (isUseNewFccGain) {
              DeltaVol = 2.0F;
            }
            const float ForceRef = 9.0F;
            const float AdcMax = 4095.0F;
            const float Vref = 5.0F;
            const float BaseVoltage = 2.5F;
            float Retval = (((CurrentCustomForce + (AdcMax / 2)) * (Vref / AdcMax)) - BaseVoltage) * (ForceRef / DeltaVol);
            if (!UseKg)
            {
                Retval *= (float)KgInLb;
            }
            return (decimal)Math.Round(Retval, 1);
        }




        private string BdcDecode(int VersionBin)
        {
            //#define VERSION_BCD(Major, Minor, Revision) \
            //            CPU_TO_LE16(((Major & 0xFF) << 8) | \
            //                         ((Minor & 0x0F) << 4) | \
            //                         (Revision & 0x0F))
            //byte inMajor = 4;
            //byte inMinor = 2;
            //byte inRevision = 14;
            //VersionBin = (((inMajor & 0xFF) << 8) | ((inMinor & 0x0F) << 4) | (inRevision & 0x0F));
            //Console.WriteLine(VersionBin);

            byte lower = (byte)(VersionBin & 0xFF);

            byte Major = (byte)(VersionBin >> 8);
            byte Minor = (byte)(lower >> 4);
            byte Revision = (byte)(lower & 0x0F);


            string RetVal = Major + "." + Minor + "." + Revision;
            return RetVal;
        }


        public bool UpgradeFirmware(string HexPath)
        {
            bool isSuccess = false;

            //List initial ports
            HexUpdater Uploader = new HexUpdater();

            string[] AvailablePorts = Uploader.AvailablePorts;

            // Trigger reboot for COM port to appear
            if (isConnected)
            {
                SendToStick((byte)ConfigOptions.RebootDevice, 0);
            }
            device = null;


            string ProgPort = Uploader.AutoDetectNewPort(AvailablePorts);
            if (ProgPort != "")
            {
                isSuccess = Uploader.uploadHex(ProgPort, HexPath);
            }
            return isSuccess;
        }
    }
}
