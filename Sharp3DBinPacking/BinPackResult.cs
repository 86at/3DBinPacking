using System.Collections.Generic;

namespace Sharp3DBinPacking;

public class BinPackResult(IList<IList<Cuboid>> bestResult)
{
    public IList<IList<Cuboid>> BestResult { get; private set; } = bestResult;
}