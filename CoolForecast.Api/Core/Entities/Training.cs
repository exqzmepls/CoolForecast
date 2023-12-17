using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoolForecast.Api.Core.Entities;

public sealed class Training
{
    [Key]
    public Guid Id { get; set; }

    public DateTime TimestampUtc { get; set; }

    [Column(TypeName = "oid")]
    public uint DataOid { get; set; }

    public double Accuracy { get; set; }
    public double Recall { get; set; }
    public double F1Score { get; set; }
    public double MeanSquaredError { get; set; }
    public double R2 { get; set; }
    public double AucRoc { get; set; }
    public double LogLoss { get; set; }
}