using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityAssistent.WebV2.Client.Interfaces.Audio
{
    public  interface IAudioRecorderService
    {
        bool IsRecording { get; }

        Task StartRecordingAsync();

        Task<string> StopRecordingAsync();
    }
}
