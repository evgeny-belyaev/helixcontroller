using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelixController
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputDevideName = "TSMIDI2.0";
            var outputDeviceName = "loopMIDI";

            int outputDeviceId = GetOutDevice(outputDeviceName);
            if (outputDeviceId == -1)
            {
                Console.WriteLine(outputDeviceName + " not found!");
                return;
            }
            var midiOut = new MidiOut(outputDeviceId);
            Console.WriteLine(outputDeviceName + " connected successfully");


            int inputDeviceId = GetInDevice(inputDevideName);
            if (inputDeviceId == -1)
            {
                Console.WriteLine(inputDevideName + " not found!");
                return;
            }
            var midiIn = new MidiIn(inputDeviceId);

            Console.WriteLine(inputDevideName + " connected successfully");

            var converter = new Converter(midiIn, midiOut);
            converter.start();
            
            Console.ReadKey();
        }


        static int GetOutDevice(string deviceName)
        {
            for (int device = 0; device < MidiOut.NumberOfDevices; device++)
            {
                var deviceInfo = MidiOut.DeviceInfo(device);
                var productName = deviceInfo.ProductName;

                if (productName.IndexOf(deviceName, StringComparison.Ordinal) != -1)
                {
                    return device;
                }
            }

            return -1;
        }

        static int GetInDevice(string deviceName)
        {
            for (int device = 0; device < MidiIn.NumberOfDevices; device++)
            {
                var deviceInfo = MidiIn.DeviceInfo(device);
                var productName = deviceInfo.ProductName;

                if (productName.IndexOf(deviceName, StringComparison.Ordinal) != -1)
                {
                    return device;
                }
            }

            return -1;
        }
    }
}
