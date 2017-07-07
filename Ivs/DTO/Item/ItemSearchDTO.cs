using Core.Interface;
using System;
using System.ComponentModel.DataAnnotations;

namespace DTO.Item
{
    public class ItemSearchDTO : IDTO
    {
        public int? id { get; set; }

        [Display(Name = "Category")]
        public int? category_id { get; set; }

        [Display(Name = "Item Code")]
        public string code { get; set; }

        public string code_key { get; set; }

        [Display(Name = "Item Name")]
        public string name { get; set; }

        [Display(Name = "Dangerous")]
        public bool dangerous { get; set; }

        [Display(Name = "Discontinued Date")]
        public DateTime? discontinued_datetime { get; set; }

        public int page_count { get; set; }
    }
}
