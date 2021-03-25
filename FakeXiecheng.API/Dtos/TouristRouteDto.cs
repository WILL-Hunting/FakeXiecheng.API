using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeXiecheng.API.Models;

namespace FakeXiecheng.API.Dtos
{
    public class TouristRouteDto
    {

        public Guid Id { get; set; }
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
        public ICollection<TouristRoutePictureDto> TouristRoutePictures { get; set; }

    }
}
