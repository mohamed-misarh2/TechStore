using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
            var AllData = await _reviewRepository.GetAllAsync();
            var Reviews = (await _reviewRepository.GetAllAsync()).Skip(items * pagenumber - 1).Take(items).
                Select(p => new GetAllReviewDto
                {
                  Id = p.Id,
                 //   TechUserId = p.TechUserId,
                    Comment=p.Comment,
                  //  ProductId=p.ProductId,
                     Rating=p.Rating,
                     ReviewDate= (DateTime)p.ReviewDate,
                }).ToList();


            ResultDataList<GetAllReviewDto> resultDataList = new ResultDataList<GetAllReviewDto>();
            resultDataList.Entities = Reviews;
            resultDataList.Count = AllData.Count();
            return resultDataList;
        }

        public async Task<CreateOrUpdateReviewDto> GetOneReview(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            var reviewDto = _mapper.Map<CreateOrUpdateReviewDto>(review);
            return reviewDto;
        }

        public async Task<ResultView<CreateOrUpdateReviewDto>> HardDeleteReview(CreateOrUpdateReviewDto Review)
        {
            try
            {

                var review = _mapper.Map<Review>(Review);
                var oldreview = await _reviewRepository.DeleteAsync(review);
                      await  _reviewRepository.SaveChangesAsync();
                var ReviewDto = _mapper.Map<CreateOrUpdateReviewDto>(oldreview);
                return new ResultView<CreateOrUpdateReviewDto> { Entity = ReviewDto, IsSuccess = true, Message = "Deleted Successfully" };

            }
            catch (Exception ex)
            {
                return new ResultView<CreateOrUpdateReviewDto> { Entity = null, IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<ResultView<CreateOrUpdateReviewDto>> SoftDeleteReview(CreateOrUpdateReviewDto Review)
        {

            try
            {

                var review = _mapper.Map<Review>(Review);
                var oldreview = (await _reviewRepository.GetAllAsync()).FirstOrDefault(p => p.Id == Review.Id);
                oldreview.IsDeleted = true;
               await  _reviewRepository.SaveChangesAsync();
                var reviewDto = _mapper.Map<CreateOrUpdateReviewDto>(oldreview);
                return new ResultView<CreateOrUpdateReviewDto> { Entity =reviewDto , IsSuccess = true, Message = "Deleted Successfully" };

            }
            catch (Exception ex)
            {
                return new ResultView<CreateOrUpdateReviewDto> { Entity = Review, IsSuccess = false, Message = ex.Message };
            }


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
