using RabbitMQ.Client;
using System;

namespace GRC.BuildingBlocks.EventBus.RabbitMQ;

public interface IRabbitMQPersistentConnection : IDisposable
{
    bool IsConnected { get; }
    bool TryConnect();
    IModel CreateModel();
}