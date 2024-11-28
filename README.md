# Coding assignment

## Description
The use-case is a company-wide steps leaderboard application for teams of employees: picture that all
employees are grouped into teams and issued step counters. This application needs to receive and store
step count increments for each team, and calculate sums.

## Getting Started
The following prerequisites are required to build and run the solution:
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (latest version)

## Launch the app
```bash
cd EfficyTask.Web
dotnet run
```
and then just open the following link in your browser:
```
http://localhost:5270/swagger/index.html
```
you can also use MS Visual Studio to run the project.

# Additional questions
## Persistence
**Q:** How would you add a persistent storage layer such that the app could be restarted without losing counter states?

**A:** I have already added a persistent storage layer that uses a SQLite database.
It is lightweight, open-source, self-contained, serverless SQL database.
But for the real world, we certainly need something more flexible and reliable.
The project is already configured to use any persistence technology.
It is also configured to use Entity Framework with LinqToSQL, which provides flexibility.
___
**Q:** What storage technology would you suggest?

**A:** It depends on what our plans are for this application and how we want to develop it further.
If we plan to add much more entities, and data integrity and consistency are of primary importance to us,
then it is better to use a relational DBMS. For example MS SQL Server.

But at this stage, the best choice is a non-relational database, for example, MonogoDB.
Or we can use Azure CosmosDB because it has a better integratio with Azure resources in case we want to deploy it in Azure.
Anyway, NoSQL is a better option for now, because of flexible schema. For example, we probably don't even need 2 collections/enities
in this case, because we never need counters outside of teams, so they can probably be nested.
Also, NoSQL databases are usually faster and scale much better.

## Fault tolerance
**Q:** How would you design the app in order to make the functionality be available even if some parts of
the underlying hardware systems were to fail?

**A:** For that scenario we need to configure the replication. 
We can deploy our app to Azure App sercive and configure autoscaling. 
We can also deploy it across multiple regions and configure load balancing.
As for the database, if we use a NoSQL database, it will also be easy to set up replication for them.
We can also configure retriy policies in case of transient failures.

## Scalability
**Q:** How would you design the app in order to ensure that it wouldnʼt slow down or fail if usage increased
by many orders of magnitude? what if this turned into a global contest with 10x, 100x, 1000x the
number of teams and traffic?

**A:** As i said before, i would use Azure App Service build-in autoscaling. It could be manual, automatic or based on rules.
Same for database server. Both MongoDB (Atlas) and Azure Cosmos DB support scaling.
We can also use caching (like Azure Cache for Redis) to cache frequenly accessed data.
___
**Q:** Does your choice of persistence layer change in this scenario?

**A:** No, i would still use NoSQL databases like MongoDB or Azure CosmosDB due to their support for scaling, global distribution, and low-latency reads and writes.

## Authentication
**Q:** How would you ensure that only authorised users can submit and/or retrieve data?

**A:** We can add JWT-based authentication. The token would include claims for user roles and permissions.
___
**Q:** How would you then add support to allow different users to only update specific counters? Or
perform only specific operations?

**A:** We need to use a combination of authentication and authorization. 
We will add JWT tokens that will include user information such as id, teamId and roles that we will store in the database.
And then we can add Role-Based Access Control by creating RBAC policies and validating them using attributes for each endpoint.
This will allow us to validate roles and provide basic security.
But we will also need to implement some custom business logic to prevent users from updating counters from other teams, for example.

