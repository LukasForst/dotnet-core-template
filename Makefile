build-db:
	docker-compose build db

migrate:
	(cd DataAccess && exec dotnet ef database update)

# with NAME=MigrationName
add-migration:
	(cd DataAccess && exec dotnet ef migrations add $(NAME) -o Migrations)
