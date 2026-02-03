using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment
{
    internal class SpecialOffer
    {
        public string offerCode { get; set; }
        public string offerDesc { get; set; }
        public SpecialOffer(string OfferCode, string OfferDesc)
        {
            offerCode = OfferCode;
            offerDesc = OfferDesc;
        }
        public override string ToString()
        {
            return $"Offer Code: {offerCode}, Offer Description: {offerDesc}";
        }
    }
}
