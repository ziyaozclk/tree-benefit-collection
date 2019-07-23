using System;
using System.Collections.Generic;

namespace Tree_Benefit_Collection
{
  public static class TreeHelper
    {
        #region Tree structure methods

        public static IList<T> ConvertToForest<T>(this IEnumerable<T> flatNodeList)
            where T : class, ITreeNode<T>
        {
            Dictionary<int, T> dictionary = new Dictionary<int, T>();
            foreach (T node in flatNodeList)
            {
                dictionary.Add(node.Id, node);
                node.Children = new List<T>();
            }

            List<T> rootNodes = new List<T>();

            foreach (T node in flatNodeList)
            {
                if (!node.ParentId.HasValue)
                {
                    rootNodes.Add(node);
                }
                else
                {
                    if (!dictionary.ContainsKey(node.ParentId.GetValueOrDefault()))
                    {
                        continue;
                    }

                    node.Parent = dictionary[node.ParentId.GetValueOrDefault()];

                    node.Parent.Children.Add(node);
                }
            }


            return rootNodes;
        }

        public static List<T> ConvertToFlatArray<T>(this IEnumerable<T> trees)
            where T : class, ITreeNode<T>
        {
            List<T> treeNodeList = new List<T>();
            foreach (T rootNode in trees)
            {
                foreach (T node in DepthFirstTraversal(rootNode))
                {
                    treeNodeList.Add(node);
                }
            }

            return treeNodeList;
        }

        #endregion


        #region Search methods

        public static T FindDescendant<T>(this T searchRoot, int id)
            where T : class, ITreeNode<T>
        {
            EnsureTreePopulated(searchRoot, "searchRoot");

            foreach (T child in DepthFirstTraversal(searchRoot))
            {
                if (child.Id == id)
                {
                    return child;
                }
            }

            return null;
        }

        public static T FindTreeNode<T>(this IEnumerable<T> trees, int id)
            where T : class, ITreeNode<T>
        {
            foreach (T rootNode in trees)
            {
                if (rootNode.Id == id)
                {
                    return rootNode;
                }

                T descendant = FindDescendant(rootNode, id);
                if (descendant != null)
                {
                    return descendant;
                }
            }

            return null;
        }

        #endregion


        #region Useful tree properties

        public static bool HasHeirachyLoop<T>(this T node)
            where T : class, ITreeNode<T>
        {
            EnsureTreePopulated(node, "node");

            T tempParent = node.Parent;
            while (tempParent != null)
            {
                if (tempParent.Id == node.Id)
                {
                    return true;
                }

                tempParent = tempParent.Parent;
            }

            return false;
        }


        public static T GetRootNode<T>(this T node)
            where T : class, ITreeNode<T>
        {
            EnsureTreePopulated(node, "node");

            T cur = node;
            while (cur.Parent != null)
            {
                cur = cur.Parent;
            }

            return cur;
        }


        public static int GetDepth<T>(this T node)
            where T : class, ITreeNode<T>
        {
            EnsureTreePopulated(node, "node");

            int depth = 0;
            while (node.Parent != null)
            {
                ++depth;
                node = node.Parent;
            }

            return depth;
        }

        public static NodeType GetNodeType<T>(this T node)
            where T : class, ITreeNode<T>
        {
            EnsureTreePopulated(node, "node");

            if (node.Parent == null)
            {
                return NodeType.Root;
            }
            else if (node.Children.Count == 0)
            {
                return NodeType.Leaf;
            }

            return NodeType.Internal;
        }

        #endregion


        #region Iterators

        public static IEnumerable<T> ClimbToRoot<T>(this T startNode)
            where T : class, ITreeNode<T>
        {
            EnsureTreePopulated(startNode, "startNode");

            T current = startNode;
            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }

        public static List<T> FromRootToNode<T>(this T node)
            where T : class, ITreeNode<T>
        {
            EnsureTreePopulated(node, "node");

            List<T> nodeToRootList = new List<T>();
            foreach (T n in ClimbToRoot(node))
            {
                nodeToRootList.Add(n);
            }

            nodeToRootList.Reverse();
            return nodeToRootList;
        }

        public static IEnumerable<T> DepthFirstTraversal<T>(this T startNode)
            where T : class, ITreeNode<T>
        {
            EnsureTreePopulated(startNode, "node");

            yield return startNode;
            foreach (T child in startNode.Children)
            {
                foreach (T grandChild in DepthFirstTraversal(child))
                {
                    yield return grandChild;
                }
            }
        }

        public static IEnumerable<T> DepthFirstTraversalOfList<T>(this IEnumerable<T> trees)
            where T : class, ITreeNode<T>
        {
            foreach (T rootNode in trees)
            {
                foreach (T node in DepthFirstTraversal(rootNode))
                {
                    yield return node;
                }
            }
        }

        public static IEnumerable<T> Siblings<T>(this T node, bool includeGivenNode)
            where T : class, ITreeNode<T>
        {
            EnsureTreePopulated(node, "node");

            if (GetNodeType(node) == NodeType.Root)
            {
                if (includeGivenNode)
                {
                    yield return node;
                }

                yield break;
            }

            foreach (T sibling in node.Parent.Children)
            {
                if (!includeGivenNode && sibling.Id == node.Id)
                {
                    continue;
                }

                yield return sibling;
            }
        }

        public static IEnumerable<T> BreadthFirstTraversal<T>(this T node, bool returnRootNode)
            where T : class, ITreeNode<T>
        {
            EnsureTreePopulated(node, "node");

            if (returnRootNode)
            {
                yield return node;
            }

            foreach (T child in node.Children)
            {
                yield return child;
            }


            foreach (T child in node.Children)
            {
                foreach (T grandChild in BreadthFirstTraversal(child, false))
                {
                    yield return grandChild;
                }
            }
        }

        #endregion


        #region Private methods

        [System.Diagnostics.Conditional("DEBUG")]
        private static void EnsureTreePopulated<T>(T node, string parameterName)
            where T : class, ITreeNode<T>
        {
            if (node == null)
            {
                throw new ArgumentNullException(parameterName, "The given node cannot be null.");
            }

            if (node.Children == null)
            {
                throw new ArgumentException(
                    "The children of " + parameterName +
                    " is null. Have you populated the tree fully by calling TreeHelper<T>.ConvertToForest(IEnumerable<T> flatNodeList)?",
                    parameterName);
            }
        }

        #endregion
    }
}