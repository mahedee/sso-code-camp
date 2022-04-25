using IdentityServer4.EntityFramework.Entities;

namespace ISTS.Application.Common.Models
{
    public class ClientDetails
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ClientSecret { get; set; }
        public Client Client { get; set; }
    }
}
