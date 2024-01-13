#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Sharp3DBinPacking.Combinatorics;

public sealed class Variations<T> : IEnumerable<IReadOnlyList<T>>
{
    private readonly Permutations<int>? _myPermutations;

    private readonly List<T> _myValues;

    public Variations(IEnumerable<T> values, int lowerIndex, GenerateOption type)
    {
        Type = type;
        LowerIndex = lowerIndex;
        _myValues = values.ToList();

        if (type != GenerateOption.WithoutRepetition) return;

        var myMap = new List<int>(_myValues.Count);
        var index = 0;
        myMap.AddRange(_myValues.Select((_, i) => i >= _myValues.Count - LowerIndex ? index++ : int.MaxValue));

        _myPermutations = new Permutations<int>(myMap);
    }

    public BigInteger Count => Type == GenerateOption.WithoutRepetition
        ? _myPermutations!.Count
        : BigInteger.Pow(UpperIndex, LowerIndex);

    private GenerateOption Type { get; }

    private int UpperIndex => _myValues.Count;

    private int LowerIndex { get; }

    public IEnumerator<IReadOnlyList<T>> GetEnumerator()
    {
        return Type == GenerateOption.WithRepetition
            ? new EnumeratorWithRepetition(this)
            : new EnumeratorWithoutRepetition(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private sealed class EnumeratorWithRepetition(Variations<T> source) : IEnumerator<IReadOnlyList<T>>
    {
        private List<T>? _myCurrentList;

        private List<int>? _myListIndexes;

        void IEnumerator.Reset()
        {
            throw new NotSupportedException();
        }

        public bool MoveNext()
        {
            var carry = 1;
            if (_myListIndexes == null)
            {
                _myListIndexes = new List<int>(source.LowerIndex);
                for (var i = 0; i < source.LowerIndex; ++i) _myListIndexes.Add(0);

                carry = 0;
            }
            else
            {
                for (var i = _myListIndexes.Count - 1; i >= 0 && carry > 0; --i)
                {
                    _myListIndexes[i] += carry;
                    carry = 0;

                    if (_myListIndexes[i] < source.UpperIndex) continue;

                    _myListIndexes[i] = 0;
                    carry = 1;
                }
            }

            _myCurrentList = null;
            return carry != 1;
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
        }

        private void ComputeCurrent()
        {
            if (_myCurrentList != null) return;

            _ = _myListIndexes ??
                throw new InvalidOperationException(
                    $"Cannot call {nameof(Current)} before calling {nameof(MoveNext)}.");

            _myCurrentList = new List<T>(_myListIndexes.Count);
            foreach (var index in _myListIndexes)
                _myCurrentList.Add(source._myValues[index]);
        }
    }

    private sealed class EnumeratorWithoutRepetition : IEnumerator<IReadOnlyList<T>>
    {
        private readonly Variations<T> _myParent;

        private readonly Permutations<int>.Enumerator _myPermutationsEnumerator;

        private List<T>? _myCurrentList;

        public EnumeratorWithoutRepetition(Variations<T> source)
        {
            _myParent = source;
            _myPermutationsEnumerator = (Permutations<int>.Enumerator)_myParent._myPermutations!.GetEnumerator();
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
            if (_myCurrentList != null) return;

            _myCurrentList = new List<T>(_myParent.LowerIndex);
            var index = 0;
            var currentPermutation = _myPermutationsEnumerator.Current;

            for (var i = 0; i < _myParent.LowerIndex; ++i) _myCurrentList.Add(_myParent._myValues[0]);

            foreach (var position in currentPermutation)
                if (position != int.MaxValue)
                {
                    _myCurrentList[position] = _myParent._myValues[index];
                    if (_myParent.Type == GenerateOption.WithoutRepetition) ++index;
                }
                else
                {
                    ++index;
                }
        }
    }
}