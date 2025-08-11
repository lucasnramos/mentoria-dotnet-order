using Marraia.Queue;

namespace PaymentWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private IDisposable _disposable;
    private readonly Consumer _consumer;

    public Worker(ILogger<Worker> logger, Consumer consumer)
    {
        _logger = logger;
        _consumer = consumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _disposable = _consumer.Start<OrderMessage>("messagebus.payment", async (message) =>
        {
            _logger.LogInformation("Received OrderMessage: {OrderId}, Customer: {CustomerName}, Email: {CustomerEmail}, Total: {TotalAmount}",
                message.OrderId, message.CustomerName, message.CustomerEmail, message.TotalAmount);
            // Here you can add logic to process the order message
        });
    }
}
