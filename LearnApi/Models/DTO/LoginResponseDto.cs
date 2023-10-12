namespace LearnApi.Models.DTO
{
    public class LoginResponseDto
    {
        public string email { get; set; }
        public string authToken { get; set; }
        public string refreshToken { get; set; }
        public DateTime tokenValidTo { get; set; }
    }
}
