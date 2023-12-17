namespace CoolForecast.Api.Endpoints.Trainings;

public class CreateTrainingParams
{
    public required double Accuracy { get; set; }
    public required double Recall { get; set; }
    public required double F1Score { get; set; }
    public required double MeanSquaredError { get; set; }
    public required double R2 { get; set; }
    public required double AucRoc { get; set; }
    public required double LogLoss { get; set; }
}