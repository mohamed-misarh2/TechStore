using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Dtos.ReviewDtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.Application.Services
{
    public class ReviewServices : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewServices(IReviewRepository reviewRepository  ,IMapper mapper) {
           
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        
        }
        public async Task<ResultView<CreateOrUpdateReviewDto>> CreateReview(CreateOrUpdateReviewDto Review)
        {
           
                var review=  _mapper.Map<Review>(Review);
                var NewReview= await _reviewRepository.CreateAsync(review);
                await _reviewRepository.SaveChangesAsync();
                var ReviewDto= _mapper.Map<CreateOrUpdateReviewDto>(NewReview);
                return new ResultView<CreateOrUpdateReviewDto>() { Entity=ReviewDto,IsSuccess=true, Message = "Created Successfully" };

           
        }

        public async Task<ResultDataList<GetAllReviewDto>> GetAllPaginationReview(int items, int pagenumber)
        {
            var AllData = (await _reviewRepository.GetAllAsync()).Include(r=>r.User).Include(r=>r.Product);
            var Reviews = AllData.Where(R => R.IsDeleted == false);
            var allReviews=Reviews.Skip(items * (pagenumber-1)).Take(items).
                Select(r => new GetAllReviewDto
                {
                   Id = r.Id,
                   UserName = r.User.FirstName +" " +r.User.LastName,
                    Comment=r.Comment,
                    ProductName=r.Product.ModelName,
                     Rating=r.Rating,
                     ReviewDate= r.ReviewDate,
                }).ToList();


            ResultDataList<GetAllReviewDto> resultDataList = new ResultDataList<GetAllReviewDto>();
            resultDataList.Entities = allReviews;
            resultDataList.Count = Reviews.Count();
            return resultDataList;
        }

        public async Task<CreateOrUpdateReviewDto> GetOneReview(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            var reviewDto = _mapper.Map<CreateOrUpdateReviewDto>(review);
            return reviewDto;
        }

        public async Task<ResultView<CreateOrUpdateReviewDto>> HardDeleteReview(int reviewId  )
        {

                var review =await _reviewRepository.GetByIdAsync(reviewId);
                var oldreview = await _reviewRepository.DeleteAsync(review);
                await  _reviewRepository.SaveChangesAsync();
                var ReviewDto = _mapper.Map<CreateOrUpdateReviewDto>(oldreview);
                return new ResultView<CreateOrUpdateReviewDto> { Entity = ReviewDto, IsSuccess = true, Message = "Deleted Successfully" };

          
        }

        public async Task<ResultView<CreateOrUpdateReviewDto>> SoftDeleteReview(int reviewId)
        {

           
                var oldreview = (await _reviewRepository.GetAllAsync()).FirstOrDefault(p => p.Id ==  reviewId);
                oldreview.IsDeleted = true;
                 await  _reviewRepository.SaveChangesAsync();
                var reviewDto = _mapper.Map<CreateOrUpdateReviewDto>(oldreview);
                return new ResultView<CreateOrUpdateReviewDto> { Entity =reviewDto , IsSuccess = true, Message = "Deleted Successfully" };

            

        }

        public async Task<ResultView<CreateOrUpdateReviewDto>> UpdateReview(CreateOrUpdateReviewDto Review)
        {
           
                var review = _mapper.Map<Review>(Review);
                var Newreview = await _reviewRepository.UpdateAsync(review);
                await _reviewRepository.SaveChangesAsync();
                var reviewDto = _mapper.Map<CreateOrUpdateReviewDto>(Newreview);
                return new ResultView<CreateOrUpdateReviewDto> { Entity = reviewDto, IsSuccess = true, Message = "Updated Successfully" };

          
        }
           
        }
    }
