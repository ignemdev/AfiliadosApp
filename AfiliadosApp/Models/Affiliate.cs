using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace AfiliadosApp.Models
{
    public partial class Affiliate
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        [StringLength(150, MinimumLength = 1)]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Apellido")]
        [StringLength(150, MinimumLength = 1)]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Genero")]
        public string Genre { get; set; }
        [Required]
        [Display(Name = "Cedula")]
        [StringLength(11, MinimumLength = 11)]
        public string IdCard { get; set; }
        [Required]
        [Display(Name = "NSS")]
        [StringLength(11, MinimumLength = 11)]
        public string SocialSecurity { get; set; }
        [Display(Name = "Gastado")]
        public double SpendedAmount { get; set; }
        [Display(Name = "Registro")]
        public DateTime CreatedOn { get; set; }
        [Display(Name = "Estatus")]
        public int StatusId { get; set; } = 2;
        [Required]
        [Display(Name = "Plan")]
        public int InsurancePlanId { get; set; }

        [Display(Name = "Plan")]
        public virtual InsurancePlan InsurancePlan { get; set; }
        [Display(Name = "Registro")]
        public virtual Status Status { get; set; }

        [NotMapped]
        public bool IsOverdue { get 
            { 
                return SpendedAmount > ((InsurancePlan is not null) ? InsurancePlan.CoverageLimit : 0); 
            } 
        }
    }
}
