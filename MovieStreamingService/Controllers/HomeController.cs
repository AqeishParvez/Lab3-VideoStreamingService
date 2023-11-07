<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieStreamingService.Areas.Identity.Data;
=======
﻿using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieStreamingService.Areas.Identity.Data;
using MovieStreamingService.DTO;
>>>>>>> 6128962 (Initial merge commit)
using MovieStreamingService.Models;
using System.Diagnostics;

namespace MovieStreamingService.Controllers
{
<<<<<<< HEAD
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            this._userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewData["User ID"] = _userManager.GetUserId(this.User);
            return View();
        }

        public IActionResult Privacy()
=======
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Amazon.DynamoDBv2.DataModel.DynamoDBContext _dynamoDBContext;
        public HomeController(Amazon.DynamoDBv2.DataModel.DynamoDBContext dynamoDBContext, ILogger<HomeController> logger)
        {
            _dynamoDBContext = dynamoDBContext;
            _logger = logger;

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
                await _dynamoDBContext.SaveAsync(movie);
                return Ok();

            }
            return Ok();
        }
        public IActionResult UserReview()
>>>>>>> 6128962 (Initial merge commit)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}