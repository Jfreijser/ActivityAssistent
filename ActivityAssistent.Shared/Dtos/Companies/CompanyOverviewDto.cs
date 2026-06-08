using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityAssistent.Shared.Dtos.Companies
{
    public class CompanyOverviewDto
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public int TotalConversations { get; set; }
        public DateTime? LastContactDate { get; set; }
    }
}
