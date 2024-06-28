# Introduction 
BandLab Test Assignment by John Inyang. A system that allows you to upload images and comment on them.

# Build and Test
Before you Run the code, you will need to :-
- Provide a connection string for an SQL Server database in the local.settings.json file
- Run 'update-database' command in the package manager console to initialize your database
- Provide a connection string for an azure storage account in the local.settings.json file

# Deploy to Production
The following actions should be completed before deploying to production :-

- Manual test to confirm that the functionalities work as expected.
- Load test to make sure the appication is able to handle expected load.
- Setup a CI/CD pipeline with necessary permissions to automate deployment
