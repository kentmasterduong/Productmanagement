using Core.Interface;
using DTO.Measure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTO.Item
{
    public class ItemDTO : IDTO
    {
        [Key]
        public int? id { get; set; }

        [Required(ErrorMessage ="Category is required")]
        [Display(Name = "Category")]
        public int? category_id { get; set; }

        public string category_name { get; set; }

        [Display(Name = "Parent Category")]
        public string category_parent_name { get; set; }

        [Display(Name = "Item Code")]
        [Required(ErrorMessage = "Item Code is required")]
        [StringLength(30, ErrorMessage = "Item Code Maximum is 30")]
        public string code { get; set; }
        
        public string code_key { get; set; }

        [Display(Name = "Item Name")]
        [Required(ErrorMessage = "Item Name is required")]
        public string name { get; set; }

        [Display(Name = "Specification")]
        [StringLength(256, ErrorMessage = "Specification Maximum is 256")]
        public string specification { get; set; }

        [Display(Name = "Description")]
        [StringLength(64, ErrorMessage = "Description Maximum is 64")]
        public string description { get; set; }

        [Display(Name = "Dangerous")]
        public bool dangerous { get; set; }

        [Display(Name = "Discontinued Date")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Discontinued Date is required")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? discontinued_datetime { get; set; }

        [Display(Name = "Inventory Measure")]
        [Required(ErrorMessage = "Inventory Measure is required")]
        public int? inventory_measure_id { get; set; }

        [Display(Name = "Inventory Expired")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Inventory Expired must be numeric")]
        public int? inventory_expired { get; set; }

        [Display(Name = "Inventory Standard Cost")]
        [Range(-999999999.9999,999999999.9999, ErrorMessage = "Inventory Standard Cost must be numeric")]
        public decimal? inventory_standard_cost { get; set; }

        [Display(Name = "Inventory List Price")]
        [Range(-999999999.9999, 999999999.9999, ErrorMessage = "Inventory List Price must be numeric")]
        public decimal? inventory_list_price { get; set; }

        [Display(Name = "Manufacture Day")]
        [Range(-9999999.99, 9999999.99, ErrorMessage = "Manufacture Day must be numeric")]
        public decimal? manufacture_day { get; set; }

        [Display(Name = "Manufacture Make")]
        public bool manufacture_make { get; set; }

        [Display(Name = "Manufacture Tool")]
        public bool manufacture_tool { get; set; }

        [Display(Name = "Manufacture Finished Goods")]
        public bool manufacture_finished_goods { get; set; }

        [Display(Name = "Manufacture Size")]
        [StringLength(16, ErrorMessage = "Manufacture Size Maximum is 16")]
        public string manufacture_size { get; set; }

        [Display(Name = "Manufacture Size Measure")]
        [Required(ErrorMessage = "Manufacture Size Measure is required")]
        public int? manufacture_size_measure_id { get; set; }

        [Display(Name = "Manufacture Weight")]
        [StringLength(16, ErrorMessage = "Manufacture Weight Maximum is 16")]
        public string manufacture_weight { get; set; }

        [Display(Name = "Manufacture Weight Measure")]
        [Required(ErrorMessage = "Manufacture Weight Measure is required")]
        public int? manufacture_weight_measure_id { get; set; }

        [Display(Name = "Manufacture Style")]
        [StringLength(16, ErrorMessage = "Manufacture Style Maximum is 16")]
        public string manufacture_style { get; set; }

        [Display(Name = "Manufacture Class")]
        [StringLength(16, ErrorMessage = "Manufacture Class Maximum is 16")]
        public string manufacture_class { get; set; }

        [Display(Name = "Manufacture Color")]
        [StringLength(16, ErrorMessage = "Manufacture Color Maximum is 16")]
        public string manufacture_color { get; set; }
    }

}
