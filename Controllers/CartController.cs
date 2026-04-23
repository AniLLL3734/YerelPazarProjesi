using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proje.Data;
using Proje.Models;
using System.Text.Json;

namespace Proje.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var cart = GetCartFromSession();
            var products = await _context.Products
                .Include(p => p.Producer)
                .Where(p => cart.Contains(p.Id))
                .ToListAsync();

            return View(products);
        }

        public IActionResult AddToCart(int id)
        {
            var cart = GetCartFromSession();
            if (!cart.Contains(id))
            {
                cart.Add(id);
                SaveCartToSession(cart);
            }
            return RedirectToAction("Index");
        }

        
        public IActionResult RemoveFromCart(int id)
        {
            var cart = GetCartFromSession();
            if (cart.Contains(id))
            {
                cart.Remove(id);
                SaveCartToSession(cart);
            }
            return RedirectToAction("Index");
        }

        // Siparişi Tamamla
        public IActionResult Checkout()
        {
            // Sepeti boşaltıyoruz
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("CheckoutSuccess");
        }

        // Sipariş Başarılı Sayfası
        public IActionResult CheckoutSuccess()
        {
            return View();
        }

        private List<int> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<int>();
            }
            return JsonSerializer.Deserialize<List<int>>(cartJson) ?? new List<int>();
        }

        private void SaveCartToSession(List<int> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString("Cart", cartJson);
        }
    }
}
