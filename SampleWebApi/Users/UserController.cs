using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace SampleWebApi.Users
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserStorage _userStorage;

        public UserController(UserStorage userStorage)
        {
            _userStorage = userStorage;
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            if (_userStorage.Find(id) is { } foundUser)
            {
                return Ok(foundUser);
            }
            return NotFound();
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            var allUsers = _userStorage.GetAll().Select(x => new UserDTO
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName
            });
            return Ok(allUsers);
        }

        [HttpPost]
        public IActionResult Create(CreateUserDTO dto)
        {
            var newId = Guid.NewGuid().ToString();
            _userStorage.Add(new UserEntity
            {
                Id = newId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email
            });

            return new ObjectResult(new EntityCreatedResult(newId))
            {
                StatusCode = (int)HttpStatusCode.Created
            };
        }
    }
}
