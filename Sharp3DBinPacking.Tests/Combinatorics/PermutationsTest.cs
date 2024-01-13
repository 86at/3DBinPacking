using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Sharp3DBinPacking.Combinatorics;

namespace Sharp3DBinPacking.Tests.Combinatorics;

[TestFixture(typeof(List<int>), typeof(GenerateOption))]
[TestOf(typeof(Permutations<int>))]
public class PermutationsTest<T> where T : IList, new()
{
    private IList<int> _list;
    private GenerateOption _generateOption;

    [SetUp]
    public void CreateList()
    {
        _list = new List<int> { 1, 1, 2, 3 };
        _generateOption = GenerateOption.WithRepetition;
    }

    [Test]
    public void GeneratePermutationsWithRepetitionOn4InputItemsIncludingDuplicatesShouldCreate24OutputPermutations()
    {
        var p = new Permutations<int>(_list, _generateOption);
        Assert.Equals(24, p.Count);
    }
}