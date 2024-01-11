using System.Collections.Generic;

namespace Sharp3DBinPacking;

public interface IBinPackAlgorithm
{
    void Insert(IEnumerable<Cuboid> cuboids);
}