using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Dtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.Application.Services
{
    public class ReviewServices : IReviewService
    {
        IReviewRepository _reviewRepository;
        private IMapper _mapper;
        public ReviewServices(IReviewRepository reviewRepository  ,IMapper mapper ) {
           
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        
        }
        public async Task<ResultView<CreateOrUpdateReviewDto>> Create(CreateOrUpdateReviewDto createOrUpdateReview)
        {
           var review=  _mapper.Map<Review>(createOrUpdateReview);
            var NewReview= await _reviewRepository.CreateAsync(review);
                           await _reviewRepository.SaveChangesAsync();
            var ReviewDto= _mapper.Map<CreateOrUpdateReviewDto>(NewReview);
            return new ResultView<CreateOrUpdateReviewDto>() { Entity=ReviewDto,IsSuccess=true, Message = "Created Successfully" };

        }

        public async Task<ResultDataList<GetAllReviewDto>> GetAllPagination(int items, int pagenumber)
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
                     ReviewDate=p.ReviewDate,
                   //  IsDeleted=p.IsDeleted,
                 //  UserName=p.User.UserName
                }).ToList();


            ResultDataList<GetAllReviewDto> resultDataList = new ResultDataList<GetAllReviewDto>();
            resultDataList.Entities = Reviews;
            resultDataList.Count = AllData.Count();
            return resultDataList;
        }

        public async Task<GetAllReviewDto> GetOne(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            var reviewDto = _mapper.Map<GetAllReviewDto>(review);
            return reviewDto;
        }

        public async Task<ResultView<CreateOrUpdateReviewDto>> HardDelete(CreateOrUpdateReviewDto createOrUpdateReview)
        {
            try
            {

                var review = _mapper.Map<Review>(createOrUpdateReview);
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

        public async Task<ResultView<CreateOrUpdateReviewDto>> SoftDelete(CreateOrUpdateReviewDto createOrUpdateReview)
        {

            try
            {

                var review = _mapper.Map<Review>(createOrUpdateReview);
                var oldreview = (await _reviewRepository.GetAllAsync()).FirstOrDefault(p => p.Id == createOrUpdateReview.Id);
                oldreview.IsDeleted = true;
               await  _reviewRepository.SaveChangesAsync();
                var reviewDto = _mapper.Map<CreateOrUpdateReviewDto>(oldreview);
                return new ResultView<CreateOrUpdateReviewDto> { Entity =reviewDto , IsSuccess = true, Message = "Deleted Successfully" };

            }
            catch (Exception ex)
            {
                return new ResultView<CreateOrUpdateReviewDto> { Entity = createOrUpdateReview, IsSuccess = false, Message = ex.Message };
            }


        }

        public async Task<ResultView<CreateOrUpdateReviewDto>> Update(CreateOrUpdateReviewDto createOrUpdateReview)
        {
            var oldreview = (await _reviewRepository.GetAllAsync()).Where(p => p.Id == createOrUpdateReview.Id).FirstOrDefault();
            await  _reviewRepository.DeleteAsync(oldreview);
             await  _reviewRepository.SaveChangesAsync();

            if (oldreview == null)
            {
                return new ResultView<CreateOrUpdateReviewDto> { Entity = null, IsSuccess = false, Message = "Already Exist" };

            }
            else
            {

                  var review = _mapper.Map<Review>(createOrUpdateReview);
              
                var Newreview = await _reviewRepository.UpdateAsync(review);
                await _reviewRepository.SaveChangesAsync();
                var reviewDto = _mapper.Map<CreateOrUpdateReviewDto>(Newreview);
                return new ResultView<CreateOrUpdateReviewDto> { Entity = reviewDto, IsSuccess = true, Message = "Created Successfully" };
            }
        }
    }
}
