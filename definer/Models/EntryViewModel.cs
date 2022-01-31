using System.ComponentModel.DataAnnotations;

namespace definer.Models
{
    public class EntryViewModel
    {
        [Required(ErrorMessage = "that's not saying much..")]
        [StringLength(int.MaxValue, ErrorMessage = "3 characters minimum.", MinimumLength = 3)]
        public string Body { get; set; }
    }
}
