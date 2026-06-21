using Microsoft.AspNetCore.Mvc;
using MedisyncAPI.Models;
using System;
using System.IO;
using System.Net;
using System.Net.Mail; // E-posta fırlatacak akıllı kütüphanemiz

namespace MedisyncAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IletisimController : ControllerBase
    {
        // Dosyanın jüri sunumunda sol menüde, göz önünde oluşması için ana klasörü seçtik
        private readonly string _dbFolder = Directory.GetCurrentDirectory();

        [HttpPost("MesajGonder")]
        public IActionResult MesajGonder([FromBody] IletisimMesaji yeniMesaj)
        {
            if (yeniMesaj == null || string.IsNullOrEmpty(yeniMesaj.AdSoyad) || string.IsNullOrEmpty(yeniMesaj.Email) || string.IsNullOrEmpty(yeniMesaj.Mesaj))
            {
                return BadRequest(new { status = "Hata", message = "Lütfen tüm alanları eksiksiz doldurun." });
            }

            try
            {
                // 1. ADIM: Bilgileri Projenin İçindeki Dosyaya Logluyoruz
                string logPath = Path.Combine(_dbFolder, "IletisimMesajlari.txt");
                string veriSatiri = $"ID: {Guid.NewGuid()}, Ad: {yeniMesaj.AdSoyad}, Email: {yeniMesaj.Email}, Mesaj: {yeniMesaj.Mesaj}, Tarih: {DateTime.Now}\n";
                System.IO.File.AppendAllText(logPath, veriSatiri);

                // 2. ADIM: Güvenli E-Posta Gönderim (SMTP) Ayarları
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587, // Güvenli TLS portu
                    Credentials = new NetworkCredential("cobanmedine804@gmail.com", "wply nsyh lkac stnw"), // Ürettiğin 16 haneli şifreyi mühürledik
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("cobanmedine804@gmail.com", "Medisync İletişim Sistemi"),
                    Subject = $"Yeni İletişim Formu Mesajı: {yeniMesaj.AdSoyad}",
                    Body = $"<h3>Medisync Akıllı Sisteminden Yeni Bildirim</h3>" +
                           $"<p><b>Gönderen Adı:</b> {yeniMesaj.AdSoyad}</p>" +
                           $"<p><b>E-Posta:</b> {yeniMesaj.Email}</p>" +
                           $"<p><b>Mesaj:</b> {yeniMesaj.Mesaj}</p>" +
                           $"<p><b>Sistem Tarihi:</b> {DateTime.Now}</p>",
                    IsBodyHtml = true, // Şık bir HTML e-postası gitmesi için aktif ettik
                };

                // Mesajın anlık düşeceği hedef e-posta kutun
                mailMessage.To.Add("medisync73s@gmail.com"); 

                smtpClient.Send(mailMessage); // Maili gökyüzüne fırlatıyoruz!

                return Ok(new { status = "Basarili", message = "Mesajınız veri tabanına kaydedildi ve e-postanıza başarıyla gönderildi!" });
            }
            catch (Exception ex)
            {
                // Mail gönderiminde teknik aksaklık olursa arayüze detayı fırlatırız
                return StatusCode(500, new { status = "Hata", message = "Sistem çalıştı fakat mail gönderilemedi: " + ex.Message });
            }
        }
    }
}