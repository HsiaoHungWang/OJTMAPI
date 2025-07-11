using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OJTMAPI.Models;
using OJTMAPI.Models.Dto;

namespace OJTMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly ClassDbContext _context;
        private readonly IWebHostEnvironment _host;
        public MembersController(ClassDbContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }

        // GET: api/Members
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberReadDto>>> GetMembers()
        {
             var members = await _context.Members.ToListAsync();

            //把資料放進DTO
            var memberDtos = members.Select(m => new MemberReadDto
            {
                MemberId = m.MemberId,
                Name = m.Name,
                Email = m.Email,
                Age = m.Age,
                FileName = m.FileName
            });
            

            return Ok(memberDtos);
        }

        // GET: api/Members/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberReadDto>> GetMember(int id)
        {
            var member = await _context.Members.FindAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            var memberDto = new MemberReadDto
            {
                MemberId = member.MemberId,
                Name = member.Name,
                Email = member.Email,
                Age = member.Age,
                FileName = member.FileName
            };

            return Ok(memberDto);
        }

        // PUT: api/Members/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMember(int id, MemberEditDto member)
        {
            if (id != member.MemberId)
            {
                return BadRequest();
            }
            var _member = await _context.Members.FindAsync(id);
            if (_member == null) return NotFound();
            _member.Name = member.Name;
            _member.Email = member.Email;
            _member.Age = member.Age;

            _context.Members.Update(_member);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


       

        // POST: api/Members
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
   
        [HttpPost]
        public async Task<IActionResult> PostMember(MemberDto dto)
        {
            //結合路徑及檔案名稱
            string fileName = "";
            using var memoryStream = new MemoryStream();
            if (dto.File != null)
            {
                fileName = dto.File.FileName;
                dto.File.CopyTo(memoryStream);
            }
            string filePath = Path.Combine(_host.WebRootPath, "images", fileName);
            //將檔案存到資料夾中
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                dto.File?.CopyTo(fileStream);
            }
            

            // 將salt轉成字串寫進資料庫中
            byte[] salt = GenerateSalt();

            var member = new Member
            {
                Name = dto.Name,
                Email = dto.Email,
                Age = dto.Age,
                //FileName = dto.File?.FileName,
                FileData = memoryStream.ToArray(),
                FileName = fileName,
                Password = HashPassword(dto.Password!, salt),
                Salt = Convert.ToBase64String(salt),
                Address = dto.Address
            };

            _context.Members.Add(member);
            await _context.SaveChangesAsync();
            //return Ok(member);
            return CreatedAtAction("GetMember", new { id = member.MemberId }, member);

        }
      

        // DELETE: api/Members/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.MemberId == id);
        }

        public static byte[] GenerateSalt(int size = 16)
        {
            //加密安全隨機數生成器。
            return RandomNumberGenerator.GetBytes(size);
        }

        // 使用 PBKDF2 演算法加密密碼
        private static string HashPassword(string password, byte[] salt)
        {
            // 使用 HMACSHA256，迭代 10,000 次，輸出 256-bit (32 bytes) 雜湊值
            byte[] hashed = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                //Pseudo-Random Function，偽隨機函數，用於計算密碼的雜湊值
                prf: KeyDerivationPrf.HMACSHA256, //使用 HMAC-SHA256 作為哈希演算法
                iterationCount: 10000, // 迭代次數       
                numBytesRequested: 256 / 8);  // 產生密鑰的長度 32 bytes(256-bit) 的雜湊值

            return Convert.ToBase64String(hashed);
        }
    }
}
