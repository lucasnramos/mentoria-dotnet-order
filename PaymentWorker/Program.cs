using Marraia.Queue;
using Marraia.Queue.Interfaces;
using PaymentWorker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IEventBus>(provider => new EventBus(builder.Configuration.GetSection("RabbitMq:Connection").Value,
                                                        builder.Configuration.GetSection("RabbitMq:ExchangeName").Value,
                                                        "direct"));
builder.Services.AddSingleton((provider) =>
{
    var consumer = new Consumer((EventBus)provider.GetService<IEventBus>()!);
    consumer.Subscribe<OrderMessage>(builder.Configuration.GetSection("RabbitMq:QueuePayment").Value,
                                            builder.Configuration.GetSection("RabbitMq:RoutingKey").Value);
    return consumer;
});

var host = builder.Build();
host.Run();
