using Marraia.Queue;
using Marraia.Queue.Interfaces;
using PaymentWorker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IEventBus>(provider => new EventBus(builder.Configuration.GetSection("RabbitMq:Connection").Value,
                                                        builder.Configuration.GetSection("RabbitMq:ExchangeName").Value,
                                                        "direct"));


var host = builder.Build();
host.Run();
