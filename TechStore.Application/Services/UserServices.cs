using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<CreateOrUpdateUserDTO>_userManager;
        private readonly SignInManager<CreateOrUpdateUserDTO> _signInManager;
        public UserServices(IUserRepository userRepository , IMapper mapper , UserManager<CreateOrUpdateUserDTO> userManager , SignInManager<CreateOrUpdateUserDTO> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

       



        public async Task<CreateOrUpdateUserDTO> GetUserById(int ID)
        {
           var userid = await _userRepository.GetByIdAsync(ID);
            var returnuserDTO =_mapper.Map<CreateOrUpdateUserDTO>(userid);
            return returnuserDTO;
        }

        public async Task<CreateOrUpdateUserDTO> SearchUserByName(string Name) // add function for SearchByName
        {
            var username = await _userRepository.SearchUserByName(Name);
            var returnuserDTO = _mapper.Map<CreateOrUpdateUserDTO>(username);
            return returnuserDTO;
        }

        public async Task<ResultDataList<GetAllUserDTO>> GetAllPaginationUser(int items, int pagenumber) 
        {
            var AlldAta = (await _userRepository.GetAllAsync());
            var UserSer = AlldAta.Skip(items * (pagenumber - 1)).Take(items)
                                              .Select(U => new GetAllUserDTO()
                                              {
                                                  Name = U.FirstName + U.LastName,
                                                  Email = U.Email,
                                                  Address = U.Address
                                                  
                                              }).ToList();
            ResultDataList<GetAllUserDTO> resultDataList = new ResultDataList<GetAllUserDTO>();
            resultDataList.Entities = UserSer;
            resultDataList.Count = AlldAta.Count();
            return resultDataList;
        }

        public async Task<ResultView<bool>> HardDeleteUser(int UserId)
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



        public async Task<ResultView<RegisterDto>> RegisterUser(RegisterDto model)
        {
            var user = new CreateOrUpdateUserDTO {
                                                      UserName = model.UserName,
                                                      Email = model.Email ,
                                                      FirstName = model.FirstName,
                                                      LastName = model.LastName,
                                                      Address = model.Address ,
                                                      image = model.Image,
                                                      PhoneNumber = model.PhoneNumber,
                                                 };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                
                await _signInManager.SignInAsync(user, isPersistent: false);
                return new ResultView<RegisterDto> { Entity = model, IsSuccess = true, Message = "User registered successfully" };
            }
            else
            {
                return new ResultView<RegisterDto> { Entity = model, IsSuccess = false, Message = "User registration failed" };
            }
        }


        public async Task<ResultView<LoginDto>> LoginUser(LoginDto model)
        {
          var user = await _userManager.FindByNameAsync(model.UserName);
            if(user == null)
            {
                return new ResultView<LoginDto> { Entity = null, IsSuccess = false, Message = " UserName Or Password incorrcet" };
            }
            var result = _signInManager.CheckPasswordSignInAsync(user , model.Password, true).Result;
            if (!result.Succeeded)
            {
                return new ResultView<LoginDto> { Entity = null, IsSuccess = false, Message = " UserName Or Password incorrcet" };

            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            return new ResultView<LoginDto> { Entity = model, IsSuccess = true, Message = " Login successfully" };
        }

        public async Task<bool> LogoutUser()
        {
           await _signInManager.SignOutAsync();
            return true;
        }

        public Task<bool> AddRole(string name)
        {
            throw new NotImplementedException();
        }

       
    }
}
