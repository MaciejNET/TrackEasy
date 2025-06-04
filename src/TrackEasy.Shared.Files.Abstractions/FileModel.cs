namespace TrackEasy.Shared.Files.Abstractions;

public sealed record FileModel(string Name, byte[] Content, string ContentType);
