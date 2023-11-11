using System;
using System.Threading.Tasks;
using FitnessApp.ApiGateway.Exceptions;
using FitnessApp.ApiGateway.Models.Notification;
using FitnessApp.Common.Serializer.JsonSerializer;
using FitnessApp.Common.ServiceBus.Nats;
using FitnessApp.Common.ServiceBus.Nats.Events;
using FitnessApp.Common.ServiceBus.Nats.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace FitnessApp.ApiGateway.Services.NotificationService
{
    public class NotificationService : INotificationService
    {
        private const int KEY_EXPIRES_SECONDS = 5;

        private readonly IServiceBus _serviceBus;
        private readonly IDistributedCache _distributedCache;
        private readonly IJsonSerializer _serializer;

        public NotificationService(
            IServiceBus serviceBus,
            IDistributedCache distributedCache,
            IJsonSerializer serializer)
        {
            _serviceBus = serviceBus;
            _distributedCache = distributedCache;
            _serializer = serializer;
        }

        public async Task<string> GetNotificationTicket(NotificationTicketModel model)
        {
            var ticket = CreateTicket(model);
            await _distributedCache.SetStringAsync(
                ticket,
                DateTime.UtcNow.AddSeconds(KEY_EXPIRES_SECONDS).Ticks.ToString(),
                new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(KEY_EXPIRES_SECONDS)
                });
            return ticket;
        }

        public Task SendMessage(FollowRequestConfirmed model)
        {
            _serviceBus.PublishEvent(Topic.FOLLOW_REQUEST_CONFIRMED, _serializer.SerializeToBytes(model));
            return Task.CompletedTask;
        }

        public async Task<bool> ValidateNotificationTicket(ValidateNotificationTicketModel model)
        {
            var expirationDateCached = await _distributedCache.GetStringAsync(model.Ticket);
            if (expirationDateCached == null)
                return false;

            var nowTicks = DateTime.UtcNow.Ticks;
            var expirationDateTicks = long.Parse(expirationDateCached);
            if (nowTicks > expirationDateTicks)
                return false;

            var notificationTicketModel = ExtractNotificationTicketModelFromString(model.Ticket);
            return model.Ip == notificationTicketModel.Ip
                && model.UserId == notificationTicketModel.UserId;
        }

        private static string CreateTicket(NotificationTicketModel model)
        {
            return $"{model.Ip}_{model.UserId}";
        }

        private static NotificationTicketModel ExtractNotificationTicketModelFromString(string data)
        {
            var fields = data.Split('_', StringSplitOptions.RemoveEmptyEntries);
            if (fields.Length != 2) throw new InternalUnAuthorizedException("Invalid ticket");
            return new NotificationTicketModel
            {
                Ip = fields[0],
                UserId = fields[1],
            };
        }
    }
}