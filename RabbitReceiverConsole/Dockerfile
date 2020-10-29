FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5021

ENV ASPNETCORE_Kestrel__Endpoints__Http__Url="http://ws-pc-70:5021"
ENV ASPNETCORE_URLS=http://+:5021
ENV RabbitMq__QueueName="auth"
ENV RabbitMq__Credentials__HostName="172.20.0.7"
ENV RabbitMq__Credentials__UserName="user"
ENV RabbitMq__Credentials__Password="password"

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["RabbitReceiverConsole.csproj", "./"]


RUN dotnet restore "./RabbitReceiverConsole.csproj"

COPY . .

WORKDIR "/src/."

RUN dotnet build "RabbitReceiverConsole.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "RabbitReceiverConsole.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "RabbitReceiverConsole.dll"]
