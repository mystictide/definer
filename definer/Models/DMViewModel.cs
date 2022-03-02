using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace definer.Models
{
    public class DMViewModel
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "don't be shy.")]
        public string dmBody { get; set; }
    }
}
