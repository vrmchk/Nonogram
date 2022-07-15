using System.Collections.Generic;

namespace Nonogram.Models;

internal class SerializableField : IFieldGenerator
{
    public SerializableField(List<Cell> cells, List<int> colorsCounts, int hintsLeft)
    {
        Cells = cells;
        ColorsCounts = colorsCounts;
        HintsLeft = hintsLeft;
    }

    public List<Cell> Cells { get; init; }
    public List<int> ColorsCounts { get; init; }
    public int HintsLeft { get; init; }
}