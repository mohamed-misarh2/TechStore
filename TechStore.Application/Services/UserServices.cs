using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Dtos.AccountDtos;
using TechStore.Dtos.UserDTO;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.Application.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserServices(IUserRepository userRepository , IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;

        }
        public async Task<ResultView<CreateOrUpdateUserDTO>> CreateUser(CreateOrUpdateUserDTO User)
        {
            var Query = (await _userRepository.GetAllAsync());
            var olduser = Query.Where(u=>u.UserName == User.UserName).FirstOrDefault();
            if (olduser != null) 
            {
                return new ResultView<CreateOrUpdateUserDTO> { Entity = null, IsSuccess = false, Message = "Already Exist" };
            }
            else
            {
                var user = _mapper.Map<TechUser>(User);
                var newUser = await _userRepository.CreateAsync(user);
                await _userRepository.SaveChangesAsync();
                var userDTO = _mapper.Map<CreateOrUpdateUserDTO>(newUser);
                return new ResultView<CreateOrUpdateUserDTO> { Entity = userDTO, IsSuccess = true, Message = "Created Successfully" };
            }
        }

       

        public async Task<ResultView<CreateOrUpdateUserDTO>> EditUser(CreateOrUpdateUserDTO User)
        {
            var exisUser = await _userRepository.GetByIdAsync(User.Id);
            if (exisUser != null)
            {
                _mapper.Map(User, exisUser);
                await _userRepository.UpdateAsync(exisUser);
                await _userRepository.SaveChangesAsync();
                var userDTO = _mapper.Map<CreateOrUpdateUserDTO>(exisUser);
                return new ResultView<CreateOrUpdateUserDTO> { Entity = userDTO, IsSuccess = true, Message = "User Updated Successfully" };
            }
            else
            {
                return new ResultView<CreateOrUpdateUserDTO> { Entity = null, IsSuccess = false, Message = "User Not Found" };
            }
        }

       



        public async Task<CreateOrUpdateUserDTO> GetByIdUser(int ID)
        {
           var userid = await _userRepository.GetByIdAsync(ID);
            var returnuserDTO =_mapper.Map<CreateOrUpdateUserDTO>(userid);
            return returnuserDTO;
        }

        public async Task<CreateOrUpdateUserDTO> SearchByNameUser(string Name) // add function for SearchByName
        {
            var username = await _userRepository.SearchByNameUser(Name);
            var returnuserDTO = _mapper.Map<CreateOrUpdateUserDTO>(username);
            return returnuserDTO;
        }

        public async Task<ResultDataList<GetAllUserDTO>> GetAllPaginationUser(int items, int pagenumber) 
        {
            var AlldAta = (await _userRepository.GetAllAsync());
            var UserSer = AlldAta.Skip(items * (pagenumber - 1)).Take(items)
                                              .Select(U => new GetAllUserDTO()
                                              {
                                                  Name = U.FName + U.LName,
                                                  Email = U.Email,
                                                  Address = U.Address
                                                  
                                              }).ToList();
            ResultDataList<GetAllUserDTO> resultDataList = new ResultDataList<GetAllUserDTO>();
            resultDataList.Entities = UserSer;
            resultDataList.Count = AlldAta.Count();
            return resultDataList;
        }

        public async Task<ResultView<bool>> DeleteUser(int UserId)
        {
            var exisUser = await _userRepository.GetByIdAsync(UserId);

            if (exisUser != null)
            {
                return new ResultView<bool> { Entity = false, IsSuccess = false, Message = "User Not Found" };
            }
            await _userRepository.DeleteAsync(exisUser);
            await _userRepository.SaveChangesAsync();
            return new ResultView<bool> { Entity = true, IsSuccess = true, Message = "User Deleted" };
        }

        public Task<ResultView<RegisterDto>> RegisterUser(RegisterDto model)
        {
            throw new NotImplementedException();
        }

        public Task<ResultView<LoginDto>> LoginUser(LoginDto model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> LogoutUser()
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddRole(string name)
        {
            throw new NotImplementedException();
        }
    }
}
