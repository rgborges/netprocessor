
using BgSoftLab.Results;
namespace NetProcessor.Data.Importer;

public abstract class BaseImporter
{
      public BaseImporter()
      {

      }
      public Result Test()
      {
            var result = new CreateResult();
            return result.Failure(new string[] { "Error 01" });
      }
}
