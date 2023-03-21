## Implemented functionality
- Application
	- command line argument parsing
	- makefile with build, publish and clean
- Mode TCP
	- entering correct query
	- entering wrong query
	- creating connection, closing connection
	- communication
	- reaction to sigint
- Mode UDP
	- entering correct query
	- entering wrong query
	-  reaction to sigint
## Known limitations
- Application
	- `make clean` do not work on windows 11 
- Mode UDP
	- when server sends wrong opcode, there is no timeout implemented 
