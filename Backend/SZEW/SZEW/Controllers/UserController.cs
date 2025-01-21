using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SZEW.Interfaces;
using SZEW.Models;
using SZEW.Repository;
using SZEW.DTO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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

        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<User>))]
        public IActionResult GetAllUsers()
        {
            var users = _mapper.Map<List<UserDto>>(_userRepository.GetAllUsers());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(users);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{userId:int}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUserById(int userId)
        {
            if (!_userRepository.UserExists(userId))
            {
                return NotFound();
            }

            var user = _mapper.Map<UserDto>(_userRepository.GetUserById(userId));

            if (!ModelState.IsValid)
            {
                return BadRequest($"User {userId} is not valid");
            }

            return Ok(user);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{userId}/exists")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        public IActionResult UserExists(int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"User {userId} is not valid");
            }

            if (_userRepository.UserExists(userId))
            {
                return Ok(true);
            }
            else return Ok(false);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{userId}/type")]
        [ProducesResponseType(200, Type = typeof(UserType))]
        [ProducesResponseType(400)]
        public IActionResult GetUserType(int userId)
        {
            if (!ModelState.IsValid || !_userRepository.UserExists(userId))
            {
                return BadRequest($"User {userId} is not valid");
            }

            return Ok(_userRepository.GetUserType(userId));
        }

        [HttpGet("profile")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(401)]
        public IActionResult GetUserProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier); // userId should be stored as NameIdentifier
            if (userIdClaim == null)
            {
                return Unauthorized("User not found in the token.");
            }

            var userId = int.Parse(userIdClaim.Value);

            var user = _mapper.Map<UserDto>(_userRepository.GetUserById(userId));

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] CreateUserDto userCreate)
        {
            if (userCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMap = _mapper.Map<User>(userCreate);

            try
            {
                // Manually set the ID based on the current max ID from the database
                var maxId = _userRepository.GetAllUsers().Select(v => v.Id).DefaultIfEmpty(0).Max();
                userMap.Id = maxId + 1;

                userMap.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userCreate.PlaintextPassword);

                if (!_userRepository.CreateUser(userMap))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException postgresEx && postgresEx.SqlState == "23505")
            {
                ModelState.AddModelError("", "A User with the same ID already exists");
                return StatusCode(409, ModelState);
            }

            return Ok("Successfully created");
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser(int userId, [FromBody] UpdateUserDto updatedUser)
        {
            if (updatedUser == null)
                return BadRequest(ModelState);

            if (!_userRepository.UserExists(userId))
                return NotFound();

            var existingUser = _userRepository.GetUserById(userId);
            if (existingUser == null)
                return NotFound();

            _mapper.Map(updatedUser, existingUser);
            if (userId != existingUser.Id)
                return BadRequest(ModelState);

            if (!_userRepository.UpdateUser(existingUser))
            {
                ModelState.AddModelError("", "Something went wrong updating the User");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(int userId)
        {
            /*
             * Przy usuwaniu usera pojawia się problem odwołań do inncyh obiektów, np ToolRequest.
             * Usuwanie kaskadowe nie jest dobrym rozwiązaniem ze względu na archiwizajcę, tak samo jak ustawiwanie NULLi. 
             * Warto rozważyć opcję zmiany usuwania na dezaktywowanie usera.
            */
            if (!_userRepository.UserExists(userId))
            {
                return NotFound();
            }

            var UserToDelete = _userRepository.GetUserById(userId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userRepository.DeleteUser(UserToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting the User");
            }

            return NoContent();
        }

        [Authorize]
        [HttpPut("{userId}/changepassword")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult ChangePassword(int userId, [FromBody] PasswordUpdateUserDto updatedUser)
        {
            if (updatedUser == null)
                return BadRequest(ModelState);

            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (currentUserIdClaim == null)
                return Unauthorized("User not found in the token.");

            var currentUserId = int.Parse(currentUserIdClaim.Value);

            var currentUser = _userRepository.GetUserById(currentUserId);
            if (currentUser == null)
                return Unauthorized("Current user not found.");

            if (currentUserId != userId && currentUser.UserType != UserType.Admin)
                return Unauthorized("You can only change your own password unless you're an admin.");

            if (!_userRepository.UserExists(userId))
                return NotFound("User not found.");

            var existingUser = _userRepository.GetUserById(userId);
            if (existingUser == null)
                return NotFound("User not found.");

            if (BCrypt.Net.BCrypt.Verify(updatedUser.PlaintextPassword, existingUser.PasswordHash))
            {
                return BadRequest("The new password cannot be the same as the old password.");
            }

            existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updatedUser.PlaintextPassword);

            if (!_userRepository.UpdateUser(existingUser))
            {
                ModelState.AddModelError("", "Something went wrong updating the password.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
