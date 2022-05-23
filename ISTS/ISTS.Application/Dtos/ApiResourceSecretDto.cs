namespace ISTS.Application.Dtos
{
    public class ApiResourceSecretDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string SecretValue { get; set; }
        public DateTime? Expiration { get; set; }
        public DateTime? Created { get; set; }
    }
}
