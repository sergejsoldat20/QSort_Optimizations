using System;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Arhitekture_drugi_projektni
{
    class QuickSort
    {
        private static int numberOfElements = 100000000;

        private static void Quick_Sort(int[] arr, int left, int right)
        {
            if (left < right)
            {
                int pivot = Partition(arr, left, right);

                if (pivot > 1)
                {
                    Quick_Sort(arr, left, pivot - 1);
                }
                if (pivot + 1 < right)
                {
                    Quick_Sort(arr, pivot + 1, right);
                }
            }

        }

        private static int Partition(int[] arr, int left, int right)
        {
            int pivot = arr[right];

            int i = (left - 1); 
                                
            for (int j = left; j <= right - 1; j++)
            {
                
                if (arr[j] < pivot)
                {
                    i++;    
                    swap(arr, i, j);
                }
            }
            swap(arr, i + 1, right);
            return (i + 1);
        }
        private static void Parallel_Optimisation(int[] arr, int left,int right)
        {
            if(left < right)
            {
                if (right - left < numberOfElements / 500)
                {
                    Quick_Sort(arr, left, right);
                }
                else
                {
                    int p = Partition(arr, left, right);
                    Parallel.Invoke(() => Parallel_Optimisation(arr, left, p - 1),
                       () => Parallel_Optimisation(arr, p + 1, right));
                }
            }
        }
        private static void swap(int[] arr, int i, int j)
        {
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
      
        }
        private static void Generate_Array(int[] arr)
        {
            int minNumber = 0;
            int maxNumber = 1000000;
            Random rand = new Random();
            for(int i = 0;i<numberOfElements;i++)
            {
                arr[i] = rand.Next(minNumber, maxNumber);
            }
             
                
        
        }
        private static void Print_Array(int[] arr)
        {
            for (int i = 0; i < arr.Length - 1; i++)
                Console.WriteLine(arr[i]);
        }
        private static void Write_Time(long time)
        {
            Console.WriteLine(time);
        }

        static int[] Pivot_Cache(int[] arr, int left, int right)
        {
            if (arr[left] > arr[right])
                swap(arr, left, right);


            int j = left + 1;
            int g = right - 1, k = left + 1;
            int p = arr[left], q = arr[right];

            while (j <= g)
            {

                if (arr[j] < p)
                {
                    swap(arr, j, k);
                    k++;
                }


                else if (arr[j] >= q)
                {
                    while (arr[g] > q && j < g)
                        g--;

                    swap(arr, j, g);
                    g--;

                    if (arr[j] < p)
                    {
                        swap(arr, j, k);
                        k++;
                    }
                }
                j++;
            }
            k--;
            g++;


            swap(arr, left, k);
            swap(arr, right, g);
            return new int[] { k, g };
        }
       
        private static void Dual_Pivot_Qucik_Sort(int[] arr, int left,int right)
        {
            if(left < right)
            {
                int[] p;
                p = Pivot_Cache(arr, left, right);



                Dual_Pivot_Qucik_Sort(arr, left, p[0] - 1);
                Dual_Pivot_Qucik_Sort(arr, p[0]+1, p[1] - 1);
                Dual_Pivot_Qucik_Sort(arr, p[1]+1, right);



            }
          
        }
        private static void Dual_Pivot_Qucik_Sort_Parallel(int[] arr, int left, int right)
        {
            if (left < right)
            {

                if (right - left < 20000)
                {
                    Quick_Sort(arr, left, right);
                }
                else
                {

                    int[] p;
                    p = Pivot_Cache(arr, left, right);
                    Parallel.Invoke(

                        () => Dual_Pivot_Qucik_Sort_Parallel(arr, left, p[0] - 1),
                        () => Dual_Pivot_Qucik_Sort_Parallel(arr, p[0] + 1, p[1] - 1),
                        () => Dual_Pivot_Qucik_Sort_Parallel(arr, p[1] + 1, right));
                }
            }
        }




        static void Main(string[] args)
        {
            Console.WriteLine("Quick sort bez optimizacije");
            int[] arr = new int[numberOfElements];
            Generate_Array(arr);
            Stopwatch st0 = new Stopwatch();
            st0.Start();
            Quick_Sort(arr, 0, arr.Length - 1);
            st0.Stop();
            Write_Time(st0.ElapsedMilliseconds);




            Console.WriteLine("Paralelni quick sort");
            int[] arr1 = new int[numberOfElements];
            Generate_Array(arr1);
            Stopwatch st = new Stopwatch();
            st.Start();
            Parallel_Optimisation(arr1, 0, arr1.Length - 1);
            st.Stop();
            Write_Time(st.ElapsedMilliseconds);




            Console.WriteLine("Quick sort sa 2 pivota");
            Stopwatch st1 = new Stopwatch();
            int[] arr2 = new int[numberOfElements];
            Generate_Array(arr2);
            st1.Start();
            Dual_Pivot_Qucik_Sort(arr2, 0, arr2.Length - 1);
            st1.Stop();
            Write_Time(st1.ElapsedMilliseconds);




            Console.WriteLine("Quick sort sa 2 pivota koji je paralelizovan");
            int[] arr3 = new int[numberOfElements];
            Stopwatch st2 = new Stopwatch();
            Generate_Array(arr3);
            st2.Start();
            Dual_Pivot_Qucik_Sort_Parallel(arr3, 0, arr3.Length - 1);
            st2.Stop();
            Write_Time(st2.ElapsedMilliseconds);

        }
    }
}
