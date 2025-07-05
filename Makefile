.phony : all clean compile test

all: compile test

compiler:
	@echo "Compiling the project..."
	dotnet build CLIShover/
	dotnet run --project CLIShover/ClIShover.csproj

test:
	@echo "Running tests..."
	dotnet build Unit-Test/
	dotnet run --project Unit-Test/Unit-Test.csproj

asm:
	@echo "Compiling generated assembly code..."
	nasm -f elf64 output.asm -o output.o
	ld -o output output.o

clean:
	@echo "Cleaning up..."
	rm -rf output.o output
	rm -rf output.asm
	dotnet clean CLIShover/
	dotnet clean Unit-Test/