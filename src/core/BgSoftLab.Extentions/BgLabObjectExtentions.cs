using System.Text.Json;
using System.Text.Json.Serialization;

namespace BgSoftLab.Extentions;

public static class BgLabObjectExtentions
{
      public static string ToJson<TType>(this object obj) =>
                              JsonSerializer.Serialize<TType>((TType)obj);

}
