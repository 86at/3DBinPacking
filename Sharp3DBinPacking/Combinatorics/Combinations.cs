#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Sharp3DBinPacking.Combinatorics;

public sealed class Combinations<T> : IEnumerable<IReadOnlyList<T>>
{
    private readonly Permutations<bool> _myPermutations;

    private readonly List<T> _myValues;

    public Combinations(IEnumerable<T> values, int lowerIndex,
        GenerateOption type = GenerateOption.WithoutRepetition)
    {
        _ = values ?? throw new ArgumentNullException(nameof(values));
        Type = type;
        LowerIndex = lowerIndex;
        _myValues = values.ToList();
        List<bool> myMap;
        if (type == GenerateOption.WithoutRepetition)
        {
            myMap = new List<bool>(_myValues.Count);
            myMap.AddRange(_myValues.Select((_, i) => i < _myValues.Count - LowerIndex));
        }
        else
        {
            myMap = new List<bool>(_myValues.Count + LowerIndex - 1);
            for (var i = 0; i < _myValues.Count - 1; ++i)
                myMap.Add(true);
            for (var i = 0; i < LowerIndex; ++i)
                myMap.Add(false);
        }

        _myPermutations = new Permutations<bool>(myMap);
    }

    public BigInteger Count => _myPermutations.Count;

    private GenerateOption Type { get; }

    private int LowerIndex { get; }

    public IEnumerator<IReadOnlyList<T>> GetEnumerator()
    {
        return new Enumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private sealed class Enumerator : IEnumerator<IReadOnlyList<T>>
    {
        private readonly Combinations<T> _myParent;

        private readonly Permutations<bool>.Enumerator _myPermutationsEnumerator;

        private List<T>? _myCurrentList;

        public Enumerator(Combinations<T> source)
        {
            _myParent = source;
            _myPermutationsEnumerator = (Permutations<bool>.Enumerator)_myParent._myPermutations.GetEnumerator();
        }

        void IEnumerator.Reset()
        {
            throw new NotSupportedException();
        }

        public bool MoveNext()
        {
            var ret = _myPermutationsEnumerator.MoveNext();
            _myCurrentList = null;
            return ret;
        }

        public IReadOnlyList<T> Current
        {
            get
            {
                ComputeCurrent();
                return _myCurrentList!;
            }
        }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            _myPermutationsEnumerator.Dispose();
        }

        private void ComputeCurrent()
        {
            if (_myCurrentList != null)
                return;

            _myCurrentList = new List<T>(_myParent.LowerIndex);
            var index = 0;
            var currentPermutation = _myPermutationsEnumerator.Current;
            foreach (var p in currentPermutation)
                if (!p)
                {
                    _myCurrentList.Add(_myParent._myValues[index]);
                    if (_myParent.Type == GenerateOption.WithoutRepetition)
                        ++index;
                }
                else
                {
                    ++index;
                }
        }
    }
}