
using FitnessApp.ApiGateway.Enums.Settings;

namespace FitnessApp.ApiGateway.Contracts.Settings.Output
{
    public class SettingsContract
    {
        public PrivacyType CanFollow { get; set; }
        public PrivacyType CanViewFollowers { get; set; }
        public PrivacyType CanViewFollowings { get; set; }
        public PrivacyType CanViewFood { get; set; }
        public PrivacyType CanViewExercises { get; set; }
        public PrivacyType CanViewJournal { get; set; }
        public PrivacyType CanViewProgress { get; set; }
    }
}
