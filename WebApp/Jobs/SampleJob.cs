using System;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace WebApp.Jobs;

public class SampleJob(ILogger<SampleJob> logger, FacebookService facebook)
{
    private readonly ILogger<SampleJob> _logger = logger;
    private readonly FacebookService _facebook = facebook;

    [AutomaticRetry(Attempts = 3)]
    public async Task Execute()
    {
        try
        {
            var postId = await _facebook.PostMessageToPageAsync($"Sample job executed at {DateTimeOffset.Now}");
            _logger.LogInformation("Posted to Facebook page. Post id: {PostId}", postId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to post to Facebook.");
        }

        _logger.LogInformation("SampleJob executed at {Time}", DateTimeOffset.Now);
    }
}
