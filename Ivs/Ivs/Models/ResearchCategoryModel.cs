using DTO.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ivs.Models
{
    public class ResearchCategoryModel
    {
        public CategoryDTO Category { get; set; }

        public string code { get; set; }

        public string name { get; set; }

        public int? category_id { get; set; }

        public int page_count { get; set; }

        public int page { get; set; } = 1;

        public List<CategoryDTO> Categorys { get; set; }
    }
}