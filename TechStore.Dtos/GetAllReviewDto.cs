﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos
{
    public class GetAllReviewDto
    {
       public int Id { get; set; }
      //  public int TechUserId { get; set; }
       // public int ProductId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
      //  public bool? IsDeleted { get; set; }
       
        public string UserName { get; set; }

    }
}
