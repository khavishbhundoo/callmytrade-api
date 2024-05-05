# Stage 1: Build environment
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
LABEL org.opencontainers.image.source="https://github.com/khavishbhundoo/callmytrade-api"
ARG BUILD_CONFIGURATION=Release
ARG TARGETARCH
WORKDIR /src
COPY /src .
RUN dotnet restore "CallMyTrade.Api/CallMyTrade.Api.csproj" -a $TARGETARCH && \
    dotnet publish --no-restore -a $TARGETARCH "CallMyTrade.Api/CallMyTrade.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish

# Stage 2: Runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app
COPY --from=build /app/publish .
USER $APP_UID 
ENTRYPOINT ["dotnet", "CallMyTrade.Api.dll"]