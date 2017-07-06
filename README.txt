Identity Server 4 Hosted on a .NetCore MVC App with AspNet Identity user base.

There are 3 projects, each in its own folder:
idlib (class library), 
idsvr4 (identity server 4) 
userhub (AspNet Core MVC)
Note: There is an extra folder called database which contains SQL Server database creation scripts 
for AspNetIdentity and IdentityServer configuration.

Before you run Identity Server, you need the AspNetIdentity database up and running as well as the Identity Server
persisted storage configuration tables, both in SQL server (See step 4)
See appsettings.json configuration files to change your connection string.


1. Clone Repo.
2. run >dotnet restore on each project.
3. run >dotnet build on each project.
4. Navigate to /database directory and execute scripts in the defined numeric order.
   Note: My database is called IdSvr4. You can keep that name or make changes to the
   scripts for whatever name you choose.
5. Navigate to idsvr4 project directory.
6. run >dotnet run (you should be runing Identity Server 4 on port 5000)
7. Navigate to userhub project directory.
8 run >dotnet run (userhub should be running in port 5005)
   Note: userhub should be properly configured using the implicit flow in the identity server 
   tables created in SQL server during step 4.

Note: userhub is meant to be the user management UI. The status is WIP.