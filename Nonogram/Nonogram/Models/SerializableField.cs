using System.Collections.Generic;

namespace Nonogram.Models;

internal class SerializableField
{
    public SerializableField(List<Cell> cells, List<string> blocksContent, int hintsLeft)
    {
        Cells = cells;
        BlocksContent = blocksContent;
        HintsLeft = hintsLeft;
    }
    
    public List<Cell> Cells { get; set; }
    public List<string> BlocksContent { get; set; }
    public int HintsLeft { get; set; }
}