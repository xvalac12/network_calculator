﻿ # IPK Project 1: IPK Calculator Protocol
## Description of the client implementation
The application is implemented in the C# programming language with the **.NET 6.0** framework using libraries from the **base SDK** (NET SDK).  The compilation is done using a **Makefile** (*dotnet clean, build and publish*) and the `make` command (`make OS=win-x64` for windows). It run with `./ipkcpc -h (host) -p (port) -m (mode)`, where host is the IPv4 address or Hostname of the server, to which we want to connect, port is an integer indicating the port to which we want to connect and  mode is either **udp** or **tcp**.  It has been tested to run on Windows 11, Ubuntu 22.04 and NIX operating system.  The client consists of 3 classes: `Client`, `Tcp`, `Udp`.

### Client
In the Client class, the `cla_handling()` method is used to check whether the command line arguments match the specified format.  If an argument does not match, the mode is entered incorrectly, or the port is entered in the wrong format, an error output is sent to standard error output and the application exits with code 1. If everything is OK, the method returns the port number.  It is followed by check whether it was entered as a host IPv4 address or Hostname.  If a Hostname was entered, it will be converted to an IPv4 address.  Subsequently, an `endPoint()` is created by combining the IPv4 address and the port number.  Then it's time to create a socket.  A UDP form socket is created by default.  If entered mode was TCP, the socket is reformatted to TCP form and an attempt to connect to the server follows.  If successful, communication begins.  Than the client reads data from the standard input until the method `Tcp.communication()`  from **Tcp class** returns 1 or interruption occurs (*C_c*).  Subsequently, the reading from standard input ends, the socket is closed and the application ends with code 0.

### Tcp
The Tcp class contains methods for processing the TCP mode.  The `connect()` method tries to connect to the server, and if it fails, prints an error to standard error output and exits the application with code 2. 
The following method `communication()` encodes the message entered by user on standard input and stores it in a byte array.  Subsequently, this message is sent as request and a response is awaited.  When the response arrives, it is encoded to a string and printed to standard output.  Right after, the method checks whether the response is a `BYE` message.  If yes, the method returns a return value of 1, otherwise a return value of 0. 
 The `sigint` method sends a `BYE` request to the server in case of entering an interrupt signal(*C_c*),  receives a response from the server, prints it to standard output and closes the socket.

### Udp
There is one method (`communication()`) in the class.  In this method,  according to the specified format (*see the UDP mode section*), 0 is inserted into the byte field at the first byte of array, followed by a number that indicates the length of the string entered from the standard input  An encoded string to byte follows after, which is a message from standard input. 
Subsequently, the application sends an array of bytes as a request and expects a response from the server.  When the response arrives, it checks the correct format (*see UDP mode section*).  If the wrong format arrives, the application discards the response and moves on to sending the next request.  If the message has the correct format, it determines whether it is a result or an error and prints it correctly to the standard output.  The communication continues until it receives an interrupt signal(*C_c*).


## TCP mode (textual variant)

About mode

 - operates over TCP protocol (*statefull*)
 - communication begin with `HELLO` from client, which is responded by server with `HELLO`
 - user then can send queries and server will response with result until user enter `BYE`, wrong message or uses interrupt
- query format is  `SOLVE (operator num num)`, for example `SOLVE (- 100 100)`

## UDP mode (binary variant)
About mode

 - operates over UDP protocol (*stateless*)
 - user then can send queries and server will response with result until user use interrupt
 - opcode is number with type of message
    - 0 = Request
    - 1 = Response
 - status code is number with status
    - 0 = OK
    - 1 = Error
 - payload length is number with length of string in bytes

Request format

 - 8 bits of opcode
 - 8 bits of payload length
 - rest is payload data
 
Request format

 - 8 bits of opcode
 - 8 bits of status code
 - 8 bits of payload length
 - rest is payload data
 
 User format to enter
 - `(operator num num)`, for example  `(* 5 4)`
 

## Protocol TCP vs UDP


Connection  
- In TCP, the connection is made first and then the communication is carried out, we achieve this using the so-called  three-way handshake  
- In UDP, the connection is not established and communication immediately follows 

Reliability  
- TCP is a reliable protocol because it checks data correctness, data order, flow control...  
- UDP is not a reliable protocol because it does not perform checks like TCP

Speed  
- TCP is slower than UDP because of all the checks it does  

Duplex connection  
- TCP offers full-duplex  
- UDP is ideal for half-duplex

## Testing
Testing was performed on three operating systems: Windows 11 (win-x64), Nix OS and Ubuntu 22.04 (linux-x64).  Ubuntu ran in WSL2.  In TCP mode, I tried to enter the correct entries, what happens if I don't enter HELLO at the beginning, how will it behave in case of a wrong entry and how will it behave in the event of an interrupt.  
In the UDP mode, I tested the behaviour in case of bad and correct entry and the response to an interrupt.

### Windows 11
![Windows 11 mode tcp](tests/tcp_windows.png)
![Windows 11 mode udp](tests/udp_windows.png)

### Ubuntu 22.04
![Ubuntu 22.04 mode tcp](tests/tcp_wsl2_ubuntu22-04.png)
![Ubuntu 22.04 mode udp](tests/udp_wsl2_ubuntu22-04.png)

### NIX
![NIX mode tcp](tests/tcp_nix.png)
![NIX mode udp](tests/udp_nix.png)


## Bibliography

 - Contributors to Wikimedia projects. [Duplex (Telecommunications) - Wikipedia.](https://en.wikipedia.org/wiki/Duplex_(telecommunications)) _Wikipedia, the Free Encyclopedia_, Wikimedia Foundation, Inc., 25 June 2005,.
 - [TCP vs UDP: What’s the Difference?](https://www.javatpoint.com/tcp-vs-udp) - _Javatpoint_. Accessed 21 Mar. 2023.
 - [NESFIT/IPK-Projekty - IPK-Projekty - FIT - VUT Brno - Git.](https://git.fit.vutbr.cz/NESFIT/IPK-Projekty/src/branch/master) _FIT - VUT Brno - Git_ . Accessed 21 Mar. 2023.
