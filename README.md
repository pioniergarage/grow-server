# GROW Server

Public-facing website, administration backend and API for the GROW contest.

The GROW contest is a yearly student founder contest organized by the [PionierGarage e.V.](https://www.pioniergarage.de).

## Routes

* /{year}/{controller}/{action} - Public-facing website about the contest 
* /Api/{controller}/{action} - API for the GROW app
* /Admin/{controller}/{action} - Content Management Web App for the GROW administrators  
* /Team/{controller}/{action} - Web App for the participating GROW teams

## Technologies used

* ASP.NET Core Web API
* ASP.NET Core MVC
* Razor
* Entity Framework Core + Migrations

## Hosted Infrastructure

* Hosted URL: https://grow.pioniergarage.de
* Azure App Service as Web Host
* Azure SQL DB as Database
* Azure Blob Storage as Image Store

## CI/CD information

* The code is hosted on [Github](https://github.com)
* CI/CD pipelines are set up in [Azure DevOps](https://dev.azure.com/)
  * DevOps project: https://dev.azure.com/pioniergarage/_git/GROW
  * Build pipeline ("GROW - ASP.NET Core") is triggered on commit to master
  * Release pipeline ("Deploy to Azure") is triggered on new Build

## License

All intellectual property of this project as well as the trademark "PionierGarage" is owned by [PionierGarage e.V.](https://www.pioniergarage.de)