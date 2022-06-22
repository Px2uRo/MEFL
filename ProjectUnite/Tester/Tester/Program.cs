namespace Tester
{
    public class Program
    {
        static void Main(string[] args)
        {
            MyNode n1 = new MyNode() { Num = 1 };
            MyNode n2 = new MyNode() { Num = 2 };
            MyNode n3 = new MyNode() { Num = 3 };
            MyNode n4 = new MyNode() { Num = 4 };
            MyNode n5 = new MyNode() { Num = 5 };
            MyNode n6 = new MyNode() { Num = 6 };
            MyNode n7 = new MyNode() { Num = 7 };
            n1.ConnectedNodes = new MyNode[2] { n4, n6 };
            n2.ConnectedNodes = new MyNode[2] { n5, n6 };
            n3.ConnectedNodes = new MyNode[2] { n4, n5 };
            n4.ConnectedNodes = new MyNode[4] { n1,n3, n5,n6 };
            n5.ConnectedNodes = new MyNode[3] { n4, n6, n2 };
            n6.ConnectedNodes = new MyNode[3] { n4, n5, n2 };
            List<int> Loaded = new List<int>();
            MyNode[] myNodes = new MyNode[7] {n1,n2,n3,n4,n5,n6,n7};

            foreach (var item in n1.ConnectedNodes)
            {
                
            }
        }
    }

    class MyNode
    {
        public int Num { get; set; }
        
        public MyNode[] ConnectedNodes { get; set; }
    }
}