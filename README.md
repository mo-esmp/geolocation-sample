# geolocation-sample

To run project via `docker-compose`, run following commands:
* docker-compose build
* docker-compose up
* access site via http://localhost:5005/docs

To run project via Visual Studio, edit following connection strings:
* Change `PostgresSQL` and `RabbitMQ` connection string in `appsettings.Development.json` file in `GeographySample.WebApi` and `GeographySample.DistanceCalculator` projects and run both projects.
