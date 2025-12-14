FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY OtelDotnetExample.Api/OtelDotnetExample.Api.csproj OtelDotnetExample.Api/
COPY OtelDotnetExample.Api.Client/OtelDotnetExample.Api.Client.csproj OtelDotnetExample.Api.Client/
RUN dotnet restore OtelDotnetExample.Api/OtelDotnetExample.Api.csproj

COPY . .
RUN dotnet publish OtelDotnetExample.Api/OtelDotnetExample.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://0.0.0.0:5001
EXPOSE 5001
ENTRYPOINT ["dotnet", "OtelDotnetExample.Api.dll"]
