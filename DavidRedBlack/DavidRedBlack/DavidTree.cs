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
            top.Color = NodeColor.Black;
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

        private void RuleCheck(TreeNode<T> currentNode)
        {
            if (currentNode == null || currentNode is NullNode<T>)
            {
                return;
            }
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
            TreeNode<T> replacementNode;
            if(!(foundNode is NullNode<T>))
            {
                while (!(foundNode.RightNode is NullNode<T>))
                {
                    foundNode = foundNode.RightNode;
                }
                //finding replacement node
                if (!(foundNode.LeftNode is NullNode<T>))
                {
                    replacementNode = foundNode.LeftNode;
                    replacementNode.Parent = foundNode.Parent;
                    if (replacementNode.Color == NodeColor.Red || foundNode.Color == NodeColor.Red)
                    {
                        replacementNode.Color = NodeColor.Black;
                    }
                }
                else
                {
                    replacementNode = new NullNode<T>(foundNode.Parent);
                    replacementNode.Color = NodeColor.DoubleBlack;
                }
            }
            else
            {
                //case where nodetodelete is leaf
                replacementNode = new NullNode<T>(foundNode.Grandparent);
                if (nodeToReplace.Color != NodeColor.Red)
                {
                    replacementNode.Color = NodeColor.DoubleBlack;
                }

                if (nodeToReplace.Parent.LeftNode == nodeToReplace)
                {
                    nodeToReplace.Parent.LeftNode = replacementNode;
                }
                else
                {
                    nodeToReplace.Parent.RightNode = replacementNode;
                }
                return replacementNode;
            }


            //fix the parent of the found node
            if (foundNode.Parent.RightNode == foundNode)
            {
                foundNode.Parent.RightNode = replacementNode;
            }
            else
            {
                foundNode.Parent.LeftNode = replacementNode;
            }

            if (nodeToReplace.Parent == null)
            {
                foundNode.Parent = null;
                top = foundNode;
            }
            else
            {
                //fix the parent of the NodeToReplace
                if (nodeToReplace.Parent.LeftNode == nodeToReplace)
                {
                    nodeToReplace.Parent.LeftNode = foundNode;
                }
                else if(nodeToReplace.Parent.RightNode == nodeToReplace)
                {
                    nodeToReplace.Parent.RightNode = foundNode;
                }
            }


            foundNode.LeftNode = nodeToReplace.LeftNode;
            foundNode.LeftNode.Parent = foundNode;

            foundNode.RightNode = nodeToReplace.RightNode;
            foundNode.RightNode.Parent = foundNode;

            foundNode.Parent = nodeToReplace.Parent;
            
            return replacementNode;
        }

        public void Delete(T item)
        {
            TreeNode<T> nodeToDelete = Search(item);
            TreeNode<T> replacementNode = ReplaceNode(nodeToDelete);

            
            if(top == replacementNode)
            {
                replacementNode.Color = NodeColor.Black;
            }
            else if (replacementNode.Color == NodeColor.DoubleBlack)
            {
                DeleteFixUp(replacementNode);
            }
            if(replacementNode == top)
            {
                replacementNode.Color = NodeColor.Black;
            }
        }

        public void DeleteFixUp(TreeNode<T> currentNode)
        {
            if(currentNode.Color != NodeColor.DoubleBlack || top == currentNode)
            {
                return;
            }
            //(a)If sibling s is black and at least one of the sibling's children is red, perform rotations on the sibling.
            //      Let the red child of s be r. This case can be divided into 4 subcases depending upon positions of s and r. 
            //      (This should remind you of AVL double rotates and the cases in InsertRuleCheck).            
            if (currentNode.Sibiling.Color == NodeColor.Black
                && (currentNode.Sibiling.LeftNode.Color == NodeColor.Red || currentNode.Sibiling.RightNode.Color == NodeColor.Red))
            {
                TreeNode<T> sibilingChild;
                if (currentNode.Sibiling.LeftNode.Color == NodeColor.Red)
                {
                    sibilingChild = currentNode.Sibiling.LeftNode;
                }
                else
                {
                    sibilingChild = currentNode.Sibiling.RightNode;
                }

                #region cases
                //i. Left-Right Case: If x is a right child and it's parent is a left child, rotate parent left (check step ii)
                if (sibilingChild == sibilingChild.Parent.RightNode
                    && sibilingChild.Parent == sibilingChild.Grandparent.LeftNode)
                {
                    RotateLeft(sibilingChild.Parent);
                    sibilingChild = sibilingChild.LeftNode;
                }
                //iii.Right - Left Case: If x is a left child and it's parent is a right child, rotate parent right (check step iv)
                else if (sibilingChild == sibilingChild.Parent.LeftNode
                    && sibilingChild.Parent == sibilingChild.Grandparent.RightNode)
                {
                    RotateRight(sibilingChild.Parent);
                    sibilingChild = sibilingChild.RightNode;
                }

                //ii.Left - Left Case: If x is a left child and it's parent is a left child, rotate grandparent right and swap colors of grandparent and parent.
                if (sibilingChild == sibilingChild.Parent.LeftNode
                    && sibilingChild.Parent == sibilingChild.Grandparent.LeftNode)
                {
                    NodeColor temp = sibilingChild.Grandparent.Color;
                    sibilingChild.Grandparent.Color = sibilingChild.Parent.Color;
                    sibilingChild.Parent.Color = temp;

                    RotateRight(sibilingChild.Grandparent);
                }

                //iv.Right - Right Case: If x is a right child and it's parent is a right child, rotate grandparent left and swap colors of grandparent and parent.
                else if (sibilingChild == sibilingChild.Parent.RightNode
                    && sibilingChild.Parent == sibilingChild.Grandparent.RightNode)
                {
                    NodeColor temp = sibilingChild.Grandparent.Color;
                    sibilingChild.Grandparent.Color = sibilingChild.Parent.Color;
                    sibilingChild.Parent.Color = temp;

                    RotateLeft(sibilingChild.Grandparent);
                }
                #endregion
                return;
            }

            //(b) If sibling is black and both its' children are black, perform recoloring. Set sibling to red
            //and parent to double black and recur for the parent. If parent was red, then we don't need to recur
            //since red +double black = single black.
            if (currentNode.Sibiling.Color == NodeColor.Black
                && currentNode.Sibiling.LeftNode.Color == NodeColor.Black
                && currentNode.Sibiling.RightNode.Color == NodeColor.Black)
            {

                if(currentNode.Parent.Color != NodeColor.Red)
                {
                    currentNode.Sibiling.Color = NodeColor.Red;
                    currentNode.Parent.Color = NodeColor.DoubleBlack;
                    DeleteFixUp(currentNode.Parent);
                }
                else
                {
                    currentNode.Sibiling.Color = NodeColor.Black;
                    currentNode.Parent.Color = NodeColor.Black;
                }
                return;
            }
            
            //(c)  If sibling is red, perform a single rotation to move sibling up, recolor old sibling and old parent.
            //      The new sibling is always black. This mainly converts the tree to black sibling case (by rotation) 
            //      and leads to case (a)or(b).
            if (currentNode.Sibiling.Color == NodeColor.Red)
            {
                if (currentNode.Sibiling.Parent.RightNode == currentNode)
                {
                    currentNode.Parent.Color = NodeColor.Black;
                    currentNode.Sibiling.Color = NodeColor.Black;

                    RotateLeft(currentNode.Sibiling);
                }
                else
                {
                    currentNode.Parent.Color = NodeColor.Black;
                    currentNode.Sibiling.Color = NodeColor.Black;

                    RotateRight(currentNode.Sibiling);
                }
                DeleteFixUp(currentNode);
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
            else if (currentNode.Item.CompareTo(key) < 0)
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
            Console.WriteLine($"{currentNode.Item}, {currentNode.Color.ToString()}");
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
                if (newParent.Parent.LeftNode == pivotNode)
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