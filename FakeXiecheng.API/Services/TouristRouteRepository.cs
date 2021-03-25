using FakeXiecheng.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeXiecheng.API.Database;
using Microsoft.EntityFrameworkCore;

namespace FakeXiecheng.API.Services
{
    public class TouristRouteRepository : ITouristRouteRepository
    {
        //数据库连接器
        private readonly AppDbContext _context;

        public TouristRouteRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TouristRoutePicture> GetPictureByTouristRouteId(Guid touristRouteId)
        {
            return _context.TouristRoutePictures.Where(p => p.TouristRouteId == touristRouteId).ToList();
        }

        public TouristRoute GetTouristRoute(Guid touristRouteId)
        {
            return _context.TouristRoutes.Include(t => t.TouristRoutePictures).FirstOrDefault(n => n.Id == touristRouteId);  //找不到返回null
        }

        public IEnumerable<TouristRoute> GetTouristRoutes(string keyword,string operatorType,int? raringVlaue)
        {
            //return _context.TouristRoutes;
            //include VS join
            //return _context.TouristRoutes.Include(t => t.TouristRoutePictures);
            //生成SQL语句
            IQueryable<TouristRoute> result = _context.TouristRoutes.Include(t => t.TouristRoutePictures);
            
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                result = result.Where(t => t.Title.Contains(keyword));
            }
            if (raringVlaue > 0)
            {
                switch (operatorType)
                {
                    case "largerThan":
                        result = result.Where(t => t.Rating >= raringVlaue);
                        break;
                    case "lessThan":
                        result = result.Where(t => t.Rating <= raringVlaue);
                        break;
                    case "equalTo":
                        result = result.Where(t => t.Rating == raringVlaue);
                        break;
                }
                //result = operatorType switch
                //{
                //    "largerThan" => result.Where(t => t.Rating >= raringVlaue),
                //    "lessThan" => result.Where(t => t.Rating <= raringVlaue,
                //    _ => result.Where(t => t.Rating == raringVlaue)
                //};
            }

            return result.ToList();  //真正执行数据库
        }

        public bool TouristRouteExists(Guid touristRouteId)
        {
            return _context.TouristRoutes.Any(t => t.Id == touristRouteId);  //只要有任何的数据就返回true，否则返回false
        }

        public TouristRoutePicture GetPicture(int pictureId)
        {
            //return _context.TouristRoutePictures.Where(p => p.Id == pictureId).FirstOrDefault();
            return _context.TouristRoutePictures.FirstOrDefault(p => p.Id == pictureId);
        }

        public void AddTouristRoute(TouristRoute touristRoute)
        {
            if (touristRoute == null)
            {
                throw new ArgumentException(nameof(touristRoute));
            }

            _context.TouristRoutes.Add(touristRoute);
            //_context.SaveChanges();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);             
        }

        public void AddTouristRoutePicture(Guid touristrRouteId, TouristRoutePicture touristRoutePicture)
        {
            if (touristrRouteId == Guid.Empty) 
            {
                throw new ArgumentException(nameof(touristrRouteId));
            }
            if (touristRoutePicture == null)
            {
                throw new ArgumentException(nameof(touristRoutePicture));
            }

            touristRoutePicture.TouristRouteId = touristrRouteId;
            _context.TouristRoutePictures.Add(touristRoutePicture);

        }
    }
}
