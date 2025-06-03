using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using TrackEasy.Shared.Files.Abstractions;

namespace TrackEasy.Shared.Files;

internal sealed class BlobService(IConfiguration configuration) : IBlobService
{
    private readonly BlobServiceClient _blobServiceClient = new(configuration["cs-blob"]);


    public async Task<FileModel?> FindAsync(string name, string containerName, CancellationToken cancellationToken)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(name);
        if (!await blobClient.ExistsAsync(cancellationToken))
        {
            return null;
        }
        var blobDownloadInfo = await blobClient.DownloadAsync(cancellationToken);
        using var memoryStream = new MemoryStream();
        await blobDownloadInfo.Value.Content.CopyToAsync(memoryStream, cancellationToken);
        return new FileModel(name, memoryStream.ToArray(), blobDownloadInfo.Value.ContentType);
    }

    public async Task<FileModel> SaveAsync(string name, byte[] content, string contentType, string containerName,
        CancellationToken cancellationToken)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        var blobClient = containerClient.GetBlobClient(name);
        using var memoryStream = new MemoryStream(content);
        await blobClient.UploadAsync(memoryStream, new Azure.Storage.Blobs.Models.BlobHttpHeaders
        {
            ContentType = contentType
        }, cancellationToken: cancellationToken);
        return new FileModel(name, content, contentType);
    }

    public async Task DeleteAsync(string name, string containerName, CancellationToken cancellationToken)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(name);
        if (await blobClient.ExistsAsync(cancellationToken))
        {
            await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        }
    }
}