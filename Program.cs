using System;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BlobQuickstartV12
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("MySQL backup to Azure Blob storage\n");

            var backupPath = "/home/whistler/dotnet/publish/";
            await createBackup(backupPath);
            await LoadFiles(backupPath);
        }

        private static async Task createBackup(string backupPath)
        {
            // mysqldump -u MYSQLUSER -pPASSWORD --all-databases > all_databases.sql
            ProcessStartInfo startInfo = new ProcessStartInfo("/bin/bash", backupPath + "backup.sh");
            Process proc = new Process() { StartInfo = startInfo, };
            proc.Start();
        }

        private static async Task LoadFiles(string backupPath)
        {
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

                //Creating Container
                string containerName = $"mysqlbackup-{DateTime.UtcNow.ToString("ddMyyyy-HHmmss")}";
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                Console.WriteLine("containerName: " + containerName);

                BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);

                Console.WriteLine($"containerClient {containerClient.Name}");

                // Create a local file in the ./data/ directory for uploading and downloading
                /* string localPath = "./data/";
                string fileName = $"quickstart{Guid.NewGuid().ToString()}.txt";
                string localFilePath = Path.Combine(localPath, fileName);

                await File.WriteAllTextAsync(localFilePath, "Hello, World!"); */
                string fileName = "all_databases.sql";
                string localFilePath = Path.Combine(backupPath, fileName);

                // Ger a reference to a blob
                BlobClient blobClient = containerClient.GetBlobClient(fileName);

                Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

                // Open the file and upload its data
                using (FileStream uploadFileStream = File.OpenRead(localFilePath))
                {
                    await blobClient.UploadAsync(uploadFileStream, true);
                }

                Console.WriteLine("Listing blobs...");

                await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
                {
                    Console.WriteLine("\t" + blobItem.Name);
                }

                // Download the blob to a local file
                // Append the string "DOWNLOAD" before the .txt extension 
                // so you can compare the files in the data directory
                /* string downloadFilePath = localFilePath.Replace(".txt", "_download.txt");
                Console.WriteLine($"\nDownloading blob to \n\t{downloadFilePath}");

                // Download the blob's contents and save it to a file
                BlobDownloadInfo download = await blobClient.DownloadAsync();

                using (FileStream downloadFileStream = File.OpenWrite(downloadFilePath))
                {
                    await download.Content.CopyToAsync(downloadFileStream);
                    downloadFileStream.Close();
                } */

                // Clean up
                // await containerClient.DeleteAsync();
                // File.Delete(localFilePath);
                Console.WriteLine("Backup finished");

            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
                throw;
            }
        }
    }
}
