
using ChatZone.Core.Domain;
using ChatZone.Core.Domain.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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

        protected override Task ExecuteAsync(CancellationToken stoppingToken) {
            
        }
    }
}
