namespace NetProcessor.Data.Extentions;

public static class ObjectExtentions
{
      public static Dictionary<string, Type> GetColumnsAndTypes<T>(this object obj, bool forceSmallCase = false)
      {
            try
            {
                  var result = new Dictionary<string, Type>();
                  var type = typeof(T);
                  var properties = type.GetProperties();
                  foreach (var property in properties)
                  {
                        if (forceSmallCase)
                        {
                              result.Add(property.Name.ToLower(), property.PropertyType);
                              continue;
                        }
                        result.Add(property.Name, property.PropertyType);
                  }
                  return result;
            }
            catch
            {
                  throw;
            }
      }
}
