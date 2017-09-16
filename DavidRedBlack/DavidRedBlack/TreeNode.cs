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
        Black
    }

    class TreeNode<T> where T : IComparable
    {
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
        public NodeColor Color;

        public int Height
        {
            get
            {
                if(this is NullNode<T>)
                {
                    return 0;
                }
                else
                {
                    if(LeftNode.Height > RightNode.Height)
                    {
                        return LeftNode.Height + 1;
                    }
                    else
                    {
                        return RightNode.Height + 1;
                    }
                }
            }
            set
            {

            }
        }
        public T Item;
        public TreeNode<T> LeftNode;
        public TreeNode<T> RightNode;
        public TreeNode<T> Parent;

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

        public void MoveBlacknessDown()
        {
            LeftNode.Color = NodeColor.Black;
            RightNode.Color = NodeColor.Black;
            Color = NodeColor.Red;
        }

        public TreeNode<T> GetSibling(TreeNode<T> child)
        {
            if (child == LeftNode)
            {
                return RightNode;
            }
            return LeftNode;
        }

        public TreeNode<T> Uncle
        {
            get
            {
                return Grandparent.GetSibling(this);
            }
        }

        public TreeNode<T> Grandparent
        {
            get
            {
                return Parent.Parent;
            }
        }

        public void RemoveChild(TreeNode<T> child)
        {
            if (child == LeftNode)
            {
                LeftNode = null;
                return;
            }
            RightNode = null;
        }
    }
}

