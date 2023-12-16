using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoolForecast.Api.Core.Entities;

public sealed class Forecast
{
    [Key]
    public Guid Id { get; set; }

    public DateTime DataUploadTimestampUtc { get; set; }

    [Column(TypeName = "oid")]
    public uint DataOid { get; set; }
}