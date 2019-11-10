FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine AS build
WORKDIR /src
COPY ["Meeeeeediator.Api/Meeeeeediator.Api.csproj", "Meeeeeediator.Api/"]
COPY ["Meeeeeediator.Core/Meeeeeediator.Core.csproj", "Meeeeeediator.Core/"]
COPY ["Meeeeeediator.Application/Meeeeeediator.Application.csproj", "Meeeeeediator.Application/"]
RUN dotnet restore "Meeeeeediator.Api/Meeeeeediator.Api.csproj"
COPY . .
WORKDIR "/src/Meeeeeediator.Api"
RUN dotnet build "Meeeeeediator.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Meeeeeediator.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Meeeeeediator.Api.dll"]