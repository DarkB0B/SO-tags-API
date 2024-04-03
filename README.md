# StackOverflow tag API - .NET 8 RESTful API
## Project Requirements:
* Retrieve a minimum of 1000 tags from the StackOverflow API to a local database or another persistent cache.
* Tag retrieval may occur either at startup or upon the first request, either in full or gradually for missing data.
* Calculate the percentage share of tags in the entire retrieved population (based on the 'count' field, appropriately calculated).
* Provide tags through paginated API with options for sorting by name and share in both directions.
* Provide an API method to force re-retrieval of tags from StackOverflow.
* Provide the OpenAPI definition of the prepared API methods.
* Include logging, error handling, and runtime service configuration.
* Prepare selected internal service unit tests.
* Prepare selected integration tests based on the provided API.
* Utilize containerization to ensure repeatable building and running of the project.
* Publish the solution in a GitHub repository.
* The entire solution should start with the command "docker-compose up".
## Technologies:
* .NET 8
* Entity Framework
* MSSQL
* Xunit
* Docker
## Startup:
### To run the project just clone this repository and run "docker-compose up" command 
