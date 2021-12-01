using System.ComponentModel.DataAnnotations;
using TruckFactory.Web.Application.Shared;

namespace TruckFactory.Web.Models
{
    public class  TruckFormModel
    {
        public int? Id { get; set; }
        [Required]
        [MaxLength(200)]
        [MinLength(3)]
        public string? Name { get; set; }
        [Required]

        public EnumTruckModel Model { get; set; }  
        [Required]

        [Display(Name= "Production's year")]
        public int ProductionYear { get; set; }//Must be current

        [Required]
        [Display(Name = "Model's year (allowed options are  from current year to future)")]
        public int ModelYear { get; set; }//Current or >
    }
}
