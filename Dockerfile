FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG NUGET_PAT
WORKDIR /src
RUN mkdir FitnessApp.ApiGateway
COPY FitnessApp.ApiGateway ./FitnessApp.ApiGateway
WORKDIR /src/FitnessApp.ApiGateway

RUN dotnet nuget add source https://nuget.pkg.github.com/voldovgan2/index.json --name FitnessApp.Github --username voldovgan2 --password ${NUGET_PAT} --store-password-in-clear-text
RUN dotnet restore "FitnessApp.ApiGateway.csproj"
RUN dotnet build "FitnessApp.ApiGateway.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "FitnessApp.ApiGateway.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=publish /app .
EXPOSE 80 443
ENTRYPOINT ["dotnet", "FitnessApp.ApiGateway.dll"]