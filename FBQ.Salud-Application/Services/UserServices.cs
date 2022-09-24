
using AutoMapper;
using FBQ.Salud_Domain.Commands;
using FBQ.Salud_Domain.Dtos;
using FBQ.Salud_Domain.Entities;

namespace FBQ.Salud_Application.Services
{
    public interface IUserServices
    {
        List<User> GetAll();
        User GetUserById(int id);
        void Update(User user);
        void Delete(User user);
        User CreateUser(UserDto user);
    }
    public class UsersServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersServices(IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public List<User> GetAll()
        {
            return _userRepository.GetAll();

        }

        public User GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }

        public User CreateUser(UserDto user)
        {
            var userMapped = _mapper.Map<User>(user);
            _userRepository.Add(userMapped);

            return userMapped;
        }

        public void Update(User user)
        {
            _userRepository.Update(user);
        }

        public void Delete(User user)
        {
            _userRepository.Delete(user);
        }
    }
}
