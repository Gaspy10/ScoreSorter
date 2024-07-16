using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class IntParser
{
    public int Parse(string s)
    {
        try
        {
            return int.Parse(s);
        }
        catch (FormatException)
        {
            throw new FormatException($"Cannot convert '{s}' to an integer");
        }
        catch (OverflowException)
        {
            throw new OverflowException($"'{s}' is outside the range of the Int32 type");
        }
    }
}