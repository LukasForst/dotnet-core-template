FROM mcr.microsoft.com/mssql/server:2019-latest AS db

ENV ACCEPT_EULA=Y
ENV DATABASE=main-db-scheme

COPY --chown=mssql:root /init /usr/src/app

WORKDIR /usr/src/app

RUN chmod +x ./*.sh 

EXPOSE 1433

ENTRYPOINT ./entrypoint.sh