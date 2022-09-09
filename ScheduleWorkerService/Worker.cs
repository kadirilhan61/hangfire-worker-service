using Hangfire;

namespace WorkerService1
{

    // https://localhost:5001/hangfire
    public class Worker : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<Worker> _logger;
        private Timer? _timer = null;
        private readonly List<ProcessJobs> jobs;

        public Worker(ILogger<Worker> logger)
        {
            if (jobs == null)
                jobs = ProcessJobs.GetProcessJobs();

            _logger = logger;
        }


        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            //_timer = new Timer(DoWork, null, TimeSpan.Zero,
            //    TimeSpan.FromSeconds(5));
            DoWork();
            return Task.CompletedTask;
        }

        private void DoWork(object? state = null)
        {
            var count = Interlocked.Increment(ref executionCount);

            var backgroundJobs = new BackgroundJobClient();
            backgroundJobs.RetryAttempts = 5;

            var job1 = BackgroundJob.Enqueue<ProcessJobs>(x => x.Test());
            var job2 = BackgroundJob.ContinueJobWith<ProcessJobs>(job1, x => x.Test2("TEST 222"));

            var deleted = jobs.FindAll(a => a.IsDeleted);
            var added = jobs.FindAll(a => a.IsNewItem);


            if (added.Any())
            {
                foreach (var jobId in added)
                    RecurringJob.AddOrUpdate<ProcessJobs>(jobId.JobName, x => x.Test2("TEST 222"), Cron.MinuteInterval(1));
            }

            if (deleted.Any())
            {
                foreach (var jobId in deleted)
                    RecurringJob.RemoveIfExists(jobId.JobName);
            }


            //RecurringJob.AddOrUpdate("seconds", () => Console.WriteLine("Hello, seconds!"), "*/15 * * * * *");
            //RecurringJob.AddOrUpdate("seconds2", () => Console.WriteLine("Hello, world!"), Cron.Minutely);

            _logger.LogInformation(
                "Timed Hosted Service is working. Count: {Count}", count);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}