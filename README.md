# Example of a bug in EF Core
There is an issue with owns many and deleting the parent where the owned collection attempts to be re-inserted.

## Setup
1. Open this project in WSL2, or Ubuntu.
2. After the install script runs, execute: `sudo /opt/mssql/bin/mssql-conf setup` to finish setting up SQL Server.
  - Choose 2 for the Developer Edition
  - Agree to the terms
  - Choose your language (1 for English)
  - sa password I used the uber secure `Password123!`