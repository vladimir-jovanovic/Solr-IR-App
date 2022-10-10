namespace SolrIrApp.ExtensionMethods;

public static class ByteUnitString
{
    public enum SizeUnits
    {
        B, KB, MB, GB
    }

    private static string toString(double value, SizeUnits unit)
    {
        return $"{(value).ToString(unit != SizeUnits.B ? "0.00" : "")}{unit.ToString()}";
    }

    public static string toByteUnitString(this long value){
        foreach(SizeUnits s in Enum.GetValues(typeof(SizeUnits))){
            double numOfBytes = Math.Pow(1000, (int)s + 1);
            if(value < numOfBytes) return toString(value/Math.Pow(1000, (int)s), s);
        }

        throw new ArgumentOutOfRangeException("Only up to GB values supported!");
    }
}
