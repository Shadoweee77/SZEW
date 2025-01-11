using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SZEW.Interfaces;
using SZEW.Models;
using SZEW.Repository;
using SZEW.DTO;
using AutoMapper;

namespace SZEW.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            this._userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<User>))]
        public IActionResult GetUsers()
        {
            var users = _mapper.Map<List<UserDto>>(_userRepository.GetUsers());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(users);
        }

        [HttpGet("{id}/exists")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        public IActionResult UserExists(int id)
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
        public IActionResult GetUserType(int id)
        {
            if (!ModelState.IsValid || !_userRepository.UserExists(id))
            {
                return BadRequest($"User {id} is not valid");
            }

            return Ok(_userRepository.GetUserType(id));
        }

        [HttpGet("profile")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(401)]
        public IActionResult GetProfile()
        {
            // Extract the user ID from the JWT token claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier); // userId should be stored as NameIdentifier
            if (userIdClaim == null)
            {
                return Unauthorized("User not found in the token.");
            }

            var userId = int.Parse(userIdClaim.Value);

            var user = _userRepository.GetUserById(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }
    }
}
