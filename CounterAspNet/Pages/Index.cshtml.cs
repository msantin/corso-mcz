using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StackExchange.Redis;

namespace CounterAspNet.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        var redisHost = Environment.GetEnvironmentVariable("REDIS_HOST");
        var redisPort = Environment.GetEnvironmentVariable("REDIS_PORT");
        var redis = ConnectionMultiplexer.Connect(new ConfigurationOptions
        {
            EndPoints = { $"{redisHost}:{redisPort}" }
        });
        var db = redis.GetDatabase();
        var key = new RedisKey("Counter");
        var counterValue = db.StringGet(new RedisKey("Counter"));
        int counter = 0;
        if (!counterValue.IsNullOrEmpty) {
            counter = ((int)counterValue);
        }        
        counter++;
        ViewData["Counter"] = counter;
        db.StringSet(key, new RedisValue(counter.ToString()));
    }
}
