using DTO.Measure;
using PagedList;

namespace Ivs.Models
{
    public class MeasureModel
    {
        public MeasureDTO Measure { get; set; }

        public StaticPagedList<MeasureDTO> Measures { get; set; }
    }
}