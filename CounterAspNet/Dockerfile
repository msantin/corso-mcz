FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 5148

ENV ASPNETCORE_URLS=http://+:5148

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["CounterAspNet/CounterAspNet.csproj", "CounterAspNet/"]
RUN dotnet restore "CounterAspNet/CounterAspNet.csproj"
COPY . .
WORKDIR "/src/CounterAspNet"
RUN dotnet build "CounterAspNet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CounterAspNet.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CounterAspNet.dll"]
