using OJTMAPI.Models;

namespace OJTMAPI.Models.Dto
{
    public class SpotsPagingDto
    {
        public int TotalPages { get; set; }
       // public int TotalCount { get; set; }
        public List<SpotImagesSpot>? SpotsResult { get; set; }
    }
}
