using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serilog;

namespace RabbitReceiverConsole
{
    public class Notifications :INotification
    {
        public string Message { get; }
        public Notifications(string message)
        {
            Message = message;
        }
    }

    public class Notif1 : INotificationHandler<Notifications>
    {
        public Task Handle(Notifications notification, CancellationToken cancellationToken)
        {
            Log.Information($"Notif1 - {notification.Message}");
            return Task.CompletedTask;
        }
    }

    public class Notif2 : INotificationHandler<Notifications>
    {
        public Task Handle(Notifications notification, CancellationToken cancellationToken)
        {
            Log.Information($"Notif1 - {notification.Message}");
            return Task.CompletedTask;
        }
    }
}
