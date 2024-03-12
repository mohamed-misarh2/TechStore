using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Dtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.Application.Services
{
    public interface IReviewService
    {
     Task<ResultView<CreateOrUpdateReviewDto>> Create(CreateOrUpdateReviewDto createOrUpdateReview);
    Task<ResultView<CreateOrUpdateReviewDto>> Update(CreateOrUpdateReviewDto createOrUpdateReview);
        Task<ResultView<CreateOrUpdateReviewDto>> HardDelete(CreateOrUpdateReviewDto createOrUpdateReview);
        Task<ResultView<CreateOrUpdateReviewDto>> SoftDelete(CreateOrUpdateReviewDto createOrUpdateReview);
        Task<ResultDataList<GetAllReviewDto>> GetAllPagination(int items, int pagenumber);


        Task<GetAllReviewDto> GetOne(int id);


    }
}
