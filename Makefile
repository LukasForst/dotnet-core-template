build-db:
	docker-compose build db

run-db:
	docker-compose up -d db

migrate-db:
	(cd DataAccess && exec dotnet ef database update)

# with NAME=MigrationName
add-migration:
	(cd DataAccess && exec dotnet ef migrations add $(NAME) -o Migrations)

unit-tests:
	for d in *.Tests/ ; do					\
	    if [ -d "$d" ]; then				\
	    	(cd "$d" && exec dotnet test)	\
        fi									\
	done

integration-tests:
	for d in *.IntegrationTests/ ; do		\
	    if [ -d "$d" ]; then				\
	    	(cd "$d" && exec dotnet test)	\
        fi									\
	done

all-tests: unit-tests integration-tests

run: run-db
	(cd Api && exec dotnet run)
