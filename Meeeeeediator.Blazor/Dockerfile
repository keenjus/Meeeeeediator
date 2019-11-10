FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine AS build
WORKDIR /src
COPY ["Meeeeeediator.Blazor/Meeeeeediator.Blazor.csproj", "Meeeeeediator.Blazor/"]
COPY ["Meeeeeediator.Core/Meeeeeediator.Core.csproj", "Meeeeeediator.Core/"]
COPY ["Meeeeeediator.Application/Meeeeeediator.Application.csproj", "Meeeeeediator.Application/"]
RUN dotnet restore "Meeeeeediator.Blazor/Meeeeeediator.Blazor.csproj"
COPY . .
WORKDIR "/src/Meeeeeediator.Blazor"
RUN dotnet build "Meeeeeediator.Blazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Meeeeeediator.Blazor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Meeeeeediator.Blazor.dll"]