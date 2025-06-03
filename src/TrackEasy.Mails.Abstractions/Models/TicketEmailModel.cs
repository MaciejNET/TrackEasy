namespace TrackEasy.Mails.Abstractions.Models;

public record TicketEmailModel(int TicketNumber, byte[] TicketPdf);