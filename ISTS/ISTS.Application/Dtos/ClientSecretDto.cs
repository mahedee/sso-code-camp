namespace ISTS.Application.Dtos
{
    public class ClientSecretDto
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Value { get; set; }
        public string Type { get; set; } = "SharedSecret";
        public DateTime? Expiration { get; set; }
        public DateTime? Created { get; set; }
    }
}
