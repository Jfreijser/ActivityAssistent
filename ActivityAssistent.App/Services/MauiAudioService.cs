using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ActivityAssistent.App.Auth;
using ActivityAssistent.App.Interfaces.Audio;
using Plugin.Maui.Audio;


namespace ActivityAssistent.App.Services
{
    // 1. De Primary Constructor: We vragen hier om IAudioManager (van de plugin!), niet je eigen interface.
    public class MauiAudioService(
        HttpClient Http,
        CustomAuthenticationStateProvider AuthStateProvider,
        IAudioManager AudioManager)
        : BaseService(Http, AuthStateProvider), IAudioRecorderService
    {
        // 2. We maken een veld van het type IAudioRecorder (van de plugin) en initialiseren het direct
        // met de AudioManager die we via de constructor-haakjes binnenkrijgen.
        private readonly IAudioRecorder _audioRecorder = AudioManager.CreateRecorder();

        public bool IsRecording => _audioRecorder.IsRecording;

        public async Task StartRecordingAsync()
        {
            await _audioRecorder.StartAsync();
        }

        public async Task<Stream> StopRecordingAsync()
        {
            var RecordedAudio = await _audioRecorder.StopAsync();
            return RecordedAudio.GetAudioStream();
        }
    }
}