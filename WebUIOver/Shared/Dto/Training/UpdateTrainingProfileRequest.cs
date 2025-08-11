using WebUIOver.Shared.Dto.Request;

namespace WebUIOver.Shared.Dto.Training;

public class UpdateTrainingProfileRequest : BasicCardRequest
{
    public TrainingProfile TrainingProfile { get; set; } = new();
}