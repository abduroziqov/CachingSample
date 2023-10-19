using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CachingSample.Controllers
{
    [ApiController]
    [Route("action")]
    public class CachingController : ControllerBase
    {
        private readonly ILogger<CachingController> _logger;
        private readonly IMemoryCache _memoryCache;

        public CachingController(ILogger<CachingController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        [HttpGet(Name = "Get")]
        public IActionResult Get()
        {
            string cacheString = "Hello";
            string testString = "Bye";

            var option = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(10),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10),
                Priority = CacheItemPriority.Low,
                Size = 1024,
                SlidingExpiration = TimeSpan.FromSeconds(10),
            };

            _memoryCache.Set(cacheString, testString, option);

            string? firstResult = _memoryCache.Get(cacheString)?.ToString();

            Thread.Sleep(20000);

            string? result = _memoryCache.Get(cacheString)?.ToString();
            string secondResult = result == null ? "Ended" : result; 

            _memoryCache.Remove(cacheString);

            return Ok($"1 = {firstResult}, 2 = {secondResult}");
        }
    }
}