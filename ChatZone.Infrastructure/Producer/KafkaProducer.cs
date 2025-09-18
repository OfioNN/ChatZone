using ChatZone.Core.Domain.Const;
using ChatZone.Core.Domain.Interfaces.Producer;
using ChatZone.Core.Domain.Options;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatZone.Infrastructure.Producer
{
    public class KafkaProducer : IKafkaProducer {
        private readonly IProducer<string, string> _producer;
        private readonly ILogger<KafkaProducer> _logger;

        public KafkaProducer(IOptions<KafkaOption> options, ILogger<KafkaProducer> logger) {
            var kafkaSetting = options.Value;

            var config = new ConsumerConfig {
                GroupId = GroupKafka.Message,
                BootstrapServers = kafkaSetting.Url,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
            _logger = logger;
        }

        public async Task Produce(string topic, Message<string, string> message) {
            try {
                await _producer.ProduceAsync(topic, message);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error producing message to Kafka");
                throw;
            }

        }
    }
}
