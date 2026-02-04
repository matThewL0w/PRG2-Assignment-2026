using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//==========================================================
// Student Number : S10271111E
// Student Name : Lee Hua Jay
// Partner Name : Low Yong Jin Matthew
//==========================================================

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
