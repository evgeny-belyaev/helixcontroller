using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HelixController
{
    class Converter
    {
        private MidiIn midiIn;
        private MidiOut midiOut;

        private int currentPreset = 0;
        private byte[] snapshot1 = new byte[] { 0xBC, 0x45, 0x00 };
        private byte[] snapshot2 = new byte[] { 0xBC, 0x45, 0x01 };
        private byte[] snapshot3 = new byte[] { 0xBC, 0x45, 0x02 };
        private byte[] snapshot4 = new byte[] { 0xBC, 0x45, 0x03 };


        public Converter(MidiIn midiIn, MidiOut midiOut)
        {
            this.midiIn = midiIn;
            this.midiOut = midiOut;
        }
        public void start()
        {
            midiIn.MessageReceived += midiIn_MessageReceived;
            midiIn.Start();
        }

        void midiIn_MessageReceived(object sender, MidiInMessageEventArgs e)
        {
            var number = (e.RawMessage - 0xC0) / 256;
            var snapshotNumber = number % 4;
            byte presetNumber = (byte)(number / 4);
            var presetChanged = presetNumber != currentPreset;

            if (presetChanged)
            {
                currentPreset = presetNumber;
                midiOut.SendBuffer(new byte[] { 0xCC, presetNumber });
                Thread.Sleep(50);
            }

            switch (snapshotNumber)
            {
                case 0:
                    {
                        midiOut.SendBuffer(snapshot1);
                        break;
                    }
                case 1:
                    {
                        midiOut.SendBuffer(snapshot2);
                        break;
                    }
                case 2:
                    {
                        midiOut.SendBuffer(snapshot3);
                        break;
                    }
                case 3:
                    {
                        midiOut.SendBuffer(snapshot4);
                        break;
                    }

            }

            Console.WriteLine($"Preset: {presetNumber}, Snapshot: {snapshotNumber}");
        }
    }
}
