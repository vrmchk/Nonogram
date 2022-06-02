using System.Collections.Generic;

namespace Nonogram.Models;

internal class SerializableField
{
    public SerializableField(List<Cell> cells, List<int> colorsCounts, int hintsLeft)
    {
        Cells = cells;
        ColorsCounts = colorsCounts;
        HintsLeft = hintsLeft;
    }
    
    public List<Cell> Cells { get; set; }
    public List<int> ColorsCounts { get; set; }
    public int HintsLeft { get; set; }
}