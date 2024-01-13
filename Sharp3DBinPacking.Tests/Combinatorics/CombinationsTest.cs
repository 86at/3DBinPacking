using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Sharp3DBinPacking.Combinatorics;

namespace Sharp3DBinPacking.Tests.Combinatorics;

[TestFixture(typeof(List<int>), typeof(int), typeof(GenerateOption))]
[TestOf(typeof(Combinations<int>))]
public class CombinationsTest<T> where T : IList, new()
{
    private IList<int> _list;
    private int _lowerIndex;
    private GenerateOption _generateOption;

    [SetUp]
    public void CreateList()
    {
        _list = new List<int> { 1, 2, 3, 4, 5, 6 };
        _lowerIndex = 2;
        _generateOption = GenerateOption.WithoutRepetition;
    }

    [Test]
    public void GenerateCombinationsOf2WithRepetitionOn6InputItemsShouldCreate21OutputItems()
    {
        var c = new Combinations<int>(_list, _lowerIndex, _generateOption);
        Assert.Equals(21, c.Count);
    }
}