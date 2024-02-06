using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Visit
    {
        public int Id { get; set; }
        public int UrlId { get; set; }
        public required string IpAddress { get; set; }
        public DateTime RegDate { get; set; }
    }
}
