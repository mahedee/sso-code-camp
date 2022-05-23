namespace ISTS.Application.Dtos
{
    public class ApiResourceDto
    {
        public ApiResourceDto()
        {
            UserClaims = new List<string>();
            Scopes = new List<string>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; } = true;

        public bool ShowInDiscoveryDocument { get; set; }

        public List<string> UserClaims { get; set; }

        public List<string> Scopes { get; set; }
    }
}
