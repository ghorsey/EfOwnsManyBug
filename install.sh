set -e
sudo apt-get update
sudo apt-get upgrade -y


echo "************************"
echo "* Install dependencies *"
echo "************************"
dotnet tool restore # Restore husky in the root of the monorepo
dotnet husky install # Install the git hooks
dotnet tool restore
dotnet restore

echo "****************************"
echo "* Setup Sql server express *"
echo "****************************"
curl https://packages.microsoft.com/keys/microsoft.asc | sudo tee /etc/apt/trusted.gpg.d/microsoft.asc
curl -fsSL https://packages.microsoft.com/config/ubuntu/22.04/mssql-server-2022.list | sudo tee /etc/apt/sources.list.d/mssql-server-2022.list
curl https://packages.microsoft.com/config/ubuntu/22.04/prod.list | sudo tee /etc/apt/sources.list.d/mssql-release.list
sudo apt-get update
sudo apt-get install -y mssql-server
sudo apt-get install mssql-tools18 unixodbc-dev
# sudo /opt/mssql/bin/mssql-conf setup
# systemctl status mssql-server --no-pager