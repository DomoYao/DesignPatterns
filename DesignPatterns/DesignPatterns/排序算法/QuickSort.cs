using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DesignPatterns.Sort
{
    /// <summary>
    //从数列中挑出一个元素（一般都选择第一个），称为 "基准"（pivot），
    //重新排序数列，所有元素比基准值小的摆放在基准前面，所有元素比基准值大的摆在基准的后面（相同的数可以到任一边）。在这个分区退出之后，该基准就处于数列的中间位置。这个称为分区（partition）操作。
    //递归地（recursive）把小于基准值元素的子数列和大于基准值元素的子数列排序。
    /// </summary>
    public class QuickSort
    {
        public static int[] Sort(int[] array,int low,int high)
        {
            if (low < high)
            {
                int index = Partition(array, low, high);
                Sort(array, low, index - 1);
                Sort(array, index + 1, high);
            }

            return array;
        }

        public static int Partition(int[] array, int low, int high)
        {
            int i = low;
            int j = high;
            int temp = array[low];
            while (i<j)
            {
                // 先判断右半部分是否有小于temp的数，如果有则交换到array[i]
                while (i < j && temp < array[j])
                {
                    Debug.WriteLine(j);
                    j--;
                }

                if (i < j)
                {
                    array[i++] = array[j];
                }

                // 在判断左半部分是否有大于temp的数，如果有则交换到array[j]
                while (i < j && temp > array[i])
                {
                    i++;
                }

                if (i < j)
                {
                    array[j--] = array[i];
                }
            }

            array[i] = temp;

            return i;
        }

        public static void Client()
        {
            var intArray = new int[]
                {
                    23, 33, 14, 12, 33, 34, 322, 4, 65, 674, 23, 456, 22, 13, 4, 8, 2, 333, 455, 6, 7, 54, 43, 4, 44, 43
                    , 3, 2, 436
                };

            var result = Sort(intArray,0,intArray.Length-1);
            Console.WriteLine(string.Join(",",result));

        }
    }

}
