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
            if (currentNode == null)
            {
                if (top == null)
                {
                    top = new TreeNode<T>(item);
                }
                return true;
            }

            if (currentNode.Item.CompareTo(item) > 0)
            {
                if (insert(item, currentNode.LeftNode))
                {
                    currentNode.LeftNode = new TreeNode<T>(item);
                }
            }
            else
            {
                if (insert(item, currentNode.RightNode))
                {
                    currentNode.RightNode = new TreeNode<T>(item);
                }
            }

            //Case 1: If the the child's uncle is also red, move blackness down a level from its grandparent to BOTH of the grandparent's children
            

            //Case 2: If the node is a right child and the parent is a left child, rotate parent left and check both Case 4 and 5(see below)


            //Case 3: If the node is a left child and it's parent is a right child, rotate parent right and check Case 4 and 5


            //Case 4: If the node is a left child and it's parent is a left child, rotate grandparent right


            //Case 5: If node is a right child and parent is a right child, rotate grandparent left


            //Finally set root to black
            top.color = NodeColor.Black;


            return false;
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
        public void RotateRight(TreeNode<T> middleNode)
        {
            TreeNode<T> oldRightNode = middleNode.RightNode;
            middleNode.RightNode = middleNode.Parent;
            middleNode.Parent = middleNode.RightNode.Parent; //the rightnode was the old parent

            if (middleNode.Parent == null)
            {
                top = middleNode;
            }

            middleNode.RightNode.Parent = middleNode;
            middleNode.RightNode.LeftNode = oldRightNode;
        }

        //literally copy pasted rotateRight and changed right to left and vice versa
        public void RotateLeft(TreeNode<T> middleNode)
        {
            TreeNode<T> oldLeftNode = middleNode.LeftNode;
            middleNode.LeftNode = middleNode.Parent;
            middleNode.Parent = middleNode.LeftNode.Parent;

            if (middleNode.Parent == null)
            {
                top = middleNode;
            }

            middleNode.LeftNode.Parent = middleNode;
            middleNode.LeftNode.RightNode = oldLeftNode;
        }
        #endregion
        #endregion
    }
}

