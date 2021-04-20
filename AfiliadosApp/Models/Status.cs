using System;
using System.Collections.Generic;

#nullable disable

namespace AfiliadosApp.Models
{
    public partial class Status
    {
        public Status()
        {
            Affiliates = new HashSet<Affiliate>();
            InsurancePlans = new HashSet<InsurancePlan>();
        }

        public int Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Affiliate> Affiliates { get; set; }
        public virtual ICollection<InsurancePlan> InsurancePlans { get; set; }
    }
}
