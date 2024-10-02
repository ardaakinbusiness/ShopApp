﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Entity
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }        
        public decimal Price { get; set; }
        public string Description { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }

        //OneToMany Relation (Bir ürünün birden fazla resmi olabilir.)

        public List<Image> Images { get; set; }

        public List<Comment> Comments { get; set; }


        public Product()
        {
            Images = new List<Image>();
            Comments = new List<Comment>();
        }
    }
}
