FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5020

ENV ASPNETCORE_Kestrel__Endpoints__Http__Url="http://ws-pc-70:5020"
ENV ASPNETCORE_URLS=http://+:5020
ENV RabbitMq__QueueNamet="auth"
ENV RabbitMq__Credentials__HostName="172.20.0.7"
ENV RabbitMq__Credentials__UserName="user"
ENV RabbitMq__Credentials__Password="password"

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["RabbitMQ.csproj", "./"]


RUN dotnet restore "./RabbitMQ.csproj"

COPY . .

WORKDIR "/src/."

RUN dotnet build "RabbitMQ.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "RabbitMQ.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "RabbitMQ.dll"]

#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
#WORKDIR /app
#EXPOSE 80
#
#FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
#WORKDIR /src
#COPY ["RabbitMQ/RabbitMQ.csproj", "RabbitMQ/"]
#RUN dotnet restore "RabbitMQ/RabbitMQ.csproj"
#COPY . .
#WORKDIR "/src/RabbitMQ"
#RUN dotnet build "RabbitMQ.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "RabbitMQ.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "RabbitMQ.dll"]