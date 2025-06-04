namespace TrackEasy.Shared.Files.Abstractions;

public interface IBlobService
{
    Task<FileModel?> FindAsync(string name, string containerName, CancellationToken cancellationToken);
    Task<FileModel> SaveAsync(string name, byte[] content, string contentType, string containerName, CancellationToken cancellationToken);
    Task DeleteAsync(string name, string containerName, CancellationToken cancellationToken);
}