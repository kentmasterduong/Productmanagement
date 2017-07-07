using DTO.Item;
using PagedList;

namespace Ivs.Models
{
    public class ItemModel
    {
        public ItemSearchDTO Item { get; set; }

        public StaticPagedList<ItemDTO> Items { get; set; }
    }
}