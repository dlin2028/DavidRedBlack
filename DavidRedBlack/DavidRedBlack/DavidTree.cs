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
            else if (top == null)
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
                    RuleCheck(currentNode.LeftNode);

                }
            }
            else
            {
                if (insert(item, currentNode.RightNode))
                {
                    currentNode.RightNode = new TreeNode<T>(item);
                    currentNode.RightNode.Parent = currentNode;
                    RuleCheck(currentNode.RightNode);
                }
            }
            return false;
        }

        private void RuleCheck(TreeNode<T> currentNode, bool fixUp = false)
        {
            if (currentNode == null || currentNode is NullNode<T>)
            {
                return;
            }

            if (!fixUp)
            {
                if (currentNode.Parent == null
                    || currentNode.Color != NodeColor.Red
                    || currentNode.Parent.Color != NodeColor.Red)
                {
                    RuleCheck(currentNode.Parent);
                    return;
                }
                //we got two reds in a row oh noes

                //Case 1: If the the child's uncle is also red, move blackness down a level from its grandparent to BOTH of the grandparent's children
                if (!(currentNode.Uncle is NullNode<T>) && currentNode.Uncle.Color == NodeColor.Red)
                {
                    //i.Change color of parent and uncle to BLACK
                    //ii.Change color of grandparent to RED
                    currentNode.Grandparent.MoveBlacknessDown();

                    //iii.Repeat Rule Check on x's grandparent
                    RuleCheck(currentNode.Grandparent);
                    return;
                }
            }


            //i. Left-Right Case: If x is a right child and it's parent is a left child, rotate parent left (check step ii)
            if (currentNode == currentNode.Parent.RightNode
                && currentNode.Parent == currentNode.Grandparent.LeftNode)
            {
                RotateLeft(currentNode.Parent);
                currentNode = currentNode.LeftNode;
            }
            //iii.Right - Left Case: If x is a left child and it's parent is a right child, rotate parent right (check step iv)
            else if (currentNode == currentNode.Parent.LeftNode
                && currentNode.Parent == currentNode.Grandparent.RightNode)
            {
                RotateRight(currentNode.Parent);
                currentNode = currentNode.RightNode;
            }

            //ii.Left - Left Case: If x is a left child and it's parent is a left child, rotate grandparent right and swap colors of grandparent and parent.
            if (currentNode == currentNode.Parent.LeftNode
                && currentNode.Parent == currentNode.Grandparent.LeftNode)
            {
                NodeColor temp = currentNode.Grandparent.Color;
                currentNode.Grandparent.Color = currentNode.Parent.Color;
                currentNode.Parent.Color = temp;

                RotateRight(currentNode.Grandparent);
            }

            //iv.Right - Right Case: If x is a right child and it's parent is a right child, rotate grandparent left and swap colors of grandparent and parent.
            else if (currentNode == currentNode.Parent.RightNode
                && currentNode.Parent == currentNode.Grandparent.RightNode)
            {
                NodeColor temp = currentNode.Grandparent.Color;
                currentNode.Grandparent.Color = currentNode.Parent.Color;
                currentNode.Parent.Color = temp;

                RotateLeft(currentNode.Grandparent);
            }


            RuleCheck(currentNode.Parent);

            //Finally set root to black
            top.Color = NodeColor.Black;
        }

        public TreeNode<T> ReplaceNode(TreeNode<T> nodeToReplace)
        {
            //get the largest value node of the leftnode
            TreeNode<T> foundNode = nodeToReplace.LeftNode;
            while (!(foundNode.LeftNode is NullNode<T>))
            {
                foundNode = foundNode.RightNode;
            }

            //fix the parent of the found node
            if (foundNode.Parent.RightNode == foundNode)
            {
                foundNode.Parent.RightNode = new NullNode<T>(foundNode.Parent);
            }
            else
            {
                foundNode.Parent.LeftNode = new NullNode<T>(foundNode.Parent);
            }

            foundNode.LeftNode = nodeToReplace.LeftNode;
            foundNode.RightNode = nodeToReplace.RightNode;
            foundNode.Parent = nodeToReplace.Parent;
            return foundNode;
        }

        public void Delete(T item)
        {
            TreeNode<T> nodeToDelete = Search(item);

            ReplaceNode(nodeToDelete);

            //balancing stuff
        }

        public void DeleteFixUp(TreeNode<T> nodeToBeFixed)
        {
            //If the nodeToBeFixed is a left child and it's sibling is red then set that sibling to Black and its parent to RED, then left rotate parent.
            if (nodeToBeFixed == nodeToBeFixed.Parent.LeftNode &&
                nodeToBeFixed.Sibiling.Color == NodeColor.Red)
            {
                nodeToBeFixed.Sibiling.Color = NodeColor.Black;
                nodeToBeFixed.Parent.Color = NodeColor.Red;
                RotateLeft(nodeToBeFixed.Parent);
            }

            //If the nodeToBeFixed's nephews' are black then the sibling becomes red. Other wise we check Cases 2-5 from the RuleCheck function.
            if (nodeToBeFixed.NephewsAreBlack)
            {
                nodeToBeFixed.Sibiling.Color = NodeColor.Red;
            }
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
            if (currentNode is NullNode<T>)
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
            if (currentNode is NullNode<T>)
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
            if (currentNode is NullNode<T>)
            {
                return;
            }
            inOrder(currentNode.LeftNode);
            Console.WriteLine(currentNode.Item);
            inOrder(currentNode.RightNode);
        }
        #endregion
        #region Rotating
        public void RotateLeft(TreeNode<T> pivotNode)
        {
            TreeNode<T> newParent = pivotNode.RightNode;

            pivotNode.RightNode = newParent.LeftNode;
            pivotNode.RightNode.Parent = pivotNode;

            newParent.Parent = pivotNode.Parent;
            pivotNode.Parent = newParent;

            newParent.LeftNode = pivotNode;

            if (newParent.Parent == null)
            {
                top = newParent;
            }
            else
            {
                if (newParent.Parent.RightNode == pivotNode)
                {
                    newParent.Parent.RightNode = newParent;
                }
                else
                {
                    newParent.Parent.LeftNode = newParent;
                }
            }
        }

        //literally copy pasted rotateRight and changed right to left and vice versa
        public void RotateRight(TreeNode<T> pivotNode)
        {
            TreeNode<T> newParent = pivotNode.LeftNode;

            pivotNode.LeftNode = newParent.RightNode;
            pivotNode.LeftNode.Parent = pivotNode;

            newParent.Parent = pivotNode.Parent;
            pivotNode.Parent = newParent;

            newParent.RightNode = pivotNode;

            if (newParent.Parent == null)
            {
                top = newParent;
            }
            else
            {
                if(newParent.Parent.LeftNode == pivotNode)
                {
                    newParent.Parent.LeftNode = newParent;
                }
                else
                {
                    newParent.Parent.RightNode = newParent;
                }
            }
        }
        #endregion
        #endregion
    }
}

