namespace MyTestion;

internal class jntm
{
    static void Main()
    {
        List<String> lst = new() { "数学预习", "数学试卷", "语文作文", "英语", "物理" };
        List<int> Shuffleed = new();
        var rand = new Random();
        for (int i = 0; i < lst.Count; i++)
        {
            var index = rand.Next(0, lst.Count);
            if (!Shuffleed.Contains(index))
            {
                Shuffleed.Add(index);
            }
            else
            {
                i -= 1;
                if (Shuffleed.Count == lst.Count)
                {
                    break;
                }
            }
        }
        var res = new List<String>();
        foreach (var item in Shuffleed)
        {
            res.Add(lst[item]);
        }
        Console.WriteLine(res[0]);
        var YES = Console.ReadLine();
        if (YES == "")
        {
            Main();
        }
    }
}