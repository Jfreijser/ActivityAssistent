using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.Shared.Dtos.ActionPoints;
using ActivityAssistent.Shared.Dtos.Agenda;

namespace ActivityAssistent.App.Interfaces.Agenda
{
    public interface IAgenda
    {
        Task<List<AgendaDto>> GetSchedulerPointsAsync(CancellationToken Token);
    }
}
