namespace LearnApi.Models.DTO
{
    public class RegisterNewDriverDto
    {
        public string DriverName { get; set; } = null!;
        public int Number { get; set; }
        public string Team { get; set; } = null!;
        public IFormFile profilePicture { get; set; }
    }
}
