using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using NSpeex;
namespace ConsoleApp1
{
    class Program
    {
        private static WaveCallbackInfo callbackinfo;

        static void Main(string[] args)
        {
            //NSpeex.SpeexEncoder encoder = new SpeexEncoder(BandMode.Wide);

            NAudio.Wave.WaveIn waveIn = new NAudio.Wave.WaveIn() { WaveFormat=new WaveFormat(8000,1)};

            waveIn.DataAvailable += WaveIn_DataAvailable;
            waveIn.StartRecording();
            IWavePlayer waveOut = new WaveOut();

            Console.ReadLine();
        }

        private static void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            Console.WriteLine(e.BytesRecorded);
        }
    }


}
