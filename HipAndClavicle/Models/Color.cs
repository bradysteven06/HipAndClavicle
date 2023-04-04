using NuGet.Packaging.Signing;
using System;
namespace HipAndClavicle.Models;

public class Color
{
    public int ColorId { get; set; }
    public string? ColorName { get; set; }
    public string? HexValue { get; set; }
    [NotMapped]
    public (int, int, int) RGB
    {

        get => (_red, _green, _blue);
        set => (_red, _green, _blue) = value;
    }

    [Range(0, 255)]
    public int _red  = 0;
    [Range(0, 255)]
    public int _blue = 0;
    [Range(0, 255)]
    public int _green = 0;

}

