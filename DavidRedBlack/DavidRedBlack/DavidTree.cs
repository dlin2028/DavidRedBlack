using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidRedBlack
{
    class DavidTree<T> where T : IComparable
    {
        public TreeNode<T> top;

        public DavidTree()
        {
            top = null;
        }

        public void Insert(T item)
        {
            insert(item, top);
        }
        private bool insert(T item, TreeNode<T> currentNode)
        {
            //insert
            if (currentNode is NullNode<T>)
            {
                return true;
            }
            else if(top == null)
            {
                top = new TreeNode<T>(item);
                top.Color = NodeColor.Black;
                return true;
            }

            if (currentNode.Item.CompareTo(item) > 0)
            {
                if (insert(item, currentNode.LeftNode))
                {
                    currentNode.LeftNode = new TreeNode<T>(item);
                    currentNode.LeftNode.Parent = currentNode;
                    RuleCheck(top);
                }
            }
            else
            {
                if (insert(item, currentNode.RightNode))
                {
                    currentNode.RightNode = new TreeNode<T>(item);
                    currentNode.RightNode.Parent = currentNode;
                    RuleCheck(top);
                }
            }
            return false;
        }

        private void RuleCheck(TreeNode<T> currentNode)
        {
            if(currentNode == null)
            {
                return;
            }

            RuleCheck(currentNode.LeftNode);
            RuleCheck(currentNode.RightNode);

            if (currentNode.Parent == null
                || currentNode.Color != NodeColor.Red
                || currentNode.Parent.Color != NodeColor.Red)
            {
                return;
            }
            //we got two reds in a row oh noes
            //Case 1: If the the child's uncle is also red, move blackness down a level from its grandparent to BOTH of the grandparent's children
            if(currentNode.Uncle.Color == NodeColor.Red)
            {
                currentNode.Grandparent.MoveBlacknessDown();
                top.Color = NodeColor.Black;
                return;
            }

            //Case 2: If the node is a right child and the parent is a left child, rotate parent left and check both Case 4 and 5(see below)
            if(currentNode == currentNode.Parent.RightNode
                && currentNode.Parent == currentNode.Grandparent.LeftNode)
            {
                RotateLeft(currentNode.Parent);
            }
            //Case 3: If the node is a left child and it's parent is a right child, rotate parent right and check Case 4 and 5
            else if (currentNode == currentNode.Parent.LeftNode
                && currentNode.Parent == currentNode.Grandparent.RightNode)
            {
                RotateRight(currentNode.Parent);
            }
            
            //Case 4: If the node is a left child and it's parent is a left child, rotate grandparent right
            if(currentNode == currentNode.Parent.LeftNode
                && currentNode.Parent == currentNode.Grandparent.LeftNode)
            {
                RotateRight(currentNode.Grandparent);
            }

            //Case 5: If node is a right child and parent is a right child, rotate grandparent left
            if(currentNode == currentNode.Parent.RightNode
                && currentNode.Parent == currentNode.Grandparent.RightNode)
            {
                RotateLeft(currentNode.Grandparent);
            }

            //Finally set root to black
            top.Color = NodeColor.Black;

            
            //unwind recursion and rebalance
        }


        public void Delete(T item)
        {
            //balancing stuff
        }


        //rotations, search, and printing
        #region AVL + BST stuff
        public TreeNode<T> Search(T key)
        {
            return search(key, top);
        }
        private TreeNode<T> search(T key, TreeNode<T> currentNode)
        {
            if (currentNode == null)
            {
                throw new Exception("node with key not found");
            }

            if (currentNode.Item.CompareTo(key) > 0)
            {
                return search(key, currentNode.LeftNode);
            }
            else if (currentNode.Item.CompareTo(key) != 0)
            {
                return search(key, currentNode.RightNode);
            }
            else
            {
                return currentNode;
            }
            throw new Exception("How can you see this error? You should be dead by now!");
        }
        #region Printing
        public void PreOrder()
        {
            preOrder(top);
        }
        private void preOrder(TreeNode<T> currentNode)
        {
            if (currentNode == null)
            {
                return;
            }
            Console.WriteLine(currentNode.Item);
            preOrder(currentNode.LeftNode);
            preOrder(currentNode.RightNode);
        }

        public void PostOrder()
        {
            postOrder(top);
        }
        private void postOrder(TreeNode<T> currentNode)
        {
            if (currentNode == null)
            {
                return;
            }
            postOrder(currentNode.LeftNode);
            postOrder(currentNode.RightNode);

            Console.WriteLine(currentNode.Item);
        }

        public void InOrder()
        {
            inOrder(top);
        }
        private void inOrder(TreeNode<T> currentNode)
        {
            if (currentNode == null)
            {
                return;
            }
            inOrder(currentNode.LeftNode);
            Console.WriteLine(currentNode.Item);
            inOrder(currentNode.RightNode);
        }
        #endregion
        #region Rotating
        public void RotateRight(TreeNode<T> pivotNode)
        {
            TreeNode<T> newParent = pivotNode.RightNode;
            pivotNode.RightNode = newParent.LeftNode;
            newParent.LeftNode = pivotNode;
            newParent.Parent = pivotNode.Parent;
            pivotNode.Parent = newParent;
        }

        //literally copy pasted rotateRight and changed right to left and vice versa
        public void RotateLeft(TreeNode<T> pivotNode)
        {
            TreeNode<T> newParent = pivotNode.LeftNode;
            pivotNode.LeftNode = newParent.RightNode;
            newParent.RightNode = pivotNode;
            newParent.Parent = pivotNode.Parent;
            pivotNode.Parent = newParent;
        }
        #endregion
        #endregion
    }
}

