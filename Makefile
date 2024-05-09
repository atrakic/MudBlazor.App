MAKEFLAGS += --silent

APP ?= app

all:
	docker-compose up --no-color --remove-orphans -d

%:
	docker-compose up --build --no-color --remove-orphans $@ -d
	docker-compose ps -a

healthcheck:
	docker inspect $(APP) --format "{{ (index (.State.Health.Log) 0).Output }}"

clean:
	docker-compose down --remove-orphans -v --rmi local
	rm -rf ./src/*.db
	rm -rf ./src/{bin,obj}

test.volume:
	docker run --rm -i -v=shared-tmpfs:/var/tmp busybox find /var/tmp

-include .env
