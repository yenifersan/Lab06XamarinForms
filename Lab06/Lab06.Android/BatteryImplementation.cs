using Android.App;
using Android.OS;
using Lab06.Droid;
using Android.Content;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(BatteryImplementation))]
namespace Lab06.Droid
{
    public class BatteryImplementation : IBattery
    {
        public BatteryImplementation()
        {
        }

        public int RemainingChargePercent
        {
            get
            {
                try
                {
                    using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                    {
                        using (var battery = Application.Context.RegisterReceiver(null, filter))
                        {
                            var level = battery.GetIntExtra(BatteryManager.ExtraLevel, -1);
                            var scale = battery.GetIntExtra(BatteryManager.ExtraScale, -1);

                            return (int)Math.Floor(level * 100D / scale);
                        }
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Ensure you have android.permission.BATTERY_STATS");
                    throw;
                }
            }
        }

        public Lab06.BatteryStatus Status
        {
            get
            {
                try
                {
                    using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                    {
                        using (var battery = Application.Context.RegisterReceiver(null, filter))
                        {
                            int status = battery.GetIntExtra(BatteryManager.ExtraStatus, -1);
                            var isCharging = status == (int)BatteryStatus.Charging || status == (int)BatteryStatus.Full;

                            var chargePlug = battery.GetIntExtra(BatteryManager.ExtraPlugged, -1);
                            var usbCharge = chargePlug == (int)BatteryPlugged.Usb;
                            var acCharge = chargePlug == (int)BatteryPlugged.Ac;
                            bool wirelessCharge = false;
                            wirelessCharge = chargePlug == (int)BatteryPlugged.Wireless;

                            isCharging = (usbCharge || acCharge || wirelessCharge);
                            if (isCharging)
                                return Lab06.BatteryStatus.Charging;

                            switch (status)
                            {
                                case (int)BatteryStatus.Charging:
                                    return Lab06.BatteryStatus.Charging;
                                case (int)BatteryStatus.Discharging:
                                    return Lab06.BatteryStatus.Discharging;
                                case (int)BatteryStatus.Full:
                                    return Lab06.BatteryStatus.Full;
                                case (int)BatteryStatus.NotCharging:
                                    return Lab06.BatteryStatus.NotCharging;
                                default:
                                    return Lab06.BatteryStatus.Unknown;
                            }
                        }
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Ensure you have android.permission.BATTERY_STATS");
                    throw;
                }
            }
        }

        public PowerSource PowerSource
        {
            get
            {
                try
                {
                    using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                    {
                        using (var battery = Application.Context.RegisterReceiver(null, filter))
                        {
                            int status = battery.GetIntExtra(BatteryManager.ExtraStatus, -1);
                            var isCharging = status == (int)BatteryStatus.Charging || status == (int)BatteryStatus.Full;

                            var chargePlug = battery.GetIntExtra(BatteryManager.ExtraPlugged, -1);
                            var usbCharge = chargePlug == (int)BatteryPlugged.Usb;
                            var acCharge = chargePlug == (int)BatteryPlugged.Ac;

                            bool wirelessCharge = false;
                            wirelessCharge = chargePlug == (int)BatteryPlugged.Wireless;

                            isCharging = (usbCharge || acCharge || wirelessCharge);

                            if (!isCharging)
                                return Lab06.PowerSource.Battery;
                            else if (usbCharge)
                                return Lab06.PowerSource.Usb;
                            else if (acCharge)
                                return Lab06.PowerSource.Ac;
                            else if (wirelessCharge)
                                return Lab06.PowerSource.Wireless;
                            else
                                return Lab06.PowerSource.Other;
                        }
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Ensure you have android.permission.BATTERY_STATS");
                    throw;
                }
            }
        }
    }
}
