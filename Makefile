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
	docker-compose -f compose.devel.yml up $(OPTIONS)

dotnet-tool-install:
	dotnet --version
	dotnet new tool-manifest --force
	dotnet tool update --local dotnet-ef
	dotnet tool install --global dotnet-outdated-tool
	dotnet tool install --global dotnet-sonarscanner

dotnet-run: docker-db dotnet-migrations
	dotnet run --environment Production --project src/app.csproj

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

docker-db:
	docker-compose up -d db

docker-dotnet-migrations:
	docker-compose run --rm app make dotnet-migrations

dotnet-upgrade:
	dotnet tool restore
	dotnet outdated --upgrade

test:
	dotnet test
	./tests/test.sh

sonarqube:
	pushd ./docker/sonarqube; \
		docker-compose up -d; \
	popd

sonarqubescanner-build:
	dotnet sonarscanner begin /k:"$(SONAR_PROJECT_KEY)" /d:sonar.host.url="$(SONAR_HOST_URL)" /d:sonar.login="$(SONAR_LOGIN)" \
        	/d:sonar.cs.opencover.reportsPaths="**/coverage.opencover.xml"
	dotnet build
	#dotnet sonarscanner end /d:sonar.login="$(SONAR_LOGIN)"

clean:
	docker-compose down --remove-orphans -v --rmi local
	dotnet clean
	rm -rf ./src/*.db
	rm -rf ./src/{bin,obj}
	rm -rf ./tests/*/{bin,obj}

-include .env
