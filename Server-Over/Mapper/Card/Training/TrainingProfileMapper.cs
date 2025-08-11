using Riok.Mapperly.Abstractions;
using WebUIOver.Shared.Dto.Training;

namespace ServerOver.Mapper.Card.Training;

[Mapper]
public static partial class TrainingProfileMapper
{
    public static partial TrainingProfile ToTrainingProfile(this Models.Cards.Profile.TrainingProfile trainingProfile);
}