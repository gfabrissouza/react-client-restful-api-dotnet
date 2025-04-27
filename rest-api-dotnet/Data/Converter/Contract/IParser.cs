namespace RestApiDotNet.Data.Converter.Contract
{
    public interface IParser<O, D>
    {
        D Parse(O origin);
        O Parse(D destination);
        List<D> ParseList(List<O> origin);
        List<O> ParseList(List<D> destination);
    }
}
