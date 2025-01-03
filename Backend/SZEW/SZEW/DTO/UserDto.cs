using SZEW.Models;

namespace SZEW.DTO
{
    public class UserDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required UserType UserType { get; set; }
    }
}
