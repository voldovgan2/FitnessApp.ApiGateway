using FitnessApp.ApiGateway.Enums.Settings;
using Newtonsoft.Json;

namespace FitnessApp.ApiGateway.Contracts.Settings.Output;

public class SettingsContract
{
    [JsonRequired]
    public PrivacyType CanFollow { get; set; }
    [JsonRequired]
    public PrivacyType CanViewFollowers { get; set; }
    [JsonRequired]
    public PrivacyType CanViewFollowings { get; set; }
    [JsonRequired]
    public PrivacyType CanViewFood { get; set; }
    [JsonRequired]
    public PrivacyType CanViewExercises { get; set; }
    [JsonRequired]
    public PrivacyType CanViewJournal { get; set; }
    [JsonRequired]
    public PrivacyType CanViewProgress { get; set; }
}
