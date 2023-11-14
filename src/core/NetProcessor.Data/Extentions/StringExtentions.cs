namespace NetProcessor.Data;

public static class StringExtentions
{
      public static string FirstCharUpper(this string obj)
      {
            if(obj.Length > 1)
            {
                  return char.ToUpper(obj[0]) + obj.Substring(1, obj.Length);
            }
            return obj;
      }
}
