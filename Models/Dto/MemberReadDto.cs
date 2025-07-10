namespace OJTMAPI.Models.Dto
{
    public class MemberReadDto
    {
        public int MemberId { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public int? Age { get; set; }

        public string? FileName { get; set; }
    }
}
