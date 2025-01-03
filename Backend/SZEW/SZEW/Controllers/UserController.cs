using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SZEW.Interfaces;
using SZEW.Models;
using SZEW.Repository;

namespace SZEW.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<User>))]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetUsers();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(users);
        }

        [HttpGet("{id}/exists")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        public IActionResult ClientExists(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"User {id} is not valid");
            }

            if (_userRepository.UserExists(id))
            {
                return Ok(true);
            }
            else return Ok(false);
        }


        [HttpGet("{id}/type")]
        [ProducesResponseType(200, Type = typeof(UserType))]
        [ProducesResponseType(400)]
        public IActionResult GetClientType(int id)
        {
            if (!ModelState.IsValid || !_userRepository.UserExists(id))
            {
                return BadRequest($"User {id} is not valid");
            }

            return Ok(_userRepository.GetUserType(id));
        }
    }
}
