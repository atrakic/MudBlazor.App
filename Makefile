MAKEFLAGS += --silent

APP ?= app

all:
	docker-compose up --no-color --remove-orphans -d

%:
	docker-compose up --build --no-color --remove-orphans $@ -d
	docker-compose ps -a

healthcheck:
	docker inspect $(APP) --format "{{ (index (.State.Health.Log) 0).Output }}"

tools.local:
	dotnet --version
	dotnet new tool-manifest --force
	dotnet tool update --local dotnet-ef

migrations.local:
	dotnet ef migrations add $(NAME) --project ./src/app.csproj

dotnet-upgrade.local:
	#dotnet tool install --global dotnet-outdated-tool
	dotnet outdated --upgrade

docker-staging-up:
	docker-compose -f docker-compose.staging.yml up --build --no-color --remove-orphans

test:
	dotnet test

#testvolume:
#	docker run --rm -i -v=shared-tmpfs:/var/tmp busybox find /var/tmp

clean:
	docker-compose down --remove-orphans -v --rmi local
	dotnet clean
	rm -rf ./src/*.db
	rm -rf ./src/{bin,obj}

-include .env
