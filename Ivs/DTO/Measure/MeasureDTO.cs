using Core.Interface;
using System.ComponentModel.DataAnnotations;

namespace DTO.Measure
{
    public class MeasureDTO:IDTO
    {
        [Key]
        public int? id { get; set; }

        [Required(ErrorMessage = "Measure Code is required")]
        [Display(Name = "Measure Code")]
        public string code { get; set; }

        public string code_key { get; set; }

        [Required(ErrorMessage = "Measure Name is required")]
        [Display(Name = "Measure Name")]
        public string name { get; set; }

        [Display(Name = "Description")]
        public string description { get; set; }

        public int page_count { get; set; }
    }
}
