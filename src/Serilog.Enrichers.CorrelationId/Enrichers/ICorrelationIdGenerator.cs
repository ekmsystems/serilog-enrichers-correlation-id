namespace Serilog.Enrichers
{
    public interface ICorrelationIdGenerator
    {
        string Generate();
    }
}