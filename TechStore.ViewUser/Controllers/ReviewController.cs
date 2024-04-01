using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Security.Claims;
using TechStore.Application.Services;
using TechStore.Dtos.ReviewDtos;
using TechStore.Models;

namespace TechStore.ViewUser.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly UserManager<TechUser> _userManager;

        public ReviewController(IReviewService reviewService  , UserManager<TechUser> userManager)
        {
            _reviewService = reviewService;         
            _userManager = userManager;
        }
        [Authorize]
        [HttpGet]
        public IActionResult AddReview()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>AddReviewAsync(CreateOrUpdateReviewDto review)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
             review.UserId = userId;
             review.ProductId=1;
             var data = await _reviewService.CreateReview(review);
             return View("AddReview" , review);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UpdateReview(int Id)
        {
             var review=await _reviewService.GetOneReview(Id); 
            return View(review);
        }
        [HttpPost]
        public async Task<IActionResult>UpdateReviewAsync(CreateOrUpdateReviewDto review)
        {

            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //review.UserId = userId;
            if (ModelState.IsValid)
            {
                //review.ProductId =7 ;
                var data = await _reviewService.UpdateReview(review);
                return View("UpdateReview", review);

            }

            return View("UpdateReview" , review);
        }
    }
}
