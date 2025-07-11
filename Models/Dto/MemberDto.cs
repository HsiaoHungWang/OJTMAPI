namespace OJTMAPI.Models.Dto
{
    public class MemberDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int? Age { get; set; }
        public IFormFile? File { get; set; }      // 上傳檔案使用
        public string? Password { get; set; }     // 新增或更新密碼
        public string? Address { get; set; }

    }
}
