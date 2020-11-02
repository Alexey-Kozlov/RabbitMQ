﻿using System;
using RabbitReceiverConsole.Commands;
using Autofac;
using MediatR;
using System.Reflection;

namespace RabbitReceiverConsole
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces().SingleInstance();

            builder.RegisterAssemblyTypes(typeof(GetStringByIdQuery).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            //builder.RegisterAssemblyTypes(typeof(Notifications).GetTypeInfo().Assembly)
            //    .AsClosedTypesOf(typeof(INotification));

            //builder.RegisterType<Notifications>().As<INotification>().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(Notifications)).As(typeof(INotification));

            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            });

        }
    }
}
