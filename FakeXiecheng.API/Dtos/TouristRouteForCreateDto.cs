using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Dtos
{
    public class TouristRouteForCreateDto : IValidatableObject
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage ="title 不可为空")]
        [MaxLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }
        //价格 = 原价 * 折扣
        public decimal Price { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpDateTime { get; set; }
        public DateTime? DepartureTime { get; set; }
        public string Features { get; set; }
        public string Fees { get; set; }
        public string Notes { get; set; }
        public double? Rating { get; set; }  //评分
        public string TravelDays { get; set; }  //enum 枚举 
        public string TripType { get; set; }
        public string DepartureCity { get; set; }
        public ICollection<TouristRoutePictureForCreationDto> TouristRoutePictures { get; set; }
                = new List<TouristRoutePictureForCreationDto>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
