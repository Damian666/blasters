using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using csvorbis;
using NAudio.Wave;

namespace BlastersShared.Audio
{
    /// <summary>
    /// Class for reading from Ogg files
    /// </summary>
    public class OggFileReader : WaveStream
    {
        private WaveFormat waveFormat;

        private object repositionLock = new object();

        private VorbisFile m_vorbisFile;

        /// <summary>Supports opening an Ogg Vorbis file</summary>
        public OggFileReader(string oggFileName) 
        {
            m_vorbisFile = new VorbisFile(oggFileName);
            Info[] info = m_vorbisFile.getInfo();
            // TODO: 8 is hard coded!! need to change it dynamically by reading tags
            waveFormat = new WaveFormat(info[0].rate, 8, info[0].channels);

            float t = m_vorbisFile.time_total(-1); // Timespan in seconds
        }


        /// <summary>
        /// This is the length in bytes of data available to be read out from the Read method
        /// (i.e. the decompressed Ogg length)
        /// n.b. this may return 0 for files whose length is unknown
        /// </summary>
        public override long Length
        {
            get
            {
                long length = m_vorbisFile.pcm_total(-1) * m_vorbisFile.getInfo(0).channels * 1 /* 16 bit TODO!*/;
                return length;
            }
        }

        /// <summary>
        /// <see cref="WaveStream.WaveFormat"/>
        /// </summary>
        public override WaveFormat WaveFormat
        {
            get { return waveFormat; }
        }

        /// <summary>
        /// <see cref="Stream.Position"/>
        /// </summary>
        public override long Position
        {
            get
            {
                return m_vorbisFile.pcm_tell();
            }
            set
            {
                lock (repositionLock)
                {
                    m_vorbisFile.pcm_seek(value);
                }
            }
        }

        /// <summary>
        /// Reads decompressed PCM data from our MP3 file.
        /// </summary>
        public override int Read(byte[] sampleBuffer, int offset, int numBytes)
        {
            int bytesRead = 0;
            lock (repositionLock)
            {
                bytesRead = m_vorbisFile.read(sampleBuffer, numBytes, _BIGENDIANREADMODE, _WORDREADMODE, _SGNEDREADMODE, null);

            }

            return bytesRead;
        }

        /// <summary>
        /// Disposes this WaveStream
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_vorbisFile != null)
                {
                    m_vorbisFile.Dispose();
                    m_vorbisFile = null;
                }
            }
            base.Dispose(disposing);
        }

        private const int _BIGENDIANREADMODE = 0;		// Big Endian config for read operation: 0=LSB;1=MSB
        private const int _WORDREADMODE = 1;			// Word config for read operation: 1=Byte;2=16-bit Short
        private const int _SGNEDREADMODE = 0;			// Signed/Unsigned indicator for read operation: 0=Unsigned;1=Signed

    }
}
