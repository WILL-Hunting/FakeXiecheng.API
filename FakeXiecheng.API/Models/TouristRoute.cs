﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FakeXiecheng.API.Models
{
    public class TouristRoute
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal OriginalPrice { get; set; }
        [Range(0.0, 1.0)]
        public double? DiscountPresent { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpDateTime { get; set; }
        public DateTime? DepartureTime { get; set; }
        [MaxLength]
        public string Features { get; set; }
        [MaxLength]
        public string Fees { get; set; }
        [MaxLength]
        public string Notes { get; set; }
        public ICollection<TouristRoutePicture> TouristRoutePictures { get; set; }
            = new List<TouristRoutePicture>();
        public double? Rating { get; set; }  //评分
        public TravelDays? TravelDays { get; set; }  //enum 枚举 
        public TripType? TripType { get; set; }
        public DepartureCity? DepartureCity { get; set; }

    }
}
