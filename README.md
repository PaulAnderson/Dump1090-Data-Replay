# Dump1090-Data-Replay
This program replays data from a file in the format produced by the Dump1090 ADS-B decoder software. It listens for incoming connections on a specified port, then reads the data from the file one line at a time and sends it to the client.  The timestamp is extracted from each line in the file and used to time the output of each line to the client.
