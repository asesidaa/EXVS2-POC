using System.Collections.ObjectModel;
using WebUIOver.Shared.Dto.Common;
using WebUIOVer.Shared.Dto.Common;
using WebUIOver.Shared.Dto.Group;
using WebUIOver.Shared.Dto.Message;
using WebUIOver.Shared.Dto.Player;
using WebUIOver.Shared.Dto.Response;
using WebUIOVer.Shared.Dto.Response;
using WebUIOver.Shared.Dto.Training;

namespace WebUIOver.Client.Context.CustomizeCard;

public class CustomizeCardContext
{
    public string AccessCode { get; set; } = string.Empty;
    public string ChipId { get; set; } = string.Empty;
    public BasicProfile BasicProfile { get; set; } = new();
    public PlayerLevelProfile PlayerLevelProfile { get; set; } = new();
    public NaviProfile NaviProfile { get; set; } = new();
    public ObservableCollection<NaviWithNavigatorGroup> NaviObservableCollection { get; set; } = new();
    public List<MsSkillGroup> MsSkillGroups { get; set; } = new();
    public List<MobileSuit> AggregetedMobileSuits { get; set; } = new();
    public List<MobileSuitWithSkillGroup> MobileSuitWithSkillGroups { get; set; } = new();
    public ObservableCollection<FavouriteMs> FavouriteMsCollection = new();
    public ObservableCollection<MobileSuitWithSkillGroup> AlternativeCostumeMobileSuitsSkillGroups = new();
    public ObservableCollection<MobileSuitWithSkillGroup> AlternativeSkinMobileSuitsSkillGroups = new();
    public CustomMessageGroupSetting CustomMessageGroupSetting { get; set; }= new();
    public StickerDto DefaultStickerSetting { get; set; } = new();
    public List<StickerDto> MobileSuitStickerSettings { get; set; } = new();
    public uint SelectedStickerMs { get; set; } = 1;
    public CpuTriadPartner CpuTriadPartner { get; set; } = new ();
    public TeamResponse TeamResponse { get; set; } = new ();
    public GamepadConfig GamepadConfig { get; set; } = new ();
    public TrainingProfile TrainingProfile { get; set; } = new ();
}