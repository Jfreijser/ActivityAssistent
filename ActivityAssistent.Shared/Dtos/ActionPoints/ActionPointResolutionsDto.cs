using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityAssistent.Shared.Dtos.ActionPoints
{
    public class ActionPointResolutionsDto
    {
        public Guid ResolutionId { get; set; }
        public Guid ActionPointId { get; set; }
        public Guid ResolvedByUserId { get; set; }
        public string ClosureReason { get; set; } = string.Empty;
        public DateTime ResolvedAt { get; set; }

        public string ResolvedByFullName { get; set; } = string.Empty;
    }
}
