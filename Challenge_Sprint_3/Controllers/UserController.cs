using Challenge_Sprint_3.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace Challenge_Sprint_3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly DataContext _context;
        public UserController(DataContext context) 
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        { 
            if(_context.Users.Any(u => u.email == request.email)) 
            {
                return BadRequest("Usuário ja existe"); 
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                email = request.email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Name = request.Name,
                VerficationToken = CreateRandomToken()
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Usuário criado com sucesso!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == request.email);
            if(user == null) 
            {
                return BadRequest("Usuário não encontrado");
            }

            if (!VerifiyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Senha incorreta");
            }

            if (user.VerifiedAt == null) 
            {
                return BadRequest("Não verificado");
            }

            return Ok($"Ola, {user.Name}!");
        }


        [HttpPost("verify")]
        public async Task<IActionResult> Verify(string token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.VerficationToken == token);
            if (user == null)
            {
                return BadRequest("Token invalido");
            }

           user.VerifiedAt = DateTime.Now;
           await _context.SaveChangesAsync();

            return Ok("Usuário verificado!");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == email);
            if (user == null)
            {
                return BadRequest("Usuário não encontrado");
            }

            user.PasswordResetToken = CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddHours(1);
            await _context.SaveChangesAsync();

            return Ok("Pode mudar a senha agora :D");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                return BadRequest("Token invalido");
            }

            CreatePasswordHash(request.Password, out byte[] passwordhash, out byte[] passwordSalt);
            user.PasswordHash = passwordhash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;   

            await _context.SaveChangesAsync();

            return Ok("Senha alterada com sucesso!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("Usuário não encontrado");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("Usuário excluído com sucesso!");
        }

        [HttpPut("{id}/change-name")]
        public async Task<IActionResult> ChangeName(int id, ChangeNameRequest request)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("Usuário não encontrado");
            }

            user.Name = request.NewName;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok("Nome do usuário atualizado com sucesso!");
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) 
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifiyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateRandomToken() 
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}
