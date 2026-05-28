using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.App.Auth;
using ActivityAssistent.App.Interfaces.Audio;

namespace ActivityAssistent.App.Services
{
    public class MauiAudioService(HttpClient Http, CustomAuthenticationStateProvider authStateProvider) : BaseService(Http, authStateProvider), IAudioRecorderService
    {
        public bool IsRecording => throw new NotImplementedException();

        public Task StartRecordingAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> StopRecordingAsync()
        {
            throw new NotImplementedException();
        }
    }
}
