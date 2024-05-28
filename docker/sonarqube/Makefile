MAKEFLAGS += --silent

OPTIONS ?= --build --remove-orphans --force-recreate

all:
	DOCKER_BUILDKIT=1 docker-compose up $(OPTIONS) -d

clean:
	docker-compose down --remove-orphans -v --rmi local
