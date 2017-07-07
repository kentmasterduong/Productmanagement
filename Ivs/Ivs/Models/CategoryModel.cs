using DTO.Category;
using PagedList;

namespace Ivs.Models
{
    public class CategoryModel
    {
        public CategoryDTO Category { get; set; }

        public StaticPagedList<CategoryDTO> Categorys { get; set; }
    }
}