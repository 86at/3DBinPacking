namespace Sharp3DBinPacking.Internal;

public class Shelf(decimal startY, decimal height, decimal binWidth, decimal binDepth)
{
    public decimal StartY { get; set; } = startY;
    public decimal Height { get; set; } = height;
    public Guillotine2D Guillotine { get; private set; } = new(binWidth, binDepth);

    public override string ToString()
    {
        return $"Shelf(StartY: {StartY}, Height: {Height})";
    }
}