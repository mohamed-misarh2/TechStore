﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Security.Claims;
using System.Security.Cryptography;
using TechStore.Application.Services;
using TechStore.Dtos.ReviewDtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;
using TechStore.ViewUser.Models;

namespace TechStore.ViewUser.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly UserManager<TechUser> _userManager;

        public ReviewController(IReviewService reviewService, UserManager<TechUser> userManager)
        {
            _reviewService = reviewService;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult>IndexReview(int id ,int page)
        {
           
            int PageNumber = 1+ page;
            int pageSize = 4;
            try
            {
         
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                 var  review = await _reviewService.GetAllReviewByProduct(pageSize, PageNumber, id);
                if (review.Entities != null && review.Entities.Any())
                {
                    var product = review.Entities.First().ProductId; // Use First() instead of FirstOrDefault() since you've already checked if it's not empty
                    ViewBag.pageSize = pageSize;
                    ViewBag.ProductId = product;
                    ViewBag.PageNumber = PageNumber;
                    ViewBag.UserId = userId;
                    return PartialView(review);
                }
                else
                {
                    // Handle case where there are no entities in the review
                    // For example, return an empty view or a message indicating no reviews found
                    return PartialView(review);
                }
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
           ;
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddReview(string Description, string imgproduct ,int productid)
        {

            string Descript = Description;
            string img = imgproduct;
            int id = productid;
            ViewBag.img = img;
            ViewBag.Descript = Descript;
            ViewBag.productId= id;
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> AddReview(CreateOrUpdateReviewDto review)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
             review.UserId = userId;
            
             var data = await _reviewService.CreateReview(review);
             int _id = data.Entity.ProductId;
             return RedirectToAction("Details", "Product", new {id=_id} );
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

       
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            var review = await _reviewService.GetOneReview(reviewId);
            var data = await _reviewService.SoftDeleteReview(review);
            int _id = data.Entity.ProductId;
            return RedirectToAction("Details", "Product", new { id = _id });
        }

    }
}
