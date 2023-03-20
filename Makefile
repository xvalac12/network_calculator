all: clean build publish

OS=linux-x64 #win-x64, osx-x64
ARG=-o . -c Release -f net6.0
clean:
	dotnet clean -c Release -f net6.0

build: 
	dotnet build -c Release -f net6.0

publish:
	dotnet publish $(ARG) -r $(OS)
