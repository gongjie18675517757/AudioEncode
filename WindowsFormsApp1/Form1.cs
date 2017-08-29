using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using NSpeex;
using System.Runtime.InteropServices;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        private WaveIn recorder;
        private BufferedWaveProvider bufferedWaveProvider;
        private WaveOut player;
        private void button1_Click(object sender, EventArgs e)
        {
            recorder = new WaveIn();
            recorder.WaveFormat = new WaveFormat(8000, 16, 1);
            recorder.BufferMilliseconds = 40;
            recorder.DataAvailable += WaveIn_DataAvailable;



            // set up our signal chain
            bufferedWaveProvider = new BufferedWaveProvider(recorder.WaveFormat);

            //bufferedWaveProvider.DiscardOnBufferOverflow = true;
            // set up playback
            player = new WaveOut();
            player.Init(bufferedWaveProvider);

            // begin playback & record
            player.Play();
            recorder.StartRecording();
        }
        SpeexEncoder encoder = new SpeexEncoder(BandMode.Wide);
        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            // convert to short
            short[] data = new short[e.BytesRecorded / 2];
            Buffer.BlockCopy(e.Buffer, 0, data, 0, e.BytesRecorded);
            var encodedData = new byte[e.BytesRecorded];
            // note: the number of samples per frame must be a multiple of encoder.FrameSize
            var encodedBytes = encoder.Encode(data, 0, data.Length, encodedData, 0, encodedData.Length);
            if (encodedBytes != 0)
            {
                // todo: do something with the encoded data
            }

            var sendData = new byte[encodedBytes];
            Buffer.BlockCopy(encodedData, 0, sendData, 0, encodedBytes);


            Decode(sendData);

            Console.WriteLine($"{e.BytesRecorded} => {encodedBytes}  {DateTime.Now}");
            //bufferedWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded); 
        }

        SpeexDecoder decoder = new SpeexDecoder(BandMode.Wide);


        private void Decode(byte[] encodedData)
        { 
            var data = new byte[640];
            Buffer.BlockCopy(encodedData, 0, data, 0, encodedData.Length);
            short[] decodedFrame = new short[320]; // should be the same number of samples as on the capturing side
            decoder.Decode(encodedData, 0, encodedData.Length, decodedFrame, 0, false);
            var playData = new byte[640];
            Buffer.BlockCopy(decodedFrame, 0, playData, 0, 640);
            bufferedWaveProvider.AddSamples(playData, 0, playData.Length);
        }

    }




}
