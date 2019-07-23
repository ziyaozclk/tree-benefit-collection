using System.Collections.Generic;

namespace Tree_Benefit_Collection
{
    public class ProductCategory : ITreeNode<ProductCategory>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProductCategory Parent { get; set; }
        public int? ParentId { get; set; }
        public IList<ProductCategory> Children { get; set; }
    }
}