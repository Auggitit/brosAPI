using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.MASTER.AccountMaster;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using AuggitAPIServer.EmailController;
using Microsoft.Extensions.Caching.Memory;
using System.ServiceModel.Channels;


namespace AuggitAPIServer.Controllers.MASTER.AccountMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class mAdminsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;
        private readonly EmailSenderService _emailSender;
        private readonly IMemoryCache _memoryCache;

        public string security_key = "Auggitan_Key@123456";
        public mAdminsController(AuggitAPIServerContext context, EmailSenderService emailSender, IMemoryCache memoryCache)
        {
            _context = context;
            _emailSender = emailSender;
            _memoryCache = memoryCache;
        }

        [HttpPost("login")]
        public async Task<ActionResult<mAdmin>> Login(LoginRequest loginRequest)
        {
            var user = await _context.mAdmins.SingleOrDefaultAsync(u =>
                (u.email == loginRequest.email) &&
                u.password == Encrypt(loginRequest.Password, security_key));

            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            var token = GenerateJwtToken(user);
            user.token = token;
            await _context.SaveChangesAsync();
            return Ok(new
            {
                code = 200,
                Message = "Password reset instructions sent successfully.",
                data = user
            });
        }

        [HttpPost("logout")]
        public async Task<ActionResult<mAdmin>> Logout(string user_name)
        {
            var user = await _context.mAdmins.SingleOrDefaultAsync(u => (u.user_name == user_name));

            if (user == null)
            {
                return Unauthorized($"Invalid username {user_name}");
            }
            user.token = "";
            await _context.SaveChangesAsync();
            return Ok();
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
            var mAdmin = await _context.mAdmins.FindAsync(id);
            if (mAdmin == null)
            {
                return NotFound();
            }

            _context.mAdmins.Remove(mAdmin);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatemAdmin(Guid id, string password)
        {
            var mAdmin = await _context.mAdmins.FindAsync(id);
            if (mAdmin == null)
                return NotFound(new
                {
                    code = 404,
                    Message = "No Datas Found"
                });
            mAdmin.password = Encrypt(password, security_key);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Code = 200,
                Message = "Password Updated Successfully"
            });
        }
        [HttpPost("sendOtp")]
        public async Task<IActionResult> SendOtp(ForgotPasswordRequest request)
        {
            var user = await _context.mAdmins.SingleOrDefaultAsync(u => u.email == request.email);
            if (user == null)
            {
                return BadRequest(new
                {
                    code = 400,
                    message = "Invalid email address."
                });
            }
            var otp = GenarateRandamnumber().ToString();
            await _emailSender.SendEmail(request.email, otp);
            _memoryCache.Set(request.email, otp, TimeSpan.FromMinutes(5)); // Cache the OTP for 5 minutes
            return Ok(new
            {
                Code = 200,
                Message = "OTP generated and sent successfully."
            });
        }
        [HttpPost("verifyOtp")]
        public async Task<IActionResult> VerifyOtp(ForgotPasswordRequest request)
        {
            var user = await _context.mAdmins.SingleOrDefaultAsync(u => u.email == request.email);
            if (user == null)
            {
                return BadRequest(new
                {
                    code = 400,
                    message = "Invalid email address."
                });
            }
            if (_memoryCache.TryGetValue(request.email, out string storedOtp) && storedOtp == request.otp)
            {
                // OTP is valid
                _memoryCache.Remove(request.email); // Remove the OTP from cache after successful verification
                return Ok(new
                {
                    Code = 200,
                    Message = "OTP verification successful.",
                    data = user
                });
            }
            else
            {
                return BadRequest(new
                {
                    Code = 400,
                    Message = "Invalid OTP."
                });
            }
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
        private string GenerateJwtToken(mAdmin user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(security_key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                    new Claim(ClaimTypes.Name, user.user_name),
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                            SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public string GenarateRandamnumber()
        {
            Random rnd = new Random();
            string number = rnd.Next(100000, 999999).ToString();
            return number;
        }

    }
    public class LoginRequest
    {
        public string email { get; set; }
        public string Password { get; set; }
    }

    public class ForgotPasswordRequest
    {
        public string email { get; set; }
        public string? otp { get; set; }
    }
}
