﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Entity
{
    public class Image
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }

        //OneToMany Relation (Her resim bir ürüne aittir.)
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
