using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HouseFlowPart1.Models
{
    public class HouseImages
    {
        // 'Id' is a unique identifier for an image.
        public ObjectId Id { get; set; }
        // 'HouseId' associates the image with a specific house, linking it to property listings.
        public ObjectId HouseId { get; set; }
        // 'ImageUrl' stores the URL of the image, and it is marked as required and not nullable
        [Required]
        [NotNull]
        public string ImageUrl { get; set; } = "";
    }
}
