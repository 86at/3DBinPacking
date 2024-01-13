#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Sharp3DBinPacking.Combinatorics;

public sealed class Permutations<T> : IEnumerable<IReadOnlyList<T>>
{
    private readonly int[] _myLexicographicOrders;

    private readonly List<T> _myValues;

    public Permutations(IEnumerable<T> values, GenerateOption type)
        : this(values, type, null)
    {
    }

    public Permutations(IEnumerable<T> values, GenerateOption type = GenerateOption.WithoutRepetition,
        IComparer<T>? comparer = null)
    {
        _ = values ?? throw new ArgumentNullException(nameof(values));


        _myValues = values.ToList();
        _myLexicographicOrders = new int[_myValues.Count];

        if (type == GenerateOption.WithRepetition)
        {
            for (var i = 0; i < _myLexicographicOrders.Length; ++i) _myLexicographicOrders[i] = i;
        }
        else
        {
            comparer ??= Comparer<T>.Default;

            _myValues.Sort(comparer);
            var j = 1;
            if (_myLexicographicOrders.Length > 0) _myLexicographicOrders[0] = j;

            for (var i = 1; i < _myLexicographicOrders.Length; ++i)
            {
                if (comparer.Compare(_myValues[i - 1], _myValues[i]) != 0) ++j;

                _myLexicographicOrders[i] = j;
            }
        }

        Count = GetCount();
    }

    public BigInteger Count { get; }

    public IEnumerator<IReadOnlyList<T>> GetEnumerator()
    {
        return new Enumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private BigInteger GetCount()
    {
        var runCount = 1;
        var divisors = Enumerable.Empty<int>();
        var numerators = Enumerable.Empty<int>();

        for (var i = 1; i < _myLexicographicOrders.Length; ++i)
        {
            numerators = numerators.Concat(SmallPrimeUtility.Factor(i + 1));

            if (_myLexicographicOrders[i] == _myLexicographicOrders[i - 1])
            {
                ++runCount;
            }
            else
            {
                for (var f = 2; f <= runCount; ++f)
                    divisors = divisors.Concat(SmallPrimeUtility.Factor(f));

                runCount = 1;
            }
        }

        for (var f = 2; f <= runCount; ++f)
            divisors = divisors.Concat(SmallPrimeUtility.Factor(f));

        return SmallPrimeUtility.EvaluatePrimeFactors(
            SmallPrimeUtility.DividePrimeFactors(numerators, divisors)
        );
    }

    public sealed class Enumerator : IEnumerator<IReadOnlyList<T>>
    {
        private readonly int[] _myLexicographicalOrders;

        private readonly Permutations<T> _myParent;

        private readonly List<T> _myValues;

        private int _myKviTemp;

        private Position _myPosition;

        public Enumerator(Permutations<T> source)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _myParent = source;
            _myLexicographicalOrders = new int[source._myLexicographicOrders.Length];
            _myValues = new List<T>(source._myValues.Count);
            source._myLexicographicOrders.CopyTo(_myLexicographicalOrders, 0);
            _myPosition = Position.BeforeFirst;
        }

        void IEnumerator.Reset()
        {
            throw new NotSupportedException();
        }

        public bool MoveNext()
        {
            switch (_myPosition)
            {
                case Position.BeforeFirst:
                    _myValues.AddRange(_myParent._myValues);
                    _myPosition = Position.InSet;
                    break;
                case Position.InSet:
                    if (_myValues.Count < 2)
                        _myPosition = Position.AfterLast;
                    else if (!NextPermutation()) _myPosition = Position.AfterLast;

                    break;
                case Position.AfterLast:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return _myPosition != Position.AfterLast;
        }

        object IEnumerator.Current => Current;

        public IReadOnlyList<T> Current
        {
            get
            {
                if (_myPosition == Position.InSet)
                    return new List<T>(_myValues);

                throw new InvalidOperationException();
            }
        }

        public void Dispose()
        {
        }

        private bool NextPermutation()
        {
            var i = _myLexicographicalOrders.Length - 1;

            while (_myLexicographicalOrders[i - 1] >= _myLexicographicalOrders[i])
            {
                --i;
                if (i == 0) return false;
            }

            var j = _myLexicographicalOrders.Length;

            while (_myLexicographicalOrders[j - 1] <= _myLexicographicalOrders[i - 1]) --j;

            Swap(i - 1, j - 1);

            ++i;

            j = _myLexicographicalOrders.Length;

            while (i < j)
            {
                Swap(i - 1, j - 1);
                ++i;
                --j;
            }

            return true;
        }

        private void Swap(int i, int j)
        {
            (_myValues[i], _myValues[j]) = (_myValues[j], _myValues[i]);
            _myKviTemp = _myLexicographicalOrders[i];
            _myLexicographicalOrders[i] = _myLexicographicalOrders[j];
            _myLexicographicalOrders[j] = _myKviTemp;
        }

        private enum Position
        {
            BeforeFirst,
            InSet,
            AfterLast
        }
    }
}