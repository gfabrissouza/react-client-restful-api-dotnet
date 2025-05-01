FROM mysql:5.7.22
EXPOSE 3306

COPY ./rest-api-dotnet/Database/Migrations/ /home/database/
COPY ./rest-api-dotnet/Database/DataSet/ /home/database/
COPY ./rest-api-dotnet/Database/init_database.sh /docker-entrypoint-initdb.d/init_database.sh

RUN chmod +x /docker-entrypoint-initdb.d/init_database.sh