
using ChatZone.Core.Domain;
using ChatZone.Core.Domain.Const;
using ChatZone.Core.Domain.Dtos;
using ChatZone.Core.Domain.Models;
using ChatZone.Core.Domain.Options;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ChatZone.MessageBroker {
    public class KafkaConsumer : BackgroundService {

        private readonly KafkaOption _kafkaOptions;
        private readonly ILogger<KafkaConsumer> _logger;
        private readonly IDbContextFactory<ChatDbContext> _dbContextFactory;

        public KafkaConsumer(IDbContextFactory<ChatDbContext> dbContextFactory, ILogger<KafkaConsumer> logger, IOptions<KafkaOption> options) {
            _kafkaOptions = options.Value;
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            try {
                await Consume(TopicKafka.Message, stoppingToken);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error in Kafka Consumer");
            }
        }

        private async Task Consume(string topic, CancellationToken stoppingToken) {
            var config = CreateConsumerConfig();
            using var consumer = new ConsumerBuilder<string, string>(config).Build();

            consumer.Subscribe(topic);

            _logger.LogInformation($"Kafka Consumer started for topic: {topic}");

            while (!stoppingToken.IsCancellationRequested) {
                try {
                    var consumeResult = consumer.Consume(stoppingToken);
                    await ProcessMessage(consumeResult.Message.Value);
                }
                catch(Exception ex) {
                    _logger.LogError(ex, "Error consuming message");
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }

        private async Task ProcessMessage(string value) {
            var messageDto = JsonSerializer.Deserialize<MessageDto>(value);

            var message = CreateMessage(messageDto);

            await SaveMessageToDatabase(message);
        }

        private async Task SaveMessageToDatabase(Message message) {
            try {
                var dbContext = _dbContextFactory.CreateDbContext();
                await dbContext.Messages.AddAsync(message);
                await dbContext.SaveChangesAsync();
                _logger.LogInformation($"Message {message.MessageId} saved to database");
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error saving message to database");
                throw;
            }
        }

        private Message CreateMessage(MessageDto messageDto) {
            return new Message {
                MessageId = messageDto.MessageId,
                SenderId = messageDto.SenderId,
                MessageText = messageDto.MessageText,
                ChatId = messageDto.ChatId,
            };
        }

        private ConsumerConfig CreateConsumerConfig() {
            return new ConsumerConfig {
                GroupId = GroupKafka.Message,
                BootstrapServers = _kafkaOptions.Url,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
        }
    }
}
