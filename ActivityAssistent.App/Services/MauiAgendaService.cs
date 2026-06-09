using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.App.Auth;
using ActivityAssistent.App.Interfaces.Agenda;
using ActivityAssistent.Shared.Dtos.ActionPoints;
using ActivityAssistent.Shared.Dtos.Agenda;
using ActivityAssistent.Shared.Dtos.Response;

namespace ActivityAssistent.App.Services
{
    public class MauiAgendaService(HttpClient Http, CustomAuthenticationStateProvider authStateProvider) : BaseService(Http, authStateProvider), IAgenda
    {
        public async Task<List<AgendaDto>> GetSchedulerPointsAsync(CancellationToken Token)
        {
			try
			{
				var result = await GetAsync<ApiResponse<List<AgendaDto>>>("api/agenda/GetAgendaPoints");
				if(result.IsSuccess)
					return result.Data;

				return new List<AgendaDto>();
            }
			catch (Exception ex)
			{

                Console.WriteLine($"[API Error] Getting the agenda points failed: {ex.Message}");
                return new List<AgendaDto>();
            }
        }
    }
}
