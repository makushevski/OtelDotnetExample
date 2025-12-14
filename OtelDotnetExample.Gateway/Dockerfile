FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY OtelDotnetExample.Gateway/OtelDotnetExample.Gateway.csproj OtelDotnetExample.Gateway/
COPY OtelDotnetExample.Api.Client/OtelDotnetExample.Api.Client.csproj OtelDotnetExample.Api.Client/
RUN dotnet restore OtelDotnetExample.Gateway/OtelDotnetExample.Gateway.csproj

COPY . .
RUN dotnet publish OtelDotnetExample.Gateway/OtelDotnetExample.Gateway.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "OtelDotnetExample.Gateway.dll"]
