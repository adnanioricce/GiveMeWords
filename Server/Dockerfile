FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
ENV ASPNETCORE_URLS http://+:80
EXPOSE 80
WORKDIR /app
# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -r linux-x64 --self-contained  -o out /p:PublishSingleFile=true /p:PublishTrimmed=true

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT [ "./Server" ]