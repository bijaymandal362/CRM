#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ChangeRequestMangement.UI/ChangeRequestMangement.UI.csproj", "ChangeRequestMangement.UI/"]
COPY ["ChangeRequestManagement.BusinessLayer/ChangeRequestManagement.BusinessLayer.csproj", "ChangeRequestManagement.BusinessLayer/"]
COPY ["ChangeRequestManagement.Entities/ChangeRequestManagement.Entities.csproj", "ChangeRequestManagement.Entities/"]
COPY ["ChangeRequestManagment.Models/ChangeRequestManagment.Models.csproj", "ChangeRequestManagment.Models/"]
RUN dotnet restore "ChangeRequestMangement.UI/ChangeRequestMangement.UI.csproj"
COPY . .
WORKDIR "/src/ChangeRequestMangement.UI"
RUN dotnet build "ChangeRequestMangement.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChangeRequestMangement.UI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChangeRequestMangement.UI.dll"]
