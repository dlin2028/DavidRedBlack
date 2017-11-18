using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DavidRedBlack
{
    class Program
    {
        static void Main(string[] args)
        {
            DavidTree<int> tree = new DavidTree<int>();

            Dictionary<string, Action> actions = new Dictionary<string, Action>();
            Dictionary<string, Action<string>> actionsWithArguements = new Dictionary<string, Action<string>>();

            Random rnd = new Random();


            actionsWithArguements.Add("test", (arguement) =>
            {
                for (int i = 1; i <= int.Parse(arguement); i++)
                {
                    tree.Insert(i);
                }
                for (int i = 1; i <= int.Parse(arguement); i++)
                {
                    //tree.Delete(i);
                }
            });

            actions.Add("lazy", () =>
            {
                tree.Insert(5);
                tree.Insert(3);
                tree.Insert(4);
                tree.Insert(7);
                tree.Insert(6);
            });

            actions.Add("help", () =>
            {
                Console.WriteLine("for help, please enter your credit card number and pin");
                Console.ReadLine();
                Console.WriteLine("Verifying...");
                Thread.Sleep(3000);
                Console.WriteLine("Invalid information, please try again");
            });

            actions.Add("inorder", () =>
            {
                tree.InOrder();
            });

            actions.Add("preorder", () =>
            {
                tree.PreOrder();
            });

            actions.Add("postorder", () =>
            {
                tree.PostOrder();
            });

            actionsWithArguements.Add("insert", (arguement) =>
            {
                tree.Insert(int.Parse(arguement));
            });

            actionsWithArguements.Add("search", (arguement) =>
            {
                Console.WriteLine(tree.Search(int.Parse(arguement)).ToString());
            });

            actionsWithArguements.Add("delete", (arguement) =>
            {
                tree.Delete(int.Parse(arguement));
            });

            while(true)
            {
                Console.WriteLine("operation:");
                string operation = Console.ReadLine().ToString();

                Action result;
                Action<string> resultWithArguements;

                if(actions.TryGetValue(operation, out result))
                {
                    result.Invoke();
                }
                else if(actionsWithArguements.TryGetValue(operation, out resultWithArguements))
                {
                    Console.WriteLine("arguements:");
                    resultWithArguements.Invoke(Console.ReadLine());
                }
                else
                {
                    Console.WriteLine("invalid command -- type help for help");
                }
            }
        }
    }
}
