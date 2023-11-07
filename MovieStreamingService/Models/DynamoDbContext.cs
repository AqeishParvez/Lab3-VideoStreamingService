using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace MovieStreamingService.Models
{

    public class DynamoDBContext
    {
        private readonly IAmazonDynamoDB _dynamoDBClient;
        private readonly Amazon.DynamoDBv2.DataModel.DynamoDBContext _context;

        public DynamoDBContext(IAmazonDynamoDB dynamoDBClient)
        {
            _dynamoDBClient = dynamoDBClient;
            _context = new Amazon.DynamoDBv2.DataModel.DynamoDBContext(dynamoDBClient);
        }

        public async Task<Movie> GetMovieAsync(string movieId)
        {
            return await _context.LoadAsync<Movie>(movieId);
        }

        public async Task SaveMovieAsync(Movie movie)
        {
            await _context.SaveAsync(movie);
        }

        public async Task DeleteMovieAsync(string movieId)
        {
            await _context.DeleteAsync<Movie>(movieId);
        }
    }

}
