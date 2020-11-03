using System;
using RabbitMQ.Models;
using RabbitMQ.Commands;
using Autofac;
using MediatR;
using System.Reflection;
using AKDbHelpers.Helpers;
using RabbitMQ.Handlers;

namespace RabbitMQ
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces().SingleInstance();

            builder.RegisterAssemblyTypes(typeof(GetStringByIdQuery).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(NewCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            });

        }
    }
}
