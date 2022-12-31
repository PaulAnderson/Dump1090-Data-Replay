# Dump1090 Data Replay

This program replays data from a file in the format produced by the Dump1090 ADS-B decoder software. It listens for incoming connections on a specified port, then reads the data from the file one line at a time and sends it to the client. The program extracts the timestamp from each line in the file and calculates the delay between it and the previous line, then sends the line to the client and repeats the process until it reaches the end of the file.

**Note:** The data for this program can be captured by running [Dump1090ConsolePlaneTracker](https://github.com/PaulAnderson/Dump1090ConsolePlaneTracker) with logging enabled.

## Usage

1. Install the .NET Core runtime, if it is not already installed on your system.
2. Clone or download this repository.
3. Open a command prompt in the repository directory.
4. Copy dump1090.txt file into the repository directory.
4. Run the following command:

```
dotnet run
```

## Usage (Containerized)    

1. Install Docker, if it is not already installed on your system.
2. Clone or download this repository.
3. Open a command prompt in the repository directory.
4. Build the Docker image using the following command:

```
docker build -t dump1090-replay .
```
5. Run the container using the following command:

```
docker run --rm -it -p 30003:30003 dump1090-replay
```

This will start the container and map the container's port 30003 to the host's port 30003, allowing you to connect to the program from the host machine.

## Configuration

The following configuration options can be modified in the `Program.cs` file:

- `TCPPort`: The port to listen for incoming connections on.
- `filePath`: The path to the file with the data to replay.
- `SpeedFactor`: The relative speed to replay data. 1 is the realtime speed the data was received. 10 is 10x faster, etc
