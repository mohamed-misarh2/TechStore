using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechStore.Application.Services;
using TechStore.Dtos.ReviewDtos;

namespace TechStore.ViewAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]

    public class ReviewController : ControllerBase
    {
        private readonly  IReviewService _reviewServices;

        public ReviewController(IReviewService reviewServices)
        {
            _reviewServices = reviewServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(int pageItem=10 , int pageNumber=1)
        {
            var data = await _reviewServices.GetAllPaginationReview(pageItem,pageNumber);
            return Ok(data);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int reviewId)
        {
            var data = await _reviewServices.HardDeleteReview(reviewId);
            return Ok(data);
        }
    }
}
