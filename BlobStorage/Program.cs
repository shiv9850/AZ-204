﻿
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

public class Program
{
    static string storageConnectionString = "CONNECTION STRING";
    static string fileName = "wtfile" + Guid.NewGuid().ToString() + ".txt";
    static string localFilePath = Path.Combine("./data/", fileName);
    static string downloadFilePath = localFilePath.Replace(".txt", "DOWNLOADED.txt");

    public static async Task Main(string[] args)
    {
        try
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);
            BlobContainerClient containerClient = await CreateContainer(blobServiceClient);
            BlobClient blobClient = await UploadBlob(containerClient);
            await ListBlobClients(containerClient);
            await DeleteContainer(containerClient);


        }
        catch (Exception e)
        {
            Console.WriteLine("Error: {0}", e);
        }
        finally
        {
            Console.WriteLine("End of program, press any key to exit.");
            Console.ReadKey();
        }
    }

    private static async Task<BlobContainerClient> CreateContainer(BlobServiceClient blobServiceClient)
    {
        string containerName = "wtblob" + Guid.NewGuid().ToString();

        // Create the container and return a container client object
        BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
        Console.WriteLine("A container named '" + containerName + "' has been created. " +
            "\nTake a minute and verify in the portal." +
            "\nNext a file will be created and uploaded to the container.");
        Console.WriteLine("Press 'Enter' to continue.");
        Console.ReadLine();
        return containerClient;
    }


    private static async Task<BlobClient> UploadBlob(BlobContainerClient containerClient)
    {

        // Write text to the file
        await File.WriteAllTextAsync(localFilePath, "Hello, World!");

        // Get a reference to the blob
        BlobClient blobClient = containerClient.GetBlobClient(fileName);

        Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

        // Open the file and upload its data
        using (FileStream uploadFileStream = File.OpenRead(localFilePath))
        {
            await blobClient.UploadAsync(uploadFileStream);
            uploadFileStream.Close();
        }

        Console.WriteLine("\nThe file was uploaded. We'll verify by listing" +
                " the blobs next.");
        Console.WriteLine("Press 'Enter' to continue.");
        Console.ReadLine();
        return blobClient;
    }

    private static async  Task ListBlobClients(BlobContainerClient containerClient)
    {
        // List blobs in the container
        Console.WriteLine("Listing blobs...");
        await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
        {
            Console.WriteLine("\t" + blobItem.Name);
        }

        Console.WriteLine("\nYou can also verify by looking inside the " +
                "container in the portal." +
                "\nNext the blob will be downloaded with an altered file name.");
        Console.WriteLine("Press 'Enter' to continue.");
        Console.ReadLine();
    }

    private static async Task DownloadBlob(BlobClient blobClient)
    {
        // Download the blob to a local file
        // Append the string "DOWNLOADED" before the .txt extension 

        Console.WriteLine("\nDownloading blob to\n\t{0}\n", downloadFilePath);

        // Download the blob's contents and save it to a file
        BlobDownloadInfo download = await blobClient.DownloadAsync();

        using (FileStream downloadFileStream = File.OpenWrite(downloadFilePath))
        {
            await download.Content.CopyToAsync(downloadFileStream);
        }
        Console.WriteLine("\nLocate the local file in the data directory created earlier to verify it was downloaded.");
        Console.WriteLine("The next step is to delete the container and local files.");
        Console.WriteLine("Press 'Enter' to continue.");
        Console.ReadLine();
    }

    private static async Task DeleteContainer(BlobContainerClient containerClient)
    {
        // Delete the container and clean up local files created
        Console.WriteLine("\n\nDeleting blob container...");
        await containerClient.DeleteAsync();

        Console.WriteLine("Deleting the local source and downloaded files...");
        File.Delete(localFilePath);
        File.Delete(downloadFilePath);

        Console.WriteLine("Finished cleaning up.");
    }
}
