.phony : all clean compile test

all: compile test

# Detect OS and arch
OSFLAG := 
ARCHFLAG :=
NASMFORMAT := 
GCCFLAGS := 

ifeq ($(OS),Windows_NT)
	OSFLAG += -D WIN32
	ifeq ($(PROCESSOR_ARCHITECTURE),AMD64)
		OSFLAG += -D AMD64
		NASMFORMAT := win64
	endif
	ifeq ($(PROCESSOR_ARCHITECTURE),x86)
		OSFLAG += -D IA32
		NASMFORMAT := win32
	endif
else
	UNAME_S := $(shell uname -s)
	UNAME_P := $(shell uname -p)
	ifeq ($(UNAME_S),Linux)
		OSFLAG += -D LINUX
	endif
	ifeq ($(UNAME_S),Darwin)
		OSFLAG += -D OSX
	endif
	ifeq ($(UNAME_P),x86_64)
		OSFLAG += -D AMD64
		NASMFORMAT := elf64
		ARCHFLAG := -m64
	endif
	ifneq ($(filter %86,$(UNAME_P)),)
		OSFLAG += -D IA32
		NASMFORMAT := elf32
		ARCHFLAG := -m32
	endif
	ifneq ($(filter arm%,$(UNAME_P)),)
		OSFLAG += -D ARM
		# example: assume aarch64 on mac/linux
		NASMFORMAT := elf64
		ARCHFLAG := -march=armv8-a
	endif
endif

os:
	echo "Operating System: $(OS)"
	echo "Architecture: $(ARCHFLAG)"

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
	nasm -f $(NASMFORMAT) output.asm -o output.o
	gcc $(ARCHFLAG) output.o -o output

clean:
	@echo "Cleaning up..."
	rm -rf output.o output
	rm -rf output.asm
	dotnet clean CLIShover/
	dotnet clean Unit-Test/