using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeXiecheng.API.Models;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Mvc;
using FakeXiecheng.API.Dtos;
using AutoMapper;
using System.Text.RegularExpressions;
using FakeXiecheng.API.ResourceParameters;
using Microsoft.AspNetCore.Authorization;

namespace FakeXiecheng.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristRoutesController : ControllerBase
    {
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public TouristRoutesController(ITouristRouteRepository touristRouteRepository,IMapper mapper)
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }
        
        /// <summary>
        /// 获取旅游路线
        /// rating 小于lessThan，大于largerThan,等于equalTo
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="rating"></param>
        /// <returns></returns>
        [HttpGet]
        [HttpHead]
        public IActionResult GerTouristRoutes(
            [FromQuery] TouristRouteResourceParameters parameters
            //[FromQuery] string keyword,
            //string rating
        )
        {
            
            var touristRouteFromRep = _touristRouteRepository.GetTouristRoutes(parameters.Keyword, parameters.operatorType, parameters.raringVlaue);
            if (touristRouteFromRep == null || touristRouteFromRep.Count() < 0 )
            {
                return NotFound("没有旅游路线");
            }

            var touristRoutesDto = _mapper.Map<IEnumerable<TouristRouteDto>>(touristRouteFromRep);

            return Ok(touristRoutesDto);
        }  //GetTouristRoute()

        //api/touristRoutes/{touristRouteId}
        [HttpGet("{touristRouteId}")]
        [Authorize]
        public IActionResult GetTouristRouteById(Guid touristRouteId)
        {
            var touristRouteFromRep = _touristRouteRepository.GetTouristRoute(touristRouteId);
            if (touristRouteFromRep == null)
            {
                return NotFound($"旅游路线{touristRouteId}未找到");
            }

            //DTO手动映射
/*            var touristRouteDto = new TouristRouteDto()
            {
                Id = touristRouteFromRep.Id,
                Title = touristRouteFromRep.Title,
                Description = touristRouteFromRep.Description,
                Price = touristRouteFromRep.OriginalPrice * (decimal)(touristRouteFromRep.DiscountPresent ?? 1),
                CreateTime = touristRouteFromRep.CreateTime,
                UpDateTime = touristRouteFromRep.UpDateTime,
                Features = touristRouteFromRep.Features,
                Fees = touristRouteFromRep.Fees,
                Notes = touristRouteFromRep.Notes,
                Rating = touristRouteFromRep.Rating,
                TravelDays = touristRouteFromRep.TravelDays.ToString(),
                TripType = touristRouteFromRep.TripType.ToString(),
                DepartureCity = touristRouteFromRep.Description
            };*/

            //DTO自动映射
            var touristRouteDto = _mapper.Map<TouristRouteDto>(touristRouteFromRep);

            return Ok(touristRouteDto);
        }  //GetTouristRouteById()

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateTouristRoute([FromBody] TouristRouteForCreateDto touristRouteForCreateDto)
        {
            var touristRouteModel = _mapper.Map<TouristRoute>(touristRouteForCreateDto);
            _touristRouteRepository.AddTouristRoute(touristRouteModel);
            _touristRouteRepository.Save();
            var touristRouteToReturn = _mapper.Map<TouristRouteDto>(touristRouteModel);
            
            //定义header location，实现RESTFul简单的自我发现功能
            return CreatedAtRoute(
                "GetTouristRouteById",
                new { touristRouteId  = touristRouteToReturn.Id},
                touristRouteToReturn
                );
        }
    }
}
