using Core.Interface;
using System.ComponentModel.DataAnnotations;

namespace DTO.Category
{
    public class CategoryDTO :IDTO
    {
        [Key]
        public int? id { get; set; }
        
        public int? parent_id { get; set; }

        [Display(Name = "Category Parent")]
        public string parent_name { get; set; }

        [Required(ErrorMessage = "Category Code is required")]
        [Display(Name = "Category Code")]
        public string code { get; set; }

        public string code_key { get; set; }

        [Required(ErrorMessage = "Category Name is required")]
        [Display(Name = "Category Name")]
        public string name { get; set; }

        [Display(Name = "Description")]
        public string description { get; set; }
    }
}
