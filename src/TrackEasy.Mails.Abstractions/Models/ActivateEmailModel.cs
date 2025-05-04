namespace TrackEasy.Mails.Abstractions.Models;

public sealed record ActivateEmailModel(string FirstName, string LastName, string Email, string Token, string Url);