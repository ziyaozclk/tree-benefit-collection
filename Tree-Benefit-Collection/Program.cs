using System;
using System.Collections.Generic;

namespace Tree_Benefit_Collection
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ProductCategory> categoryList = new List<ProductCategory>
            {
                new ProductCategory
                {
                    Id = 1,
                    Name = "Root",
                    ParentId = null
                },
                new ProductCategory
                {
                    Id = 2,
                    Name = "Child1",
                    ParentId = 1
                },
                new ProductCategory
                {
                    Id = 3,
                    Name = "Child2",
                    ParentId = 1
                },
                new ProductCategory
                {
                    Id = 4,
                    Name = "Child3",
                    ParentId = 2
                },
                new ProductCategory
                {
                    Id = 5,
                    Name = "Child4",
                    ParentId = 2
                }
            };

            TreeHelper.ConvertToForest(categoryList);
        }
    }
}