using RabbitMQ.Client;

namespace Backend;

public class RabbitmqHostedService : IHostedService
{

  ConnectionFactory? factory;
  IConnection? conn;
  ILogger<RabbitmqHostedService> log;
  IChannel? producerChannel = null;
  IChannel? consumerChannel = null;

  public RabbitmqHostedService(ILogger<RabbitmqHostedService> logger)
  {
    log = logger;
  }



  public async Task StartAsync(CancellationToken cancellationToken)
  {
    factory = new ConnectionFactory();
    factory.UserName = "guest";
    factory.Password = "guest";
    factory.VirtualHost = "/";
    factory.HostName = "rabbitmq";

    conn = await factory.CreateConnectionAsync();

    producerChannel = await conn.CreateChannelAsync();
    consumerChannel = await conn.CreateChannelAsync();

    log.LogInformation("Connected to rabbitmq");
  }

  public async Task StopAsync(CancellationToken cancellationToken)
  {
    log.LogInformation("In stop async...");
    if (producerChannel != null)
    {
      await producerChannel.CloseAsync();
      await producerChannel.DisposeAsync();
    }

    if(consumerChannel != null)
    {
      await consumerChannel.CloseAsync();
      await consumerChannel.DisposeAsync();
    }

    if(conn != null)
    {
      await conn.CloseAsync();
      await conn.DisposeAsync();
    }
      throw new NotImplementedException();
  }
}
