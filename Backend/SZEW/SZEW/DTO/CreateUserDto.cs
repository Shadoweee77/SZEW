using SZEW.Models;

namespace SZEW.DTO
{
    public class CreateUserDto
    {
        public required string Login { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public required string PlaintextPassword { get; set; }
        public required UserType UserType { get; set; }
    }
}
