using System.Collections.Generic;

namespace Nonogram.Models;

internal interface IFieldGenerator
{
    public List<Cell> Cells { get; }
    public List<int> ColorsCounts { get; }
}