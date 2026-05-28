using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.App.Auth;
using ActivityAssistent.App.Interfaces.Ai;
using ActivityAssistent.Shared.Dtos.Ai;

namespace ActivityAssistent.App.Services
{
    public class MauiAiService(HttpClient Http, CustomAuthenticationStateProvider authStateProvider) : BaseService(Http, authStateProvider), IAiMeetingAnalyzer
    {
        public Task<MeetingAnalysisResultDto> AnalyzeMeetingAsync(AudioProcessingRequestDto RequestPayload, CancellationToken Token)
        {
            throw new NotImplementedException();
        }
    }
}
