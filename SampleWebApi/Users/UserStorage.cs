using System.Collections.Generic;
using System.Linq;

namespace SampleWebApi.Users
{
    public class UserStorage
    {
        private readonly List<UserEntity> _userRepository = new List<UserEntity>();

        public void Add(UserEntity entity) => _userRepository.Add(entity);
        public UserEntity? Find(string id) => _userRepository.FirstOrDefault(x => x.Id == id);

        public IReadOnlyList<UserEntity> GetAll() => _userRepository;

    }

    public class UserEntity
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}