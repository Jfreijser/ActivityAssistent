using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityAssistent.Shared.Dtos.ActionPoints
{
    public  class CreateActionPointResolutionDto
    {
        public Guid ActionPointId { get; set; }
        public Guid ResolvedByUserId { get; set; }
        public string ClosureReason { get; set; } = string.Empty;
    }
}
