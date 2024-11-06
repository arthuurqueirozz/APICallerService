namespace APICallerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private const int interval = 5000;
        private IHttpClientFactory _httpClientFactory;
        private string _url = "https://jsonplaceholder.typicode.com/users/1/todos";

        public Worker(ILogger<Worker> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Service started at: {time}", DateTimeOffset.Now); 
            

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("CONTACTING API, REQUESTING DATA");
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(_url);

                var targetFilePath = $"C:\\Users\\Arthur\\Desktop\\workerapicalls{Guid.NewGuid()}.txt";
                var responseText = await response.Content.ReadAsStringAsync();
                File.WriteAllText(targetFilePath, responseText);
                
                await Task.Delay(interval, stoppingToken);
            }
        }
    }
}
