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
	for d in *.Tests; do (cd "$$d" && exec dotnet test) ; done ;
    
integration-tests: run-db
	for d in *.IntegrationTests; do (cd "$$d" && exec dotnet test) ; done ;
	
all-tests:
	dotnet test

run: run-db
	(cd Api && exec dotnet run)
