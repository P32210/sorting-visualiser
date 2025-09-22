using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System.Diagnostics;
using System.Linq;
internal class Program
{
    static int height = 10;
    static int width = 20;
    static byte algorithm = 0;
    static readonly string[] algorithmName = [
        "Bubble sort",
        "Quick sort",
        "Cocktail shaker",
        "Radix sort LSD",
        "Radix sort MSD",
        "Insertion sort",
        "Selection sort",
        "Merge sort",
        "Heap sort",
        "Stooge sort",
        "Circle sort",
        "Gnome sort",
        "Shell sort",
        "Comb sort",
        "Bitonic sort",
        "Bogo sort",
        "Stalin sort",
        "Communism sort",
        "Sleep sort",
        "Slow sort",
        "Bozo sort",
        ];
    static readonly TrackedArray<int> array = new(width);
    static int window = 0;
    static int window2 = 0;
    static readonly int start = Console.CursorTop;
    static ulong delay = 400000;
    static bool sorted = true;
    static bool freeze = false;
    static bool ready = false;
    static string message = "";
    static ulong arrayAccesses = 0;
    static ulong tempArrayAccesses = 0;
    static bool countTempArrayAccesses = false;
    static ulong comparisons = 0;
    static ulong tempArrayComparisons = 0;
    static bool countTempArrayComparisons = false;
    static ulong iterations = 0;
    static readonly Stopwatch time = new();
    static ulong Elapsed => (ulong)time.Elapsed.TotalMilliseconds;
    static ulong ElapsedAdjusted => (ulong)(iterations * delay / 400000) + (Elapsed - (ulong)(iterations * previousDelay / 400000));
    static ulong previousDelay = 0;
    static async Task Main()
    {
        PerformanceMonitor.Init();
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Random rand = new();
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = (byte)rand.Next(1, height);
            Delay();
        }
        _ = Task.Run(() =>
        {
            while (true)
            {
                Console.Title = $"[</>] Algorithm: {algorithmName[algorithm]} | " +
                $"[+/-] Delay: {delay} | " +
                $"[\u2190/\u2192] Count: {width} | " +
                $"[\u2191/\u2193] Ceiling: {height} | " +
                $"[R] Reset | " +
                $"Array accesses: {arrayAccesses}{(countTempArrayAccesses ? $" + {tempArrayAccesses}" : "")}, " +
                $"Comps: {comparisons}{(countTempArrayComparisons ? $" + {tempArrayComparisons}" : "")} [A] | " +
                $"Iters: {iterations} | " +
                $"Elapsed real: {Elapsed}ms, adjusted: ~{Math.Max(ElapsedAdjusted, 0)}ms | " +
                $"{message}";
                if (PerformanceMonitor.CpuUsage > 25) Thread.Sleep(100);
            }
        });
        _ = Task.Run(() =>
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
                        array.Resize(width);
                        break;
                    case ConsoleKey.LeftArrow:
                        if (width == 1) width = 1;
                        else
                        {
                            width--;
                            array.Resize(width);
                        }
                        break;
                    case ConsoleKey.OemPeriod:
                        if (algorithm == algorithmName.Length - 1) algorithm = 0;
                        else algorithm++;
                        while (!sorted) Thread.Sleep(5);
                        sorted = true;
                        break;
                    case ConsoleKey.OemComma:
                        if (algorithm == 0) algorithm = (byte)(algorithmName.Length - 1);
                        else algorithm--;
                        while (!sorted) Thread.Sleep(5);
                        sorted = true;
                        break;
                    case ConsoleKey.R:
                        ready = false;
                        while (!sorted) Thread.Sleep(100);
                        ShuffleDelay();
                        arrayAccesses = 0;
                        tempArrayAccesses = 0;
                        comparisons = 0;
                        tempArrayComparisons = 0;
                        iterations = 0;
                        window = 0;
                        window2 = array.Length;
                        previousDelay = delay;
                        sorted = false;
                        ready = true;
                        break;
                    case ConsoleKey.N:
                        ready = false;
                        while (!sorted) Thread.Sleep(100);
                        for (int i = 0; i < array.Length; i++)
                        {
                            array[i] = (byte)rand.Next(1, height);
                            Delay();
                        }
                        arrayAccesses = 0;
                        tempArrayAccesses = 0;
                        comparisons = 0;
                        tempArrayComparisons = 0;
                        iterations = 0;
                        window = 0;
                        window2 = array.Length;
                        previousDelay = delay;
                        sorted = false;
                        ready = true;
                        break;
                    case ConsoleKey.OemPlus:
                        delay += 10000;
                        break;
                    case ConsoleKey.OemMinus:
                        if (delay <= 0) delay = 0;
                        else
                            delay -= 10000;
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
                                    array.Resize(width);
                                    break;
                                case ConsoleKey.LeftArrow:
                                    if (int.TryParse(input, out width)) message = "";
                                    else message = "Invalid input";
                                    break;
                                case ConsoleKey.OemPlus:
                                    if (ulong.TryParse(input, out delay)) message = "";
                                    else message = "Invalid input";
                                    break;
                                case ConsoleKey.OemMinus:
                                    if (ulong.TryParse(input, out delay)) message = "";
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
                    case ConsoleKey.A:
                        countTempArrayAccesses = !countTempArrayAccesses;
                        countTempArrayComparisons = !countTempArrayComparisons;
                        break;
                }
                if (PerformanceMonitor.CpuUsage > 25) Thread.Sleep(100);
            }
        });
        _ = Task.Run(() =>
        {
            while (true)
            {
                while (freeze || !ready || sorted) Thread.Sleep(100);
                time.Reset();
                time.Start();
                switch (algorithm)
                {
                    case 0: BubbleSort(); break;
                    case 1: QuickSort(0, array.Length - 1); break;
                    case 2: CocktailShaker(); break;
                    case 3: RadixSort(); break;
                    case 4: RadixSort(); break;
                    case 5: InsertionSort(); break;
                    case 6: SelectionSort(); break;
                    case 7: MergeSort(0, array.Length - 1); break;
                    case 8: HeapSort(); break;
                    case 9: StoogeSort(0, array.Length - 1); break;
                    case 10: CircleSort(); break;
                    case 11: GnomeSort(); break;
                    case 12: ShellSort(); break;
                    case 13: CombSort(); break;
                    case 14: BitonicSort(0, array.Length, 1); break;
                    case 15: BogoSort(); break;
                    case 16: StalinSort(); break;
                    case 17: CommunismSort(); break;
                    case 18: SleepSort(); break;
                    case 19: SlowSort(0, array.Length - 1); break;
                    case 20: BozoSort(); break;
                }
                time.Stop();
                sorted = true;
            }
        });
        while (true)
        {
            // while (sorted && ready) Thread.Sleep(1);
            Console.SetCursorPosition(0, start);
            Draw();
            Console.CursorVisible = false;
        }
    }
    static void BubbleSort()
    {
        for (int i = 0; i < array.Length; i++)
        {
            for (int j = array.Length - 1; j > i; j--)
            {
                comparisons++;
                if (array[j - 1] > array[j])
                    (array[j], array[j - 1]) = (array[j - 1], array[j]);
                if (algorithm == 0 && ready) Delay();
                else return;
            }
        }
    }
    static void CocktailShaker()
    {
        for (int i = 0; i < array.Length / 2; i++)
        {
            for (int j = window2 - 1; j > window; j--)
            {
                comparisons++;
                if (array[j - 1] > array[j]) (array[j], array[j - 1]) = (array[j - 1], array[j]);
                if (algorithm == 2 && ready) Delay();
                else return;
            }
            for (int j = ++window; j < window2; j++)
            {
                comparisons++;
                if (array[j - 1] > array[j]) (array[j], array[j - 1]) = (array[j - 1], array[j]);
                if (algorithm == 2 && ready) Delay();
                else return;
            }
            if (window >= array.Length - 1) window = 0;
            if (--window2 <= 0) window2 = array.Length;
        }
    }
    static void QuickSort(int leftIndex, int rightIndex)
    {
        var i = leftIndex;
        var j = rightIndex;
        var pivot = array[leftIndex];
        while (i <= j)
        {
            while (array[i] < pivot)
            {
                i++;
                comparisons++;
            }
            while (array[j] > pivot)
            {
                j--;
                comparisons++;
            }
            if (i <= j)
            {
                (array[j], array[i]) = (array[i], array[j]);
                i++;
                j--;
            }
            if (algorithm == 1 && ready) Delay();
            else return;
        }
        if (leftIndex < j)
            QuickSort(leftIndex, j);
        if (i < rightIndex)
            QuickSort(i, rightIndex);

    }
    static void RadixSort()
    {
        static void CountingSort(int size, int exponent)
        {
            if (algorithm == 3 && ready) Delay();
            else return;
            TrackedTempArray<int> outputArr = new((int[])array.Raw.Clone());
            TrackedTempArray<int> occurences = new(10);
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
                array[occurences[(outputArr[i] / exponent) % 10] - 1] = outputArr[i];
                occurences[(outputArr[i] / exponent) % 10]--;
            }
        }
        var max = array[0];
        for (int i = 1; i < array.Length; i++)
            if (array[i] > max)
            {
                max = array[i];
                comparisons++;
            }
        for (int exponent = 1; max / exponent > 0; exponent *= 10)
            CountingSort(array.Length, exponent);

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
                comparisons++;
                if (algorithm == 5 && ready) Delay();
                else return;
            }
        }

    }
    static void MergeSort(int left, int right)
    {
        static void MergeArray(int left, int middle, int right)
    {
        var leftArrayLength = middle - left + 1;
        var rightArrayLength = right - middle;
        TrackedTempArray<int> leftTempArray = new(leftArrayLength);
        TrackedTempArray<int> rightTempArray = new(rightArrayLength);
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
                array[k++] = leftTempArray[i++];
            else
                array[k++] = rightTempArray[j++];
            tempArrayComparisons++;
            if (algorithm == 7 && ready) Delay();
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
        if (left < right)
        {
            if (algorithm == 7 && ready) Delay();
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
                comparisons++;
                if (algorithm == 6 && ready) Delay();
                else return;
            }
            (array[i], array[min]) = (array[min], array[i]);
        }

    }
    static void HeapSort()
    {
        static void Heapify(int size, int index)
        {
            var largestIndex = index;
            var leftChild = 2 * index + 1;
            var rightChild = 2 * index + 2;
            if (leftChild < size && array[leftChild] > array[largestIndex])
                largestIndex = leftChild;
            if (rightChild < size && array[rightChild] > array[largestIndex])
                largestIndex = rightChild;
            comparisons += 2;
            if (largestIndex != index)
            {
                (array[largestIndex], array[index]) = (array[index], array[largestIndex]);
                Heapify(size, largestIndex);
            }
        }
        int size = array.Length;
        if (size <= 1)
            return;
        for (int i = size / 2 - 1; i >= 0; i--)
        {
            Heapify(size, i);
            if (algorithm == 8 && ready) Delay();
            else return;
        }
        for (int i = size - 1; i >= 0; i--)
        {
            (array[i], array[0]) = (array[0], array[i]);
            Heapify(i, 0);
            if (algorithm == 8 && ready) Delay();
            else return;
        }

    }
    static void StoogeSort(int left, int right)
    {
        if (algorithm == 9 && ready) Delay();
        else return;
        if (array[left] > array[right]) (array[left], array[right]) = (array[right], array[left]);
        comparisons++;
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
        static int CircleSortR(int lo, int hi, int numSwaps)
    {
        if (algorithm == 10 && ready) Delay();
        else return 0;
        if (lo == hi)
            return numSwaps;
        var high = hi;
        var low = lo;
        var mid = (hi - lo) / 2;
        while (lo < hi)
        {
            if (algorithm == 10 && ready) Delay();
            else return 0;
            if (array[lo] > array[hi])
            {
                (array[lo], array[hi]) = (array[hi], array[lo]);
                numSwaps++;
            }
            comparisons++;
            lo++;
            hi--;
        }
        if (lo == hi && array[lo] > array[hi + 1])
        {
            (array[lo], array[hi + 1]) = (array[hi + 1], array[lo]);
            numSwaps++;
        }
        comparisons++;
        numSwaps = CircleSortR(low, low + mid, numSwaps);
        numSwaps = CircleSortR(low + mid + 1, high, numSwaps);
        return numSwaps;
    }
        if (array.Length > 0)
            while (CircleSortR(0, array.Length - 1, 0) != 0)
                continue;
    }
    static void ShellSort()
    {
        for (int interval = array.Length / 2; interval > 0; interval /= 2)
        {
            for (int i = interval; i < array.Length; i++)
            {
                var currentKey = array[i];
                var k = i;
                comparisons++;
                while (k >= interval && array[k - interval] > currentKey)
                {
                    array[k] = array[k - interval];
                    k -= interval;
                    comparisons++;
                    if (algorithm == 12 && ready) Delay();
                    else return;
                }
                array[k] = currentKey;
                if (algorithm == 12 && ready) Delay();
                else return;
            }
        }
    }
    static void GnomeSort()
    {
        int pos = 0;
        while (pos < array.Length)
        {
            comparisons++;
            if (pos == 0 || array[pos] >= array[pos - 1])
                pos++;
            else
            {
                (array[pos], array[pos - 1]) = (array[pos - 1], array[pos]);
                pos--;
            }
            if (algorithm == 11 && ready) Delay();
            else return;
        }
    }
    static void CombSort()
    {
        int gap = array.Length;
        float shrinkFactor = 1.3f;
        bool swapped = false;
        while (gap > 1 || swapped)
        {
            if (gap > 1)
            {
                gap = (int)(gap / shrinkFactor);
            }
            swapped = false;
            for (int i = 0; gap + i < array.Length; i++)
            {
                if (array[i] > array[i + gap])
                {
                    (array[i], array[i + gap]) = (array[i + gap], array[i]);
                    swapped = true;
                }
                comparisons++;
                if (algorithm == 13 && ready) Delay();
                else return;
            }
            if (algorithm == 13 && ready) Delay();
            else return;
        }
    }
    static void BitonicSort(int low, int cnt, int dir) 
    {
        static void CompAndSwap(int i, int j, int dir)
        {
            int k;
            comparisons++;
            if (array[i] > array[j])
                k = 1;
            else
                k = 0;
            if (dir == k)
                (array[i], array[j]) = (array[j], array[i]);
            if (algorithm == 14 && ready) Delay();
            else return;
        }
        static void BitonicMerge(int low, int cnt, int dir)
        {
            if (cnt > 1)
            {
                int k = cnt / 2;
                for (int i = low; i < low + k; i++)
                    CompAndSwap(i, i + k, dir);
                BitonicMerge(low, k, dir);
                BitonicMerge(low + k, k, dir);
            }
            if (algorithm == 14 && ready) Delay();
            else return;
        } 
        if (cnt > 1)
        {
            int k = cnt / 2;
            BitonicSort(low, k, 1);
            BitonicSort(low + k, k, 0);
            BitonicMerge(low, cnt, dir);
        } 
    }
    static void BogoSort()
    {
        static void Shuffle()
        {
            Random rng = new();
            int n = array.Length;

            for (int i = n - 1; i > 0; i--)
            {
                int j = rng.Next(0, i + 1);
                (array[j], array[i]) = (array[i], array[j]);
            }
        }
        TrackedTempArray<int> sortedArr = new(array.Length);
        for (int i = 0; i < array.Length; i++) sortedArr[i] = array[i];
        Array.Sort(sortedArr.Raw);
        while (!array.Raw.SequenceEqual(sortedArr.Raw))
        {
            comparisons++;
            Shuffle();
            if (algorithm == 15 && ready) Delay();
            else return;
        }
    }
    static void StalinSort()
    {
        int lastLargest = array[0];
        for (int i = 0; i < array.Length; i++)
        {
            comparisons++;
            if (array[i] < lastLargest) array[i] = 0;
            else lastLargest = array[i];
            if (algorithm == 16 && ready) Delay();
            else return;
        }
    }
    static void CommunismSort()
    {
        int mean = (int)(array.Raw.Sum() / array.Length);
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = mean;
            if (algorithm == 17 && ready) Delay();
            else return;
        }
    }
    static void SleepSort()
    {
        TrackedTempObservableList<int> temp = new(array.Raw);
        bool set = false;
        void Add(int x)
        {
            while (set) ;
            Thread.Sleep(x * 50);
            temp.Add(x);
        }
        List<Thread> threads = [];
        int index;
        for (int i = 0; i < array.Length; i++)
        {
            iterations++;
            index = i;
            threads.Add(new Thread(() => Add(array[index])));
            threads[i].Start();
        }
        set = true;
        bool done = false;
        int nDone = 0;
        while (!done)
        {
            for (int i = 0; i < threads.Count; i++) if (threads[i].ThreadState == System.Threading.ThreadState.Stopped) nDone++;
            if (nDone >= threads.Count) done = true;
        }
    }
    static void SlowSort(int i, int j)
    {
        if (algorithm == 19 && ready) Delay();
        else return;
        if (i >= j)
            return;
        int m = (i + j) / 2;
        SlowSort(i, m);
        SlowSort(m + 1, j);
        comparisons++;
        if (array[j] < array[m])
            (array[m], array[j]) = (array[j], array[m]);
        SlowSort(i, j - 1);
    }
    static void BozoSort()
    {
        Random rng = new();
        TrackedTempArray<int> sortedArr = new(array.Length);
        for (int i = 0; i < array.Length; i++) sortedArr[i] = array[i];
        Array.Sort(sortedArr.Raw);
        int lhs;
        int rhs;
        while (!array.Raw.SequenceEqual(sortedArr.Raw))
        {
            comparisons++;
            lhs = rng.Next(0, array.Length);
            rhs = rng.Next(0, array.Length);
            (array[lhs], array[rhs]) = (array[rhs], array[lhs]);
            if (algorithm == 20 && ready) Delay();
            else return;
        }
    }
    static void ShuffleDelay()
    {
        Random rng = new();
        int n = array.Length;
        for (int i = n - 1; i > 0; i--)
        {
            int j = rng.Next(0, i + 1);
            (array[j], array[i]) = (array[i], array[j]);
            Delay();
        }
    }
    static void Delay()
    {
        ulong c = 0;
        while (c < delay) c++;
        iterations++;
        if (freeze)
        {
            time.Stop();
            while (freeze) Thread.Sleep(100);
            time.Start();
        }
    }
    static void Draw()
    {
        int[] last = (int[])array.Raw.Clone();
        var frame = new StringBuilder(height * last.Length * 2);
        for (int i = height - 1; i >= 0; i--)
        {
            for (int j = 0; j < last.Length; j++)
                frame.Append(last[j] > i ? "\u2588" : " ");
            frame.Append('\n');
        }
        Console.Write(frame.ToString());
        if (PerformanceMonitor.CpuUsage > 25) Thread.Sleep(15);
    }
    internal class TrackedArray<T>(int length)
    {
        private T[] _data = new T[length];
        public int Length => _data.Length;
        public T[] Raw => _data;
        public T this[int index]
        {
            get { arrayAccesses++; return _data[index]; }
            set { arrayAccesses++; _data[index] = value; }
        }
        public void Resize(int targetLength) => Array.Resize(ref _data, targetLength);
    }
    internal class TrackedTempArray<T>
    {
        public TrackedTempArray(int length)
        {
            _data = new T[length];
        }
        public TrackedTempArray(T[] input)
        {
            _data = input;
        }
        private T[] _data;
        public int Length => _data.Length;
        public T[] Raw => _data;
        public T this[int index]
        {
            get { tempArrayAccesses++; return _data[index]; }
            set { tempArrayAccesses++; _data[index] = value; }
        }
    }
    class TrackedTempObservableList<T>(T[] input)
    {
        private List<T> _data = [];
        private T[] boundArray = input;
        public int Count => _data.Count;
        public List<T> Raw => _data;
        public T this[int index]
        {
            get { tempArrayAccesses++; return _data[index]; }
            set { tempArrayAccesses++; _data[index] = value; }
        }
        public void Add(T value)
        {
            _data.Add(value);
            int end = _data.Count - 1;
            boundArray[end] = _data[end];
        }
    }
    internal static class PerformanceMonitor
    {
        private static PerformanceCounter _cpuUsage = new("Processor", "% Processor Time", "_Total");
        private static PerformanceCounter _ramUsage = new("Memory", "Available MBytes");
        public static void Init()
        {
            _ = _cpuUsage.NextValue();
            _ = _ramUsage.NextValue();
        }
        public static float CpuUsage => _cpuUsage.NextValue();
        public static float RamUsage => _ramUsage.NextValue();
    }
}