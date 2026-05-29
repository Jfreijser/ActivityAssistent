using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityAssistent.App.Interfaces.Audio
{
    public  interface IAudioRecorderService
    {
        Task StartRecordingAsync();
        Task<Stream> StopRecordingAsync();
        bool IsRecording { get; }
    }
}
