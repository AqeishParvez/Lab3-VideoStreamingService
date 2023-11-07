using Amazon.DynamoDBv2.DataModel;
using MovieStreamingService.DTO;

namespace MovieStreamingService.Models
{
    [DynamoDBTable("Movies")]
    public class Movie
    {
        [DynamoDBHashKey] // Primary key
        public string MovieID { get; set; }

        [DynamoDBRangeKey]
        public string Title { get; set; }

        [DynamoDBProperty("Director")]
        public string Director { get; set; }

        [DynamoDBProperty("Link")]
        public string? S3Link { get; set; }

        [DynamoDBProperty("Genre")]
        public string Genre { get; set; }

        [DynamoDBProperty("Year")]
        public int Year { get; set; }

        [DynamoDBProperty("Rating")]
        public int Rating { get; set; }
        [DynamoDBProperty("Link")]
        public string? Link { get; set; }
        [DynamoDBProperty("Link")]
        public string UserID { get; set; }

        [DynamoDBIgnore]
        [DynamoDBProperty("Ratings")]
        public List<int>? Ratings { get; set; }
        [DynamoDBIgnore]
        [DynamoDBProperty("Comments")]
        public List<string>? Comments { get; set; }
        public List<Comment> Comment { get; set; }


    }

}
