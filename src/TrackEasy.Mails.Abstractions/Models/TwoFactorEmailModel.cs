namespace TrackEasy.Mails.Abstractions.Models;

public sealed record TwoFactorEmailModel(string FirstName, string LastName, string Token);