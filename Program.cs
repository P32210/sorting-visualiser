using System;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
internal class Program
{
    static int height = 10;
    static int width = 20;
    static byte algorithm = 0;
    static readonly string[] algorithmName = [
        "Bubble sort",
        "Quick sort",
        "Cocktail shaker",
        "Radix sort",
        "Insertion sort",
        "Selection sort",
        "Merge sort",
        "Heap sort",
        "Stooge sort",
        "Circle sort"
        ];
    static byte[] array = new byte[width];
    static int current;
    static int window = 0;
    static int window2 = 0;
    static readonly int start = Console.CursorTop;
    static int delay = 400000;
    static bool sorted = true;
    static bool freeze = false;
    static bool ready = false;
    static string message = "";
    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Random rand = new();
        Task.Run(() =>
        {
            while (true)
                Console.Title = $"[</>] Algorithm: {algorithmName[algorithm]} | [+/-] Delay: {delay} | [\u2190/\u2192] Bar count: {width} | [\u2191/\u2193] Bar height: {height} | [R] Reset | {message}";
        });
        Task.Run(() =>
        {
            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        height++;
                        break;
                    case ConsoleKey.DownArrow:
                        if (height == 10) height = 10;
                        else height--;
                        break;
                    case ConsoleKey.RightArrow:
                        width++;
                        Array.Resize(ref array, width);
                        break;
                    case ConsoleKey.LeftArrow:
                        if (width == 1) width = 1;
                        else
                        {
                            width--;
                            Array.Resize(ref array, width);
                        }
                        break;
                    case ConsoleKey.OemPeriod:
                        if (algorithm == 9) algorithm = 0;
                        else algorithm++;
                        break;
                    case ConsoleKey.OemComma:
                        if (algorithm == 0) algorithm = 9;
                        else algorithm--;
                        break;
                    case ConsoleKey.R:
                        ready = false;
                        while (!sorted) ;
                        for (int i = 0; i < array.Length; i++)
                        {
                            array[i] = (byte)rand.Next(1, height);
                            Delay();
                        }
                        window = 0;
                        window2 = array.Length - 1;
                        sorted = false;
                        ready = true;
                        break;
                    case ConsoleKey.OemPlus:
                        delay += 10000;
                        break;
                    case ConsoleKey.OemMinus:
                        if (delay <= 0) delay = 0;
                        else delay -= 10000;
                        break;
                    case ConsoleKey.F:
                        freeze = !freeze;
                        break;
                    case ConsoleKey.I:
                        message = "Insert mode: ";
                        string input = "";
                        bool mode = true;
                        while (mode)
                        {
                            mode = false;
                            var key = Console.ReadKey(true);
                            switch (key.Key)
                            {
                                case ConsoleKey.UpArrow:
                                    if (int.TryParse(input, out height)) message = "";
                                    else message = "Invalid input";
                                    break;
                                case ConsoleKey.DownArrow:
                                    if (int.TryParse(input, out height)) message = "";
                                    else message = "Invalid input";
                                    break;
                                case ConsoleKey.RightArrow:
                                    if (int.TryParse(input, out width)) message = "";
                                    else message = "Invalid input";
                                    break;
                                case ConsoleKey.LeftArrow:
                                    if (int.TryParse(input, out width)) message = "";
                                    else message = "Invalid input";
                                    break;
                                case ConsoleKey.OemPlus:
                                    if (int.TryParse(input, out delay)) message = "";
                                    else message = "Invalid input";
                                    break;
                                case ConsoleKey.OemMinus:
                                    if (int.TryParse(input, out delay)) message = "";
                                    else message = "Invalid input";
                                    break;
                                case ConsoleKey.Backspace:
                                    input = "";
                                    message = "Insert mode: ";
                                    mode = true;
                                    break;
                                default:
                                    input += key.KeyChar;
                                    message = "Insert mode: " + input;
                                    mode = true;
                                    break;
                            }
                            
                        }
                    break;
                }
            }
        });
        Task.Run(() =>
        {
            while (true)
            {
                if (freeze || !ready) continue;
                switch (algorithm)
                {
                    case 0: BubbleSort(); break;
                    case 1: QuickSort(array, 0, array.Length - 1); break;
                    case 2: CocktailShaker(); break;
                    case 3: if (!sorted) RadixSort(); break;
                    case 4: InsertionSort(); break;
                    case 5: SelectionSort(); break;
                    case 6: MergeSort(0, array.Length - 1); break;
                    case 7: if (!sorted) HeapSort(); break;
                    case 8: StoogeSort(0, array.Length - 1); break;
                    case 9: CircleSort(); break;
                }
                sorted = true;
            }
        });
        while (true)
        {
            Console.SetCursorPosition(0, start);
            Draw();
            Console.CursorVisible = false;
        }
    }
    static void BubbleSort()
    {
        for (int i = 0; i < array.Length; i++)
        {
            for (current = array.Length - 1; current > window; current--)
            {
                if (array[current - 1] > array[current]) (array[current], array[current - 1]) = (array[current - 1], array[current]);
                if (algorithm == 0 && ready) Delay();
                else return;
            }
            if (++window == array.Length - 1) window = 0;
        }

    }
    static void CocktailShaker()
    {
        for (int i = 0; i < array.Length; i++)
        {
            for (current = window2; current > window; current--)
            {
                if (array[current - 1] > array[current]) (array[current], array[current - 1]) = (array[current - 1], array[current]);
                if (algorithm == 2 && ready) Delay();
                else return;
            }
            for (current = ++window; current < window2; current++)
            {
                if (array[current - 1] > array[current]) (array[current], array[current - 1]) = (array[current - 1], array[current]);
                if (algorithm == 2 && ready) Delay();
                else return;
            }
            if (window >= array.Length - 1) window = 0;
            if (--window2 <= 0) window2 = array.Length - 1;
        }
        
    }
    static void QuickSort(byte[] input, int leftIndex, int rightIndex)
    {
        var i = leftIndex;
        var j = rightIndex;
        var pivot = input[leftIndex];
        while (i <= j)
        {
            while (input[i] < pivot)
                i++;
            while (input[j] > pivot)
                j--;
            if (i <= j)
            {
                (input[j], input[i]) = (input[i], input[j]);
                i++;
                j--;
            }
            if (algorithm == 1 && ready) Delay();
            else return;
        }
        if (leftIndex < j)
            QuickSort(input, leftIndex, j);
        if (i < rightIndex)
            QuickSort(input, i, rightIndex);
        
    }
    static void RadixSort()
    {
        static void CountingSort(byte[] input, int size, int exponent)
        {
            if (algorithm == 3 && ready) Delay();
            else return;
            var outputArr = (byte[])input.Clone();
            var occurences = new int[10];
            for (int i = 0; i < 10; i++)
                occurences[i] = 0;
            for (int i = 0; i < size; i++)
                occurences[(outputArr[i] / exponent) % 10]++;
            for (int i = 1; i < 10; i++)
                occurences[i] += occurences[i - 1];
            for (int i = size - 1; i >= 0; i--)
            {
                if (algorithm == 3 && ready) Delay();
                else return;
                input[occurences[(outputArr[i] / exponent) % 10] - 1] = outputArr[i];
                occurences[(outputArr[i] / exponent) % 10]--;
            }
        }
        var max = array[0];
        for (int i = 1; i < array.Length; i++)
            if (array[i] > max)
                max = array[i];
        for (int exponent = 1; max / exponent > 0; exponent *= 10)
            CountingSort(array, array.Length, exponent);

    }
    static void InsertionSort()
    {
        for (int i = 1; i < array.Length; i++)
        {
            var key = array[i];
            var flag = 0;
            for (int j = i - 1; j >= 0 && flag != 1;)
            {
                if (key < array[j])
                {
                    array[j + 1] = array[j];
                    j--;
                    array[j + 1] = key;
                }
                else flag = 1;
                if (algorithm == 4 && ready) Delay();
                else return;
            }
        }

    }
    static void MergeArray(int left, int middle, int right)
    {
        var leftArrayLength = middle - left + 1;
        var rightArrayLength = right - middle;
        var leftTempArray = new byte[leftArrayLength];
        var rightTempArray = new byte[rightArrayLength];
        int i, j;
        for (i = 0; i < leftArrayLength; ++i)
            leftTempArray[i] = array[left + i];
        for (j = 0; j < rightArrayLength; ++j)
            rightTempArray[j] = array[middle + 1 + j];
        i = 0;
        j = 0;
        int k = left;
        while (i < leftArrayLength && j < rightArrayLength)
        {
            if (leftTempArray[i] <= rightTempArray[j])
            {
                array[k++] = leftTempArray[i++];
            }
            else
            {
                array[k++] = rightTempArray[j++];
            }
            if (algorithm == 6 && ready) Delay();
            else return;
        }
        while (i < leftArrayLength)
        {
            array[k++] = leftTempArray[i++];
        }
        while (j < rightArrayLength)
        {
            array[k++] = rightTempArray[j++];
        }
    }
    static void MergeSort(int left, int right)
    {
        if (left < right)
        {
            if (algorithm == 6 && ready) Delay();
            else return;
            int middle = left + (right - left) / 2;
            MergeSort(left, middle);
            MergeSort(middle + 1, right);
            MergeArray(left, middle, right);
        }
        
    }
    static void SelectionSort()
    {
        var length = array.Length;
        for (int i = 0; i < length - 1; i++)
        {
            var min = i;
            for (int j = i + 1; j < length; j++)
            {
                if (array[j] < array[min])
                {
                    min = j;
                }
                if (algorithm == 5 && ready) Delay();
                else return;
            }
            (array[i], array[min]) = (array[min], array[i]);
        }
        
    }
    static void Heapify(byte[] input, int size, int index)
    {
        var largestIndex = index;
        var leftChild = 2 * index + 1;
        var rightChild = 2 * index + 2;
        if (leftChild < size && input[leftChild] > input[largestIndex])
            largestIndex = leftChild;
        if (rightChild < size && input[rightChild] > input[largestIndex])
            largestIndex = rightChild;
        if (largestIndex != index)
        {
            (input[largestIndex], input[index]) = (input[index], input[largestIndex]);
            Heapify(input, size, largestIndex);
        }
    }
    static void HeapSort()
    {
        int size = array.Length;
        if (size <= 1)
            return;
        for (int i = size / 2 - 1; i >= 0; i--)
        {
            Heapify(array, size, i);
            if (algorithm == 7 && ready) Delay();
            else return;
        }
        for (int i = size - 1; i >= 0; i--)
        {
            (array[i], array[0]) = (array[0], array[i]);
            Heapify(array, i, 0);
            if (algorithm == 7 && ready) Delay();
            else return;
        }
        
    }
    static void StoogeSort(int left, int right)
    {
        if (algorithm == 8 && ready) Delay();
        else return;
        if (array[left] > array[right]) (array[left], array[right]) = (array[right], array[left]);
        if (right - left > 1)
        {
            int t = (int)((right - left + 1) / 3);
            StoogeSort(left, right - t);
            StoogeSort(left + t, right);
            StoogeSort(left, right - t);
        }
    }
    static void CircleSort()
    {
        if (array.Length > 0)
            while (CircleSortR(0, array.Length - 1, 0) != 0)
                continue;
    }
    static int CircleSortR(int lo, int hi, int numSwaps)
    {
        if (algorithm == 9 && ready) Delay();
        else return 0;
        if (lo == hi)
            return numSwaps;
        var high = hi;
        var low = lo;
        var mid = (hi - lo) / 2;
        while (lo < hi)
        {
            if (algorithm == 9 && ready) Delay();
            else return 0;
            if (array[lo] > array[hi])
            {
                (array[lo], array[hi]) = (array[hi], array[lo]);
                numSwaps++;
            }
            lo++;
            hi--;
        }
        if (lo == hi && array[lo] > array[hi + 1])
        {
            (array[lo], array[hi + 1]) = (array[hi + 1], array[lo]);
            numSwaps++;
        }
        numSwaps = CircleSortR(low, low + mid, numSwaps);
        numSwaps = CircleSortR(low + mid + 1, high, numSwaps);
        return numSwaps;
    }
    static void Delay()
    {
        int c = 0;
        while (c < delay) c++;
    }
    static void Draw()
    {
        byte[] last = (byte[])array.Clone();
        // int lastCurrent = current;
        // string bar = " ";
        // string halfBar = " ";
        // string eraser = "  ";
        // string empty = " ";
        // for (int i = 0; i < barWidth; i++)
        // {
        //     bar += "\u2588";
        //     halfBar += "\u2584";
        //     empty += " ";
        //     eraser += "  ";
        // }
        // eraser += "\n";
        // string frame = "";
        // for (int i = height - 1; i >= 0; i--)
        // {
        //     for (int j = 0; j < last.Length; j++)
        //     {
        //         // if (j == lastCurrent)
        //         //     Console.ForegroundColor = ConsoleColor.Red;
        //         // else if (j < last.Length - 1 && j == lastCurrent + 1)
        //         //     Console.ForegroundColor = ConsoleColor.Green;
        //         // if (last[j] - i <= 0.5 && last[j] - i > 0)
        //         //     frame += " \u2584";
        //         if (last[j] > i)
        //             frame += " \u2588";
        //         else
        //             frame += "  ";
        //     }
        //     frame += "\n";
        // }
        // Console.Write(frame);
        var frame = new StringBuilder(height * last.Length * 2); // pre-size estimate
        for (int i = height - 1; i >= 0; i--)
        {
            for (int j = 0; j < last.Length; j++)
                frame.Append(last[j] > i ? "\u2588" : " ");
            frame.Append('\n');
        }
        Console.Write(frame.ToString());
    }
}