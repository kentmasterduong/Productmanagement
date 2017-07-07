using DTO.Category;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ivs.Models
{
    public class CategoryModel
    {
        public CategoryDTO Category { get; set; }

        public StaticPagedList<CategoryDTO> Categorys { get; set; }
    }
}