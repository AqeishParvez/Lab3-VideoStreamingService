using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieStreamingService.Areas.Identity.Data;
using MovieStreamingService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieStreamingService.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {
        private readonly Amazon.DynamoDBv2.DataModel.DynamoDBContext _dynamoDBContext;
        private readonly UserManager<ApplicationUser> _usermaneger;
        public MovieController(Amazon.DynamoDBv2.DataModel.DynamoDBContext dynamoDBContext, UserManager<ApplicationUser> manager)
        {
            _dynamoDBContext = dynamoDBContext;
            _usermaneger=manager;
        }

        // CRUD actions with composite keys

        // Read
        public async Task<IActionResult> List()
        {        
            var movies = await _dynamoDBContext.ScanAsync<Movie>(new List<ScanCondition>()).GetRemainingAsync();

            return View(movies);
        }

        public async Task<IActionResult> MovieList()
        {
            var movies = await _dynamoDBContext.ScanAsync<Movie>(new List<ScanCondition>()).GetRemainingAsync();

            return View(movies);
        }
        // Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Movie movie)
        {
           
                // Set a unique MovieID
                movie.MovieID = Guid.NewGuid().ToString();
                string userId = _usermaneger.GetUserId(User);
                movie.UserID = userId;
                await _dynamoDBContext.SaveAsync(movie);
                return RedirectToAction("List");
            
        }
        // Update
        [HttpGet("Movie/Edit/{movieId}/{title}")]
        public async Task<IActionResult> Edit(string movieId, string title)
        {
            var movie = await _dynamoDBContext.LoadAsync<Movie>(movieId, title);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpPost("Movie/Edit/{movieId}/{title}")]
        public async Task<IActionResult> Edit(Movie movie)
        {
            if (ModelState.IsValid)
            {
                await _dynamoDBContext.SaveAsync(movie);

                return RedirectToAction("List");
            }

            return View(movie);
        }

        // Delete
        [HttpGet("Movie/Delete/{movieId}/{title}")]
        public async Task<IActionResult> Delete(string movieId, string title)
        {
            var movie = await _dynamoDBContext.LoadAsync<Movie>(movieId, title);

            if (movie == null)
            {
                return NotFound();
            }

            await _dynamoDBContext.DeleteAsync<Movie>(movie);

            return RedirectToAction("List");
        }
    }
}
