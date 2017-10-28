using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidRedBlack
{
    public enum NodeColor
    {
        Red,
        Black,
        DoubleBlack
    }

    class TreeNode<T> where T : IComparable
    {

        public T Item;
        public NodeColor Color;

        public TreeNode<T> Parent;
        public TreeNode<T> LeftNode;
        public TreeNode<T> RightNode;

        public TreeNode<T> Uncle
        {
            get
            {
                return Grandparent.GetSibling(Parent);
            }
        }
        public TreeNode<T> Grandparent
        {
            get
            {
                return Parent.Parent;
            }
        }
        public TreeNode<T> Sibiling
        {
            get
            {
                return Parent.GetSibling(this);
            }
        }
        public TreeNode<T> GetSibling(TreeNode<T> child)
        {
            if (child == LeftNode)
            {
                return RightNode;
            }
            return LeftNode;
        }
        public bool NephewsAreBlack
        {
            get
            {
                TreeNode<T> sibling = Parent.GetSibling(this);
                if (sibling.LeftNode.Color == NodeColor.Black && sibling.RightNode.Color == NodeColor.Black)
                {
                    return true;
                }

                return false;
            }
        }

        public int Height
        {
            get
            {
                if (this is NullNode<T>)
                {
                    return 0;
                }
                else
                {
                    if (LeftNode.Height > RightNode.Height)
                    {
                        return LeftNode.Height + 1;
                    }
                    else
                    {
                        return RightNode.Height + 1;
                    }
                }
            }
        }
        public int Balance
        {
            get
            {
                int temp = 0;

                temp -= LeftNode.Height;
                temp += RightNode.Height;

                return temp;
            }
        }
        
        public TreeNode() { }
        public TreeNode(T item)
        {
            Item = item;
            Color = NodeColor.Red;
            LeftNode = new NullNode<T>(this);
            RightNode = new NullNode<T>(this);
        }

        public void SetChild(TreeNode<T> newChild)
        {
            if (newChild.Parent == LeftNode)
            {
                LeftNode = newChild;
                return;
            }
            RightNode = newChild;
        }
        public void RemoveChild(TreeNode<T> child)
        {
            if (child == LeftNode)
            {
                LeftNode = new NullNode<T>(this);
                return;
            }
            RightNode = new NullNode<T>(this);
        }

        public void MoveBlacknessDown()
        {
            LeftNode.Color = NodeColor.Black;
            RightNode.Color = NodeColor.Black;
            Color = NodeColor.Red;
        }



    }
}

