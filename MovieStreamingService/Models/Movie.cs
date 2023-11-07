using Amazon.DynamoDBv2.DataModel;

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
<<<<<<< HEAD
=======
        [DynamoDBProperty("Link")]
        public string UserID { get; set; }

>>>>>>> 6128962 (Initial merge commit)
        [DynamoDBIgnore]
        [DynamoDBProperty("Ratings")]
        public List<int>? Ratings { get; set; }
        [DynamoDBIgnore]
        [DynamoDBProperty("Comments")]
        public List<string>? Comments { get; set; }


    }

}
