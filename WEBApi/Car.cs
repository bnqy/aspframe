using System.ComponentModel.DataAnnotations;

namespace WEBApi
{
    public class Car
    {
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
    }
}
