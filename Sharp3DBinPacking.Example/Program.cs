﻿using System;

namespace Sharp3DBinPacking.Example;

internal static class Program
{
    private static void Main()
    {
        // Define the size of bin
        const int binWidth = 1000;
        const int binHeight = 1000;
        const int binDepth = 1000;
        // Define the cuboids to pack
        var parameter = new BinPackParameter(binWidth, binHeight, binDepth, new[]
        {
            new Cuboid(150, 100, 150),
            new Cuboid(500, 500, 500),
            new Cuboid(500, 550, 700),
            new Cuboid(350, 350, 350),
            new Cuboid(650, 750, 850)
        });
        // Create a bin packer instance
        // The default bin packer will test all algorithms and try to find the best result
        // BinPackerVerifyOption is used to avoid bugs, it will check whether the result is correct
        var binPacker = BinPacker.GetDefault(BinPackerVerifyOption.BestOnly);
        // The result contains bins which contains packed cuboids with their coordinates
        var result = binPacker.Pack(parameter);
        foreach (var bins in result.BestResult)
        {
            Console.WriteLine("Bin:");
            foreach (var cuboid in bins) Console.WriteLine(cuboid);
        }
    }
}