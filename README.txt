Identity Server 4 Hosted on a .NetCore MVC App with AspNet Identity user base.

There are 3 projects each in its own folder
idlib (class library), 
idsvr4 (identity server 4) 
userhub (AspNet Core MVC)

Before you run you Identity Server, you need the AspNetIdentity database up and running as well as the Identity Server
persisted storage configuration tables, both in SQL server.
See configuration to change your connection string.
Pending: Adding Database Initialization script to repo.

1. Clone Repo
2. run >dotnet restore on each project
4. run >dotnet build
5. Change directories to idsvr4
6. run >dotnet run (you should be runing Identity Server 4 on port 5000)

Pending: userhub is meant to be the user management UI. The status is WIP.