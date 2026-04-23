using System.ComponentModel.DataAnnotations;

namespace Proje.Models
{
    public class Producer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Üretici adı zorunludur.")]
        [Display(Name = "Üretici Adı")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Şehir")]
        public string City { get; set; } = string.Empty;

        [Display(Name = "İletişim")]
        public string ContactInfo { get; set; } = string.Empty;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
