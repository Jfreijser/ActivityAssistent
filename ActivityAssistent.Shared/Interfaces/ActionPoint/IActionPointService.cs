using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.Shared.Dtos.ActionPoints;

namespace ActivityAssistent.Shared.Interfaces.ActionPoint
{
    public interface IActionPointService
    {
        // Ophalen van alle openstaande actiepunten voor een medewerker
        Task<IEnumerable<ActionPointDto>> GetPendingActionPointsAsync(string UserId);

        // Een actiepunt bewerken (bijvoorbeeld afvinken als voltooid)
        Task<ActionPointDto> UpdateActionPointAsync(ActionPointDto UpdatedActionPoint);

        // Een actiepunt verwijderen
        Task DeleteActionPointAsync(string ActionPointId);
    }
}
