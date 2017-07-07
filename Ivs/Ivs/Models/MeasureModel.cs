using DTO.Measure;
using PagedList;

namespace Ivs.Models
{
    public class MeasureModel
    {
        public MeasureDTO Item { get; set; }

        public StaticPagedList<MeasureDTO> Items { get; set; }
    }
}