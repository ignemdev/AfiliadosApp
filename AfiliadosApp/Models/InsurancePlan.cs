using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AfiliadosApp.Models
{
    public partial class InsurancePlan
    {
        public InsurancePlan()
        {
            Affiliates = new HashSet<Affiliate>();
        }

        public int Id { get; set; }
        [Required]
        [Display(Name ="Descripcion")]
        [StringLength(200,MinimumLength = 1)]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Cobertura")]
        [Range(1,1000000)]
        public double CoverageLimit { get; set; }
        [Display(Name = "Registro")]
        public DateTime CreatedOn { get; set; }
        [Required]
        [Display(Name = "Estatus")]
        public int StatusId { get; set; } = 2;

        public virtual Status Status { get; set; }
        public virtual ICollection<Affiliate> Affiliates { get; set; }
    }
}
