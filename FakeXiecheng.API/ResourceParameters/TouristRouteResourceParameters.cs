using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FakeXiecheng.API.ResourceParameters
{
    public class TouristRouteResourceParameters
    {
        public string Keyword { get; set; }
        public string operatorType { get; set; }
        public int? raringVlaue { get; set; }
        private string _rating;
        public string Rating {
            get { return _rating; }
            set {
                if (!string.IsNullOrWhiteSpace(value)) {
                    Regex regex = new Regex(@"([A-Za-z0-9\-]+)(\d+)");
                    Match match = regex.Match(value);
                    if (match.Success) {
                        operatorType = match.Groups[1].Value;
                        raringVlaue = Int32.Parse(match.Groups[2].Value);
                    }
                }
                _rating = value;
            }
        }
    }
}
