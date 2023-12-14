namespace CoolForecast.Api.Core.Entities;

public sealed class Source
{
    public Guid Id { get; set; }
    public DateTime TimestampUtc { get; set; }
    public uint DataOid { get; set; }
}