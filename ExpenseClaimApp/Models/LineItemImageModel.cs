using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Models
{
    public class LineItemImageModel
    {
        public int Id { get; set; }
        public List<string> ImageDataUrls { get; set; }
    }
}
