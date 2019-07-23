using System.Collections.Generic;

namespace Tree_Benefit_Collection
{
    public interface ITreeNode<T> where T : class
    {
        int Id { get; set; }
        T Parent { get; set; }
        int? ParentId { get; set; }
        IList<T> Children { get; set; }
    }
}