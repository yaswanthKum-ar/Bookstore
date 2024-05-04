using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStorewithCRUD.Models.Domain
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Isbn { get; set; }
        [Required]
        public int Totalpages { get; set; }
        [Required]
        public int Authorid { get; set; }
        [Required]
        public int Publisherid { get; set; }
        [Required]
        public int Genreid { get; set; }
        [NotMapped]
        public string Authorname { get; set; }
        [NotMapped]
        public string Publishername { get; set; }
        [NotMapped]
        public string Genrename { get; set; }
        [NotMapped]
        public List<SelectListItem> AuthorList { get; set; }
        [NotMapped]
        public List<SelectListItem> BookList { get; set; }
        [NotMapped]
        public List<SelectListItem> GenreList { get; set; }
        [NotMapped]
        public List<SelectListItem> PublisherList { get; set; }

        
    }
}
