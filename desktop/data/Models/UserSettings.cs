using System.ComponentModel;
using Microsoft.IdentityModel.Tokens;

namespace desktop.data.Models
{
    public class UserSettings
    {
        private string? username;
        public string? Username
        {
            get => username;
            set => username = value;
        }
        private string? windowLocation;
        public string? WindowLocation
        {
            get => windowLocation;
            set => windowLocation = value;
        }
        private string? dimensions;
        public string? Dimensions
        {
            get => dimensions;
            set => dimensions = value;
        }
        private string? language;
        public string? Language
        {
            get => language;
            set => language = value;
        }
    }
}
