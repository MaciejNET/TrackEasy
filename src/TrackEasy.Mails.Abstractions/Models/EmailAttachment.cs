namespace TrackEasy.Mails.Abstractions.Models;

public sealed record EmailAttachment(
    string FileName,
    byte[] Data,
    string ContentType
);