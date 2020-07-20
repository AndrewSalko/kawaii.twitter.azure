using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace kawaii.twitter.azure.func
{
    /// <summary>
    /// Azure function
    /// </summary>
    public static class TweetPostFunction
    {
        [FunctionName("TweetPostFunction")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo timer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
