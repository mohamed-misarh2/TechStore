using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Dtos.UserDTO;
using TechStore.Dtos.ViewResult;

namespace TechStore.Application.Services
{
    public interface IUserServices
    {
        Task<ResultView<CreateOrUpdateUserDTO>> CreateUser(CreateOrUpdateUserDTO User);
        Task<ResultView<CreateOrUpdateUserDTO>> EditUser(CreateOrUpdateUserDTO User);
        Task<CreateOrUpdateUserDTO> GetByIdUser(int ID);
        Task<ResultDataList<GetAllUserDTO>> GetAllPaginationUser(int items, int pagenumber);

        Task<CreateOrUpdateUserDTO> SearchByNameUser(string Name);//add this function 

        Task<ResultView<bool>> DeleteUser(int UserId);
    }
}
