using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Sharp3DBinPacking.Combinatorics;

namespace Sharp3DBinPacking.Tests.Combinatorics;

[TestFixture(typeof(List<int>), typeof(GenerateOption))]
[TestOf(typeof(Variations<int>))]
public class VariationsTest<T> where T : IList, new()
{
    private IList<int> _list;
    private GenerateOption _generateOption;

    [SetUp]
    public void CreateList()
    {
        _list = new List<int> { 1, 2, 3, 4, 5, 6 };
        _generateOption = GenerateOption.WithoutRepetition;
    }

    [Test]
    public void GenerateVariationsOf3WithRepetitionOn6InputItemsShouldCreate216OutputItems()
    {
        var v = new Variations<int>(_list, 3, _generateOption);
        Assert.Equals(216, v.Count);
    }
}