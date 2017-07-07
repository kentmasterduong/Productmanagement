using DTO.Item;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ivs.Models
{
    public class ResearchItemModel
    {
        public ItemDTO Item { get; set; }

        public string code { get; set; }

        public string name { get; set; }

        public int? category_id { get; set; }

        public int page_count { get; set; }

        public int page { get; set; } = 1;

        public StaticPagedList<ItemDTO> Items { get; set; }
    }
}