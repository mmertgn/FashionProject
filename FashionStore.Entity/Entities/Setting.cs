using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionStore.Entity.Entities
{
    public class Setting : EntityBase
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string FacebookUrl { get; set; }
        public string GoogleUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string PinterestUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string MapXCoordinate { get; set; }
        public string MapYCoordinate { get; set; }
        public string AboutUs { get; set; }
        public string ConfidentialityAgreement { get; set; }
        public string TermsAgreement { get; set; }
        public string SalesContract { get; set; }
    }
}
