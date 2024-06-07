MAKEFLAGS += --silent

OPTIONS ?= --build --remove-orphans --force-recreate

APP ?= app

all:
	docker-compose up $(OPTIONS) -d

%:
	docker-compose up $(OPTIONS) $@ -d
	docker-compose ps -a

healthcheck:
	docker inspect $(APP) --format "{{ (index (.State.Health.Log) 0).Output }}"

docker-devel:
	docker-compose -f docker-compose.devel.yml up $(OPTIONS)

dotnet-tool-install:
	dotnet --version
	dotnet new tool-manifest --force
	dotnet tool update --local dotnet-ef
	dotnet tool install --global dotnet-outdated-tool
	dotnet tool install --global dotnet-sonarscanner

dotnet-migrations:
	pushd ./src; \
        if [ -z "$(TARGET)" ]; then \
            dotnet ef migrations list --no-build; \
            dotnet ef migrations has-pending-model-changes -- --environment Production; \
        else \
            dotnet ef migrations add $(TARGET); \
            dotnet ef database update -- --environment Production; \
        fi; \
    popd

dotnet-run: docker-db dotnet-migrations
	dotnet run --environment Production --project src/app.csproj


dotnet-upgrade:
	dotnet tool restore
	dotnet outdated --upgrade

test:
	dotnet test

clean:
	docker-compose down --remove-orphans -v --rmi local
	dotnet clean
	rm -rf ./src/*.db
	rm -rf ./src/{bin,obj}

-include .env
