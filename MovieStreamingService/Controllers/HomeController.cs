using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieStreamingService.Areas.Identity.Data;
using MovieStreamingService.DTO;
using MovieStreamingService.Models;
using System.Diagnostics;

namespace MovieStreamingService.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Amazon.DynamoDBv2.DataModel.DynamoDBContext _dynamoDBContext;
        private readonly UserManager<ApplicationUser> _usermanager;
        public HomeController(Amazon.DynamoDBv2.DataModel.DynamoDBContext dynamoDBContext, ILogger<HomeController> logger, UserManager<ApplicationUser> usermanager)
        {
            _dynamoDBContext = dynamoDBContext;
            _logger = logger;
            _usermanager = usermanager;
        }      
     
        public async Task<IActionResult> Index(string? categoryId, string ReviewSelect)
        {
            if (categoryId != null)
            {
                var scanConditions = new List<ScanCondition>

                 {
                 new ScanCondition("Genre", ScanOperator.Equal, categoryId)
                 };

                var movies = await _dynamoDBContext.ScanAsync<Movie>(scanConditions).GetRemainingAsync();
                return View(movies);
            }
            else
            {
                var movies = await _dynamoDBContext.ScanAsync<Movie>(new List<ScanCondition>()).GetRemainingAsync();

                return View(movies);
            }
           
        }     
        public async Task<IActionResult> MovieDetail(string movieId, string title)
        {
            var movie = await _dynamoDBContext.LoadAsync<Movie>(movieId, title);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SaveReview(ReviewSubmitDTO reviewSubmitDTO)
        {
            if (reviewSubmitDTO != null)
            {
                string userid =  _usermanager.GetUserId(User);
                var movie = await _dynamoDBContext.LoadAsync<Movie>(reviewSubmitDTO.movieId, reviewSubmitDTO.title);
                if (movie.Ratings == null)
                {
                    movie.Ratings = new List<int>();
                }
                movie.Ratings.Add(int.Parse(reviewSubmitDTO.rating));

                if (movie.Comments == null)
                {
                    movie.Comments = new List<string>();
                }
                movie.Comments.Add(reviewSubmitDTO.review);
              

                var newComment = new Comment
                {
                    UserId =userid ,
                    Username = userid,
                    CommentText = reviewSubmitDTO.review,
                    Date = DateTime.Now 
                };

                if (movie.Comment == null)
                {
                    movie.Comment = new List<Comment>();
                }
                movie.Comment.Add(newComment);
                await _dynamoDBContext.SaveAsync(movie);
                return Ok();

            }
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> EditReviewC(ReviewSubmitDTO reviewSubmitDTO)
        {
            if (reviewSubmitDTO != null)
            {
                string userid = _usermanager.GetUserId(User);
                var movie = await _dynamoDBContext.LoadAsync<Movie>(reviewSubmitDTO.movieId, reviewSubmitDTO.title);
               
                if (movie.Comments == null)
                {
                    movie.Comments = new List<string>();
                }
                movie.Comments.Add(reviewSubmitDTO.review);


                var newComment = new Comment
                {
                    UserId = userid,
                    Username = userid,
                    CommentText = reviewSubmitDTO.review,
                    Date = DateTime.Now
                };

                if (movie.Comment == null)
                {
                    movie.Comment = new List<Comment>();
                }
                movie.Comment.Add(newComment);
                await _dynamoDBContext.SaveAsync(movie);
                return Ok();

            }
            return Ok();
        }


        public async Task<IActionResult> UserReview()
        {
            var movies = await _dynamoDBContext.ScanAsync<Movie>(new List<ScanCondition>()).GetRemainingAsync();

            return View(movies);
        }

        public async Task<IActionResult> EditReview(string movieId, string title)
        {
            var movie = await _dynamoDBContext.LoadAsync<Movie>(movieId, title);

            if (movie != null)
            {
                string latestComment = movie.Comment.OrderByDescending(c => c.Date).Select(c => c.CommentText).FirstOrDefault();

                ViewBag.message = latestComment;

                return View(movie);
            }
            else
            {
                return NotFound();
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}