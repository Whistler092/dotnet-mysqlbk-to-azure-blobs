# dotnet-mysqlbk-to-azure-blobs

dotnet shell application to take mysql backup and upload to azure blobs

### How to execute

Install dotnet on ubuntu

```
wget https://packages.microsoft.com/config/ubuntu/19.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb

sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update
sudo apt-get install apt-transport-https
sudo apt-get update

sudo apt-get install dotnet-sdk-3.1
sudo apt-get install aspnetcore-runtime-3.1

git clone https://github.com/Whistler092/dotnet-mysqlbk-to-azure-blobs.git
cd dotnet-mysqlbk-to-azure-blobs

dotnet publish --configuration Release
cd /BlobQuickstartV12/bin/Release/netcoreapp3.1/publish/
chmod 777 backup.sh
export AZURE_STORAGE_CONNECTION_STRING="<yourconnectionstring>"
dotnet BlobQuickstartV12.dll

```

[Tutorial how to take out your AZURE_STORAGE_CONNECTION_STRING](https://docs.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet#configure-your-storage-connection-string)

Result should look like this

```
whistler@tiendas:~/dotnet/publish$ dotnet BlobQuickstartV12.dll
Azure Blob storage v12 - .NET quickstart sample

containerName: mysqlbackup-1052020-162848
containerClient mysqlbackup-1052020-162848
Uploading to Blob storage as blob:
	 https://myurl.blob.core.windows.net/mysqlbackup-1052020-162848/all_databases.sql

Listing blobs...
	all_databases.sql
Backup finished
```

### Source

https://docs.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet
