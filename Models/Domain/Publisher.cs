using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStorewithCRUD.Models.Domain
{
    public class Publisher
    {
        public int Id { get; set; }
        [Required]
        public string Publishername { get; set; }
    }
}
