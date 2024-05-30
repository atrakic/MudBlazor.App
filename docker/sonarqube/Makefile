MAKEFLAGS += --silent

OPTIONS ?= --build --remove-orphans --force-recreate

all:
	docker-compose up $(OPTIONS) -d

clean:
	docker-compose down --remove-orphans -v --rmi local
