all:
	docker-compose up --no-color --remove-orphans -d

%:
	docker-compose up --build --no-color --remove-orphans $@ -d
	docker-compose ps -a

clean:
	docker-compose down --remove-orphans -v --rmi local
	rm -rf ./src/*.db
	rm -rf ./src/{bin,obj}

-include .env
