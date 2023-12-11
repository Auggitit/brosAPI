using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.MASTER.AccountMaster;
using System.Security.Cryptography;
using System.Text;


namespace AuggitAPIServer.Controllers.MASTER.AccountMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class mAdminsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;
        public string security_key = "Auggitan_Key@123456";
        public mAdminsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/mAdmins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<mAdmin>>> GetmAdmins()
        {
            if (_context.mAdmins == null)
            {
                return NotFound();
            }
            return await _context.mAdmins.ToListAsync();
        }

        // GET: api/mAdmins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<mAdmin>> GetmAdmin(Guid id)
        {
            if (_context.mAdmins == null)
            {
                return NotFound();
            }
            var admin = await _context.mAdmins.FindAsync(id);
            // admin.password = Decrypt(admin.password, security_key);

            if (admin == null)
            {
                return NotFound();
            }

            return admin;
        }

        // POST: api/mAdmins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<mAdmin>> PostmAdmin(mAdmin mAdmin)
        {
            if (_context.mAdmins == null)
            {
                return Problem("Entity set 'AuggitAPIServerContext.mAdmin'  is null.");
            }
            // Check if the mobile number or email already exists
            if (_context.mAdmins.Any(a => a.mobile_no == mAdmin.mobile_no))
            {
                return BadRequest("Mobile number already exists.");
            }

            if (_context.mAdmins.Any(a => a.email == mAdmin.email))
            {
                return BadRequest("Email already exists.");
            }

            if (mAdmin.password != null)
            {
                string encryptedPassword = Encrypt(mAdmin.password, security_key);
                mAdmin.password = encryptedPassword;
                mAdmin.created_at = DateTime.UtcNow;
                mAdmin.updated_at = DateTime.UtcNow;
            }
            _context.mAdmins.Add(mAdmin);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/mAdmins/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletemAdmin(Guid id)
        {
            if (_context.mAdmins == null)
            {
                return NotFound();
            }
            var mAdmin = await _context.mAdmins.FindAsync(id);
            if (mAdmin == null)
            {
                return NotFound();
            }

            _context.mAdmins.Remove(mAdmin);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private string Encrypt(string plainText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                // Ensure the key is the correct size
                aesAlg.Key = Encoding.UTF8.GetBytes(key.PadRight(32, '\0')).Take(32).ToArray();
                aesAlg.IV = new byte[16];

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }
        static string Decrypt(string encryptedPassword, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key.PadRight(32, '\0')).Take(32).ToArray();
                aesAlg.IV = new byte[16];

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedPassword)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
