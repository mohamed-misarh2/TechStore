using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Dtos.ReviewDtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.Application.Services
{
    public interface IReviewService
    {
        Task<ResultView<CreateOrUpdateReviewDto>> CreateReview( CreateOrUpdateReviewDto createOrUpdateReview);
        Task<ResultView<CreateOrUpdateReviewDto>> UpdateReview(CreateOrUpdateReviewDto createOrUpdateReview);
        Task<ResultView<CreateOrUpdateReviewDto>> HardDeleteReview(int reviewId );
        Task<ResultView<CreateOrUpdateReviewDto>> SoftDeleteReview(int reviewId);
        Task<ResultDataList<GetAllReviewDto>> GetAllPaginationReview(int items, int pagenumber);

        Task<CreateOrUpdateReviewDto> GetOneReview(int id);


    }
}
