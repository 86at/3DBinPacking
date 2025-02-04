﻿namespace Sharp3DBinPacking;

public class Cuboid
{
    public Cuboid()
    {
    }

    public Cuboid(decimal width, decimal height, decimal depth, decimal weight, object tag) :
        this(width, height, depth, 0, 0, 0, weight, tag)
    {
    }

    public Cuboid(decimal width, decimal height, decimal depth, decimal x, decimal y, decimal z) :
        this(width, height, depth, x, y, z, 0, null)
    {
    }

    public Cuboid(decimal width, decimal height, decimal depth, decimal x = 0, decimal y = 0, decimal z = 0, decimal weight = 0,
        object tag = null)
    {
        Width = width;
        Height = height;
        Depth = depth;
        X = x;
        Y = y;
        Z = z;
        Weight = weight;
        Tag = tag;
    }

    public decimal Width { get; set; }
    public decimal Height { get; set; }
    public decimal Depth { get; set; }
    public decimal X { get; set; }
    public decimal Y { get; set; }
    public decimal Z { get; set; }
    public decimal Weight { get; set; }
    public object Tag { get; set; }
    internal bool IsPlaced { get; set; }

    public Cuboid CloneWithoutPlaceInformation()
    {
        return new Cuboid(Width, Height, Depth, 0, 0, 0, Weight, Tag);
    }

    public override string ToString()
    {
        return
            $"Cuboid(X: {X}, Y: {Y}, Z:{Z}, Width: {Width}, Height:{Height}, Depth:{Depth}, Weight: {Weight}, Tag: {Tag})";
    }
}