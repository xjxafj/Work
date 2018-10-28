using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ListSort
{
    class Program
    {
        static void Main(string[] args)
        {
            List<C> L = new List<C>();
            L.Add(new C { n = 1, s = "b" });
            L.Add(new C { n = 3, s = "a" });
            L.Add(new C { n = 2, s = "c" });
            // 方法1 使用Comparison<T>委托。
            L.Sort((left, right) =>
            {
                if (left.n > right.n)
                    return 1;
                else if (left.n == right.n)
                    return 0;
                else
                    return -1;
            });

            // 方法2 使用IComparer<T>接口。
            L.Sort(new CComparer());

            // 方法3 除以上两种方法以外还可以使用另一种方法，在C类中实现IComparable<T>
            L.Sort();
        }
    }

    //方法二
    public class CComparer : IComparer<C>
    {
        public int Compare(C left, C right)
        {
            if (left.n > right.n)
                return 1;
            else if (left.n == right.n)
                return 0;
            else
                return -1;
        }
    }

    //方法三
    public class C : IComparable<C>
    {
        public int n;
        public string s;

        public int CompareTo(C other)
        {
            if (this.n > other.n)
                return 1;
            else if (this.n == other.n)
                return 0;
            else
                return -1;
        }
    }
}
