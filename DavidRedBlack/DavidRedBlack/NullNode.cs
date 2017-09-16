using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidRedBlack
{
    class NullNode<T> : TreeNode<T> where T : IComparable
    {
        public NullNode(TreeNode<T> parent)
            :base()
        {
            Parent = parent;
            Height = 0;
            Color = NodeColor.Black;
        }
    }
}
