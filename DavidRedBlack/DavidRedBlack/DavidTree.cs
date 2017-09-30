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
            if(currentNode == null || currentNode is NullNode<T>)
            {
                return;
            }

            if(!fixUp)
            {
                if (currentNode.Parent == null
                    || currentNode.Color != NodeColor.Red
                    || currentNode.Parent.Color != NodeColor.Red)
                {
                    top.Color = NodeColor.Black;
                    return;
                }
                //we got two reds in a row oh noes

                //Case 1: If the the child's uncle is also red, move blackness down a level from its grandparent to BOTH of the grandparent's children
                if (!(currentNode.Uncle is NullNode<T>) && currentNode.Uncle.Color == NodeColor.Red)
                {
                    currentNode.Grandparent.MoveBlacknessDown();
                    top.Color = NodeColor.Black;
                    return;
                }
            }

            //Case 2: If the node is a right child and the parent is a left child, rotate parent left and check both Case 4 and 5(see below)
            if(currentNode == currentNode.Parent.RightNode
                && currentNode.Parent == currentNode.Grandparent.LeftNode)
            {
                RotateLeft(currentNode.Parent);
                currentNode = currentNode.LeftNode;
            }
            //Case 3: If the node is a left child and it's parent is a right child, rotate parent right and check Case 4 and 5
            else if (currentNode == currentNode.Parent.LeftNode
                && currentNode.Parent == currentNode.Grandparent.RightNode)
            {
                RotateRight(currentNode.Parent);
                currentNode = currentNode.RightNode;
            }
            
            //Case 4: If the node is a left child and it's parent is a left child, rotate grandparent right
            if(currentNode == currentNode.Parent.LeftNode
                && currentNode.Parent == currentNode.Grandparent.LeftNode)
            {
                currentNode.Grandparent.Color = NodeColor.Red;
                currentNode.Parent.Color = NodeColor.Black;
                currentNode.Grandparent.RightNode.Color = NodeColor.Black;

                RotateRight(currentNode.Grandparent);
            }

            //Case 5: If node is a right child and parent is a right child, rotate grandparent left
            else if(currentNode == currentNode.Parent.RightNode
                && currentNode.Parent == currentNode.Grandparent.RightNode)
            {
                currentNode.Grandparent.Color = NodeColor.Red;
                currentNode.Parent.Color = NodeColor.Black;
                currentNode.Grandparent.LeftNode.Color = NodeColor.Black;

                RotateLeft(currentNode.Grandparent);
            }

            //Finally set root to black
            top.Color = NodeColor.Black;

            RuleCheck(currentNode.Parent);

            //unwind recursion and rebalance
        }

        public TreeNode<T> ReplaceNode(TreeNode<T> nodeToReplace)
        {
            //get the largest value node of the leftnode
            TreeNode<T> foundNode = nodeToReplace.LeftNode;
            while(!(foundNode.LeftNode is NullNode<T>))
            {
                foundNode = foundNode.RightNode;
            }

            //fix the parent of the found node
            if(foundNode.Parent.RightNode == foundNode)
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
            if(nodeToBeFixed == nodeToBeFixed.Parent.LeftNode &&
                nodeToBeFixed.Sibiling.Color == NodeColor.Red)
            {
                nodeToBeFixed.Sibiling.Color = NodeColor.Black;
                nodeToBeFixed.Parent.Color = NodeColor.Red;
                RotateLeft(nodeToBeFixed.Parent);
            }

            //If the nodeToBeFixed's nephews' are black then the sibling becomes red. Other wise we check Cases 2-5 from the RuleCheck function.
            if(nodeToBeFixed.NephewsAreBlack)
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

            newParent.Parent.LeftNode = newParent;
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

            newParent.Parent.RightNode = newParent;
        }
        #endregion
        #endregion
    }
}

