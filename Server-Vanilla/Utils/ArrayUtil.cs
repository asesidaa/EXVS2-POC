namespace ServerVanilla.Utils;

public static class ArrayUtil
{
    public static uint[] FromString(string content)
    {
        var resultArray = new uint[] { };

        if (content.Trim() != string.Empty)
        {
            resultArray = Array.ConvertAll(content.Trim().Split(','), Convert.ToUInt32);
        }

        return resultArray;
    }
}