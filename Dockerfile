FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build
WORKDIR /source

COPY *.sln .
COPY Calories/*.csproj ./Calories/
RUN dotnet restore -r linux-musl-x64 ./Calories/Calories.csproj

COPY Calories/. ./Calories/
WORKDIR /source/Calories
RUN dotnet publish -c release -o /app -r linux-musl-x64 --self-contained false --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["./Calories"]
