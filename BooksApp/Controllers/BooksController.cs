using System;
using BooksApp.Dto;
using BooksApp.Infrastructure;
using BooksApp.Queries;
using BooksApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BooksApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;

        private readonly IBookQueries _bookQueries;
        private readonly IBookService _bookService;
        
        public BooksController(ILogger<BooksController> logger, IBookQueries bookQueries, IBookService bookService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _bookQueries = bookQueries ?? throw new ArgumentNullException(nameof(bookQueries));
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        }
        
        [HttpGet]
        public IActionResult Get([FromQuery]BooksSearchModel searchModel)
        {
            return Ok(_bookQueries.GetAll(searchModel));
        }

        [HttpPut]
        public IActionResult Update(long id, [FromBody]BookUpdateDto update)
        {
            return Ok(_bookService.Update(id, update));
        }

        [HttpGet("history")]
        public IActionResult History(long id)
        {
            return Ok(_bookQueries.GetHistory(id));
        }
    }
}
