using Application.IEventBus;
using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Messages
{
    public class KafkaMessagePublisher : IMessagePublisher
    {
        private readonly IProducer<string, string> _producer;

        // Publisher chỉ cần BootstrapServers, không cần GroupId
        public KafkaMessagePublisher(string bootstrapServers)
        {
            var config = new ProducerConfig { BootstrapServers = bootstrapServers };
            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task PublishAsync<T>(string topic, T message)
        {
            var json = JsonSerializer.Serialize(message);
            await _producer.ProduceAsync(topic, new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(), // Hoặc có thể tạo key dựa trên business logic
                Value = json
            });
        }

        public void Dispose()
        {
            _producer?.Dispose();
        }
    }

    public class KafkaMessageConsumer : IMessageConsumer
    {
        private readonly string _bootstrapServers;

        public KafkaMessageConsumer(string bootstrapServers)
        {
            _bootstrapServers = bootstrapServers;
        }

        public async Task ConsumeAsync(string topic, string groupId, CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                if (string.IsNullOrEmpty(groupId))
                    throw new ArgumentException("GroupId is required for Kafka consumer", nameof(groupId));

                var config = new ConsumerConfig
                {
                    BootstrapServers = _bootstrapServers,
                    GroupId = groupId,
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                using var consumer = new ConsumerBuilder<string, string>(config).Build();
                consumer.Subscribe(topic);

                try
                {
                    var cr = consumer.Consume(cancellationToken); // chỉ 1 lần
                    Console.WriteLine($"[Kafka] Received from {topic} ({groupId}): {cr.Message.Value}");
                }
                finally
                {
                    consumer.Close();
                }
            }, cancellationToken);
        }

    }
}