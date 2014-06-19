/// This code is based from here http://stackoverflow.com/questions/2094239/swap-two-items-in-listt

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Initiative_Tracker
{
    public static class IListExtension
    {
        public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            T temp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = temp;
            return list;
        }
        public static IList<T> MoveUp<T>(this IList<T> list, int index)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            if (list.Count < 2)
                throw new Exception("Can not move items in a list less than two.");

            if (index < 1)
                throw new ArgumentException("Can't move the first element up in a list.");

            if (index >= list.Count)
                throw new IndexOutOfRangeException("index");

            return list.Swap(index - 1, index);
        }
        public static IList<T> MoveDown<T>(this IList<T> list, int index)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            if (list.Count < 2)
                throw new Exception("Can not move items in a list less than two.");

            if (index >= list.Count)
                throw new IndexOutOfRangeException("index");

            if (index == list.Count - 1)
                throw new ArgumentException("Can't move the last element down in a list.");

            return list.Swap(index, index + 1);
        }
    }
}
