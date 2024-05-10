MAKEFLAGS += --silent

APP ?= app

all:
	docker-compose up --no-color --remove-orphans -d

%:
	docker-compose up --build --no-color --remove-orphans $@ -d
	docker-compose ps -a

healthcheck:
	docker inspect $(APP) --format "{{ (index (.State.Health.Log) 0).Output }}"

upgrade:
	#dotnet tool install --global dotnet-outdated-tool
	dotnet outdated --upgrade

test.volume:
	docker run --rm -i -v=shared-tmpfs:/var/tmp busybox find /var/tmp

test:
	dotnet test

clean:
	docker-compose down --remove-orphans -v --rmi local
	dotnet clean
	rm -rf ./src/*.db
	rm -rf ./src/{bin,obj}

-include .env
