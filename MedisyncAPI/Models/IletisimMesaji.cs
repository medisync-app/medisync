using System;

namespace MedisyncAPI.Models
{
    public class IletisimMesaji
    {
        public int Id { get; set; }
public string AdSoyad { get; set; } = string.Empty;
public string Email { get; set; } = string.Empty;
public string Mesaj { get; set; } = string.Empty;
        public DateTime KayitTarihi { get; set; } = DateTime.Now;
    }
}