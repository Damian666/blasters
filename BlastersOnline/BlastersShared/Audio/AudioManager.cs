using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Wave;

namespace BlastersShared.Audio
{
    /// <summary>
    /// Manages playback of sound effects and audio
    /// </summary>
    public class AudioManager
    {
        //A basic playback device which allows the playback of audio
        private IWavePlayer _waveOutDevice = new WaveOut();

        public AudioManager()
        {

        }

        public void Play(string musicFileName)
        {
            WaveStream stream =
            new Mp3FileReader(musicFileName);

            stream = new LoopStream(stream);

            _waveOutDevice.Volume = 0.25f;
            _waveOutDevice.Stop();
            _waveOutDevice.Init(stream);
            _waveOutDevice.Play();
        }

        public void Stop()
        {
            _waveOutDevice.Stop();
        }

    }
}
