#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Server/Gejms.Server.csproj", "Server/"]
COPY ["Client/Gejms.Client.csproj", "Client/"]
COPY ["Shared/Gejms.Shared.csproj", "Shared/"]
RUN dotnet restore "Server/Gejms.Server.csproj"
COPY . .
WORKDIR "/src/Server"
RUN dotnet build "Gejms.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gejms.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Gejms.Server.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Gejms.Server.dll