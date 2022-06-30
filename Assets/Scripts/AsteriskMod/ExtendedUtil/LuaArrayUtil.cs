using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsteriskMod.ExtendedUtil
{
    public class LuaArrayUtil
    {
        public static LuaVector CreateVector() { return LuaVector.Zero.Copy(); }
        public static LuaVector CreateVector(float x, float y) { return new LuaVector(x, y); }
        public static LuaVector CreateVector(LuaVector original) { return original.Copy(); }


        private static bool IsSorted(int[] array)
        {
            if (array.Length < 2) return true;
            for (var i = 1; i < array.Length; i++)
            {
                if (array[i - 1] > array[i]) return false;
            }
            return true;
        }

        public static int[] BogoSort(int[] array, int trials = 1)
        {
            if (trials < 1) trials = int.MaxValue;
            while (!IsSorted(array))
            {
                array = array.OrderBy(i => Guid.NewGuid()).ToArray();
            }
            return array;
        }

        public static int[] CSharpListSort(int[] array)
        {
            List<int> _ = array.ToList();
            _.Sort();
            return _.ToArray();
        }

        /*
        public static int[] CountingSort(int[] array)
        {
            int min = 0;
            int max = 0;
            foreach (int value in array)
            {
                if (value < min) min = value;
                if (max < value) max = value;
            }
            int[] counter = new int[max - min + 1];
            foreach (int value in array)
            {
                counter[value - min]++;
            }


            int valueRefIndex = 0;
            int index = 0;

            while (valueRefIndex < counter.Length)
            {
            }


            for (var v = 0; v < counter.Length; v++)
            {
                int value = v + min;
                array[index]
            }
        }
        */
    }
}
