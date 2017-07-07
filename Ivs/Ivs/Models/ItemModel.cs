using DTO.Item;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ivs.Models
{
    public class ItemModel
    {
        public ItemSearchDTO Item { get; set; }

        public StaticPagedList<ItemDTO> Items { get; set; }
    }
}