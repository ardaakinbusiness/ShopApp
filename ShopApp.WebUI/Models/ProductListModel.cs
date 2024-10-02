﻿using ShopApp.Entity;

namespace ShopApp.WebUI.Models
{
    public class PageInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public string CurrentCategory { get; set; } 

        public int TotalPages()
        {
            return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
        }
    }

    public class ProductListModel
    {
        public PageInfo PageModel { get; set; }
        public List<Product> Products { get; set; }
    }
}
