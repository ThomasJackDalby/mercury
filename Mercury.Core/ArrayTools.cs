using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Core
{
    public static class ArrayTools
    {
        public static bool AreEqual(int[] a, int[] b)
        {
            if (a[0] != b[0]) return false;
            if (a[1] != b[1]) return false;
            return true;
        }

        public static int[] Create(int size, int value)
        {
            int[] array = new int[size];
            for (int i = 0; i < size; i++) array[i] = value;           
            return array;
        }

        public static int[][] Create(int value, int length, int width)
        {
            int[][] array = new int[length][];
            for (int i = 0; i < length; i++)
            {
                array[i] = new int[width];
                for (int j = 0; j < width; j++)
                {
                    array[i][j] = value;
                }
            }
            return array;
        }

        public static bool Equals(int[] a, int[] b)
        {
            if (a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) return false;
            }
            return true;
        }
        public static T[] Append<T>(T[] a, T[] b)
        {
            T[] array = new T[a.Length + b.Length];
            for (int i = 0; i < a.Length; i++) array[i] = a[i];
            for (int i = 0; i < b.Length; i++) array[a.Length + i] = b[i];
            return array;
        }

        public static bool Contains<T>(IList<T> a, T b)
        {
            for (int i = 0; i < a.Count; i++)
            {
                if (a[i].Equals(b)) return true;
            }
            return false;
        }


        public static int Min(int[][] array)
        {
            int[] minIndex = MinIndex(array);
            return array[minIndex[0]][minIndex[1]];
        }
        public static int Min(int[] nums)
        {
            int minIndex = MinIndex(nums);
            return nums[minIndex];
        }
        public static int[] MinIndex(int[][] inputs)
        {
            int minValue = inputs[0][0];
            int[] minIndex = { 0, 0 };
            for (int i = 0; i < inputs.Length; i++)
            {
                for (int j = 0; j < inputs.Length; j++)
                {
                    if (inputs[i][j] < inputs[minIndex[0]][minIndex[1]])
                    {
                        minValue = inputs[i][j];
                        minIndex = new int[] { i, j };
                    }
                }
            }
            return minIndex;
        }
        public static int MinIndex(int[] inputs)
        {
            int minValue = inputs[0];
            int minIndex = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                if (inputs[i] < inputs[minIndex])
                {
                    minValue = inputs[i];
                    minIndex = i;
                }
            }
            return minIndex;
        }


        public static int Max(int[][] array)
        {
            int[] maxIndex = MaxIndex(array);
            return array[maxIndex[0]][maxIndex[1]];
        }
        public static int Max(int[][] array, int ignore)
        {
            int[] maxIndex = MaxIndex(array, ignore);
            return array[maxIndex[0]][maxIndex[1]];
        }
        public static int Max(int[] nums)
        {
            int maxIndex = MaxIndex(nums);
            return nums[maxIndex];
        }
        public static int[] MaxIndex(int[][] inputs, int ignore)
        {
            int maxValue = 0;
            int[] maxIndex = { 0, 0 };
            for (int i = 0; i < inputs.Length; i++)
            {
                for (int j = 0; j < inputs.Length; j++)
                {
                    if (inputs[i][j] >= ignore) continue;
                    if (inputs[i][j] > maxValue)
                    {
                        maxValue = inputs[i][j];
                        maxIndex = new int[] { i, j };
                    }
                }
            }
            return maxIndex;
        }
        public static int[] MaxIndex(int[][] inputs)
        {
            int maxValue = inputs[0][0];
            int[] maxIndex = { 0, 0 };
            for (int i = 0; i < inputs.Length; i++)
            {
                for (int j = 0; j < inputs.Length; j++)
                {
                    if (inputs[i][j] > inputs[maxIndex[0]][maxIndex[1]])
                    {
                        maxValue = inputs[i][j];
                        maxIndex = new int[] { i, j };
                    }
                }
            }
            return maxIndex;
        }
        public static int MaxIndex(int[] inputs)
        {
            int maxValue = inputs[0];
            int maxIndex = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                if (inputs[i] > inputs[maxIndex])
                {
                    maxValue = inputs[i];
                    maxIndex = i;
                }
            }
            return maxIndex;
        }

    }
}
