namespace NetProcessor.Data.Importer;

public interface ILineParserConverter<TSource, TTarget>
{
      TTarget Convert(TSource source);
}
