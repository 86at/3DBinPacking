using System.Collections.Generic;

namespace Sharp3DBinPacking;

public class BinPackParameter(
    decimal binWidth,
    decimal binHeight,
    decimal binDepth,
    decimal binWeight,
    bool allowRotateVertically,
    IEnumerable<Cuboid> cuboids)
{
    public BinPackParameter(
        decimal binWidth, decimal binHeight, decimal binDepth, IEnumerable<Cuboid> cuboids) :
        this(binWidth, binHeight, binDepth, 0, true, cuboids)
    {
    }

    public decimal BinWidth { get; private set; } = binWidth;
    public decimal BinHeight { get; private set; } = binHeight;
    public decimal BinDepth { get; private set; } = binDepth;
    public decimal BinWeight { get; private set; } = binWeight;
    public bool AllowRotateVertically { get; private set; } = allowRotateVertically;
    public IEnumerable<Cuboid> Cuboids { get; private set; } = cuboids;
    public int ShuffleCount { get; set; } = 5;
}