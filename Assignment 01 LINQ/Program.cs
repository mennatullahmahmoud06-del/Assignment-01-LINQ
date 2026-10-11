using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day_01_G03
{
    // Custom comparer for the case-insensitive ordering questions.
    internal class CaseInsensitiveComparer : IComparer<string>
    {
        public int Compare(string? x, string? y)
            => string.Compare(x, y, StringComparison.OrdinalIgnoreCase);
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            RestrictionOperators();
            ElementOperators();
            AggregateOperators();
            OrderingOperators();
            TransformationOperators();
        }

        static void Title(string text) => Console.WriteLine($"\n=== {text} ===");

        static void Print<T>(IEnumerable<T> items)
        {
            foreach (var item in items)
                Console.WriteLine(item);
        }

        #region LINQ - Restriction Operators
        static void RestrictionOperators()
        {
            Console.WriteLine("########## Restriction Operators ##########");

            Title("1. Products that are out of stock");
            var outOfStock = ListGenerator.ProductsList.Where(p => p.UnitsInStock == 0);
            Print(outOfStock);

            Title("2. Products in stock that cost more than 3.00 per unit");
            var inStockAndExpensive = ListGenerator.ProductsList
                                                   .Where(p => p.UnitsInStock > 0 && p.UnitPrice > 3.00M);
            Print(inStockAndExpensive);

            Title("3. Digits whose name is shorter than their value");
            string[] arr = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            // The index of each name is the digit's value.
            var shortNames = arr.Where((name, index) => name.Length < index);
            Print(shortNames);
        }
        #endregion

        #region LINQ - Element Operators
        static void ElementOperators()
        {
            Console.WriteLine("\n########## Element Operators ##########");

            Title("1. First product out of stock");
            var firstOutOfStock = ListGenerator.ProductsList.First(p => p.UnitsInStock == 0);
            Console.WriteLine(firstOutOfStock);

            Title("2. First product with price > 1000 (null if no match)");
            var firstExpensive = ListGenerator.ProductsList.FirstOrDefault(p => p.UnitPrice > 1000);
            Console.WriteLine(firstExpensive is null ? "null" : firstExpensive.ToString());

            Title("3. Second number greater than 5");
            int[] arr = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            var secondGreaterThanFive = arr.Where(n => n > 5).ElementAt(1);
            Console.WriteLine(secondGreaterThanFive);
        }
        #endregion

        #region LINQ - Aggregate Operators
        static void AggregateOperators()
        {
            Console.WriteLine("\n########## Aggregate Operators ##########");

            int[] arr = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            Title("1. Number of odd numbers in the array");
            Console.WriteLine(arr.Count(n => n % 2 != 0));

            Title("2. Customers and how many orders each has");
            var customerOrderCounts = ListGenerator.CustomersList
                                                   .Select(c => new { c.CustomerID, c.CustomerName, OrderCount = c.Orders.Count() });
            Print(customerOrderCounts);

            Title("3. Categories and how many products each has");
            var categoryCounts = ListGenerator.ProductsList
                                              .GroupBy(p => p.Category)
                                              .Select(g => new { Category = g.Key, ProductCount = g.Count() });
            Print(categoryCounts);

            Title("4. Total of the numbers in the array");
            Console.WriteLine(arr.Sum());

            string[] words = File.ReadAllLines("dictionary_english.txt");

            Title("5. Total number of characters of all words in dictionary_english.txt");
            Console.WriteLine(words.Sum(w => w.Length));

            Title("6. Length of the shortest word");
            Console.WriteLine(words.Min(w => w.Length));

            Title("7. Length of the longest word");
            Console.WriteLine(words.Max(w => w.Length));

            Title("8. Average length of the words");
            Console.WriteLine(words.Average(w => w.Length));
        }
        #endregion

        #region LINQ - Ordering Operators
        static void OrderingOperators()
        {
            Console.WriteLine("\n########## Ordering Operators ##########");

            string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            string[] arr = { "aPPLE", "AbAcUs", "bRaNcH", "BlUeBeRrY", "ClOvEr", "cHeRry" };

            Title("1. Products sorted by name");
            Print(ListGenerator.ProductsList.OrderBy(p => p.ProductName));

            Title("2. Words sorted case-insensitively (custom comparer)");
            Print(arr.OrderBy(w => w, new CaseInsensitiveComparer()));

            Title("3. Products sorted by units in stock, highest to lowest");
            Print(ListGenerator.ProductsList.OrderByDescending(p => p.UnitsInStock));

            Title("4. Digits sorted by name length, then alphabetically");
            Print(digits.OrderBy(d => d.Length).ThenBy(d => d));

            Title("5. Words sorted by length, then case-insensitively");
            Print(arr.OrderBy(w => w.Length).ThenBy(w => w, new CaseInsensitiveComparer()));

            Title("6. Products sorted by category, then unit price (highest to lowest)");
            Print(ListGenerator.ProductsList.OrderBy(p => p.Category).ThenByDescending(p => p.UnitPrice));

            Title("7. Words sorted by length, then case-insensitive descending");
            Print(arr.OrderBy(w => w.Length).ThenByDescending(w => w, new CaseInsensitiveComparer()));

            Title("8. Digits whose second letter is 'i', in reverse of the original order");
            Print(digits.Where(d => d.Length > 1 && d[1] == 'i').Reverse());
        }
        #endregion

        #region LINQ - Transformation Operators
        static void TransformationOperators()
        {
            Console.WriteLine("\n########## Transformation Operators ##########");

            Title("1. Names of the products");
            Print(ListGenerator.ProductsList.Select(p => p.ProductName));

            Title("2. Uppercase and lowercase version of each word");
            string[] words = { "aPPLE", "BlUeBeRrY", "cHeRry" };
            Print(words.Select(w => new { Upper = w.ToUpper(), Lower = w.ToLower() }));

            Title("3. Product properties, with UnitPrice renamed to Price");
            Print(ListGenerator.ProductsList.Select(p => new { p.ProductID, p.ProductName, p.Category, Price = p.UnitPrice, p.UnitsInStock }));

            Title("4. Does each number match its position in the array?");
            int[] arr = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            Console.WriteLine("Number: In-place?");
            foreach (var item in arr.Select((n, index) => new { Number = n, InPlace = n == index }))
                Console.WriteLine($"{item.Number}: {item.InPlace}");

            Title("5. Pairs where the number from numbersA is less than the number from numbersB");
            int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 };
            int[] numbersB = { 1, 3, 5, 7, 8 };
            var pairs = from a in numbersA
                        from b in numbersB
                        where a < b
                        select new { a, b };
            Console.WriteLine("Pairs where a < b:");
            foreach (var pair in pairs)
                Console.WriteLine($"{pair.a} is less than {pair.b}");

            Title("6. Orders where the total is less than 500.00");
            var smallOrders = ListGenerator.CustomersList
                                           .SelectMany(c => c.Orders)
                                           .Where(o => o.Total < 500.00M)
                                           .ToList();
            Print(smallOrders);
            Console.WriteLine($"({smallOrders.Count} orders)");

            Title("7. Orders made in 1998 or later");
            var recentOrders = ListGenerator.CustomersList
                                            .SelectMany(c => c.Orders)
                                            .Where(o => o.OrderDate.Year >= 1998)
                                            .ToList();
            Print(recentOrders);
            Console.WriteLine($"({recentOrders.Count} orders)");
        }
        #endregion
    }
}
