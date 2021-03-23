using System;

namespace InductionTest.MinProductOfDigits
{
    public static class Program
    {
        static int Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Console.WriteLine($"usage: <num> (<num> ...)");
                    Console.WriteLine($"  <num> - integer number between 0 and {MaxValue}");
                    return 0;
                }

                foreach (string arg in args)
                {
                    long n;
                    if (!long.TryParse(arg, out n))
                        throw new ApplicationException($"Invalid number: {arg}");
                    if (n < 0)
                        throw new ApplicationException($"Negotive number: {n}");
                    if (n > MaxValue)
                        throw new ApplicationException($"Number too large: {n}");

                    long q = FindMinProductOfDigits(n);

                    Console.WriteLine($"N={n}, Q={q}");
                }

                return 0;
            }
            catch (ApplicationException e)
            {
                Console.Error.WriteLine($"Error: {e.Message}");
                return 1;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Unexpected error:\n{e}");
                return 1;
            }
        }

        const long MaxValue = (long)1e9;

        /// <summary>
        /// Finds the smallest integer number such that the product of the digits of specified number.<br/>
        /// E.g.:<br/>
        /// N=10, Q=25<br/>
        /// N=13, Q=-1(has no solution)<br/>
        /// N=8, Q=8<br/>
        /// N=90, Q=259<br/> 
        /// </summary>
        /// <param name="n">Integer number in range [0-1e9]</param>
        public static long FindMinProductOfDigits(long n)
        {
            if (n < 10)
            {
                return n;
            }
            for (long i = 9; i > 1; --i)
            {
                long r = n % i;
                if (r == 0)
                {
                    long q = n / i;
                    long qr = FindMinProductOfDigits(q);
                    if (qr != -1)
                    {
                        return qr * 10 + i;
                    }
                }
            }
            return -1;
        }
    }
}
