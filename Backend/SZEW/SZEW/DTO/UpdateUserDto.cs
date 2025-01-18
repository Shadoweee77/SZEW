using SZEW.Models;

namespace SZEW.DTO
{
    public class UpdateUserDto
    {
        public required string Login { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public required UserType UserType { get; set; }
    }
}
