FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Install required dependencies
RUN apt-get update \
 && apt-get install -y \
    telnet \
 && rm -rf /var/lib/apt/lists/*

# Copy the source code and restore the dependencies
COPY . /app

WORKDIR /app
RUN dotnet restore

# Build the program
RUN dotnet build -c Release -o /app/out

# Create the final image
FROM mcr.microsoft.com/dotnet/runtime:7.0 AS runtime

# Install required dependencies
RUN apt-get update \
 && apt-get install -y \
    telnet \
 && rm -rf /var/lib/apt/lists/*

# Copy the built program and set it as the entrypoint
COPY --from=build /app/out /app
COPY --from=build /app/dump1090.txt /app
WORKDIR /app
ENTRYPOINT ["dotnet", "Dump1090DataReplay.dll"]
