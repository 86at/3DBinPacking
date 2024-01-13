using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Sharp3DBinPacking.Combinatorics;

internal static class SmallPrimeUtility
{
    static SmallPrimeUtility()
    {
        PrimeTable = CalculatePrimes();
    }

    private static IReadOnlyList<int> PrimeTable { get; }

    public static IEnumerable<int> Factor(int i)
    {
        var primeIndex = 0;
        var prime = PrimeTable[primeIndex];
        var factors = new List<int>();

        while (i > 1)
        {
            var divResult = Math.DivRem(i, prime, out var remainder);

            if (remainder == 0)
            {
                factors.Add(prime);
                i = divResult;
            }
            else
            {
                ++primeIndex;
                prime = PrimeTable[primeIndex];
            }
        }

        return factors;
    }

    public static IEnumerable<int> DividePrimeFactors(IEnumerable<int> numerator, IEnumerable<int> denominator)
    {
        _ = numerator ?? throw new ArgumentNullException(nameof(numerator));
        _ = denominator ?? throw new ArgumentNullException(nameof(denominator));
        var product = numerator.ToList();
        foreach (var prime in denominator)
            product.Remove(prime);
        return product;
    }

    public static BigInteger EvaluatePrimeFactors(IEnumerable<int> value)
    {
        _ = value ?? throw new ArgumentNullException(nameof(value));
        return value.Aggregate<int, BigInteger>(1, (current, prime) => current * prime);
    }

    public static IReadOnlyList<int> CalculatePrimes()
    {
        var sieve = new BitArray(65536, true);
        for (var possiblePrime = 2; possiblePrime <= 256; ++possiblePrime)
        {
            if (!sieve[possiblePrime]) continue;

            for (var nonPrime = 2 * possiblePrime; nonPrime < 65536; nonPrime += possiblePrime)
                sieve[nonPrime] = false;
        }

        var primes = new List<int>();
        for (var i = 2; i < 65536; ++i)
            if (sieve[i])
                primes.Add(i);

        return primes;
    }
}