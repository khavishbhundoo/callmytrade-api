# Stage 1: Build environment
FROM --platform=${BUILDPLATFORM} mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
ARG BUILDPLATFORM
ARG TARGETPLATFORM
ARG TARGETARCH
RUN echo "I am building on ${BUILDPLATFORM}, building for ${TARGETPLATFORM} runtime targetting ${TARGETARCH} arch"
ARG VERSION=1.0.0
ENV VERSION ${VERSION}
WORKDIR /src
COPY /src .
RUN dotnet restore "CallMyTrade.Api/CallMyTrade.Api.csproj" -a ${TARGETARCH} && \
    dotnet publish --no-restore -a ${TARGETARCH} "CallMyTrade.Api/CallMyTrade.Api.csproj" -c ${BUILD_CONFIGURATION} -o /app/publish -p:Version=${VERSION}

# Stage 2: Runtime environment
FROM --platform=${TARGETPLATFORM} mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime
ARG BUILDPLATFORM
ARG TARGETPLATFORM
RUN echo "I am running on ${BUILDPLATFORM}, building for ${TARGETPLATFORM} runtime" 
WORKDIR /app
COPY --from=build /app/publish .
USER $APP_UID 
ENTRYPOINT ["dotnet", "CallMyTrade.Api.dll"]