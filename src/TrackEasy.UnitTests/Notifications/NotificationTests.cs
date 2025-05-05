using FluentValidation;
using Microsoft.Extensions.Time.Testing;
using Shouldly;
using TrackEasy.Domain.Notifications;

namespace TrackEasy.UnitTests.Notifications;

public class NotificationTests
{
    private readonly FakeTimeProvider _fakeTimeProvider = new();

    private static void ValidateAndThrow(Notification notification) =>
        new NotificationValidator().ValidateAndThrow(notification);

    [Fact]
    public void Create_Should_CreateNotification_For_ValidData()
    {
        // Arrange
        const string title = "Valid Title";
        const string message = "This is a valid notification message.";
        var userId = Guid.NewGuid();
        var expectedCreationTime = DateTime.UtcNow;
        _fakeTimeProvider.SetUtcNow(expectedCreationTime);

        // Act
        var notification = Notification.Create(title, message, userId, _fakeTimeProvider);

        // Assert
        notification.ShouldNotBeNull();
        notification.Id.ShouldNotBe(Guid.Empty);
        notification.Title.ShouldBe(title);
        notification.Message.ShouldBe(message);
        notification.UserId.ShouldBe(userId);
        notification.CreatedAt.ShouldBe(expectedCreationTime);

        // Also validate using the validator
        Should.NotThrow(() => ValidateAndThrow(notification));
    }

    [Theory]
    [InlineData("")]
    [InlineData("AB")] // Too short
    public void Create_Should_ThrowValidationException_For_InvalidTitle(string invalidTitle)
    {
        // Arrange
        const string validMessage = "Valid message content.";
        var validUserId = Guid.NewGuid();

        // Act
        var notification = Notification.Create(invalidTitle, validMessage, validUserId, _fakeTimeProvider);

        // Assert
        Should.Throw<ValidationException>(() => ValidateAndThrow(notification))
            .Message.ShouldContain("Title");
    }

    [Fact]
    public void Create_Should_ThrowValidationException_For_TooLongTitle()
    {
        // Arrange
        var tooLongTitle = new string('A', 256); // Max length is 255
        const string validMessage = "Valid message content.";
        var validUserId = Guid.NewGuid();

        // Act
        var notification = Notification.Create(tooLongTitle, validMessage, validUserId, _fakeTimeProvider);

        // Assert
        Should.Throw<ValidationException>(() => ValidateAndThrow(notification))
            .Message.ShouldContain("Title");
    }

    [Theory]
    [InlineData("")]
    [InlineData("AB")] // Too short
    public void Create_Should_ThrowValidationException_For_InvalidMessage(string invalidMessage)
    {
        // Arrange
        var validTitle = "Valid Title";
        var validUserId = Guid.NewGuid();

        // Act
        var notification = Notification.Create(validTitle, invalidMessage, validUserId, _fakeTimeProvider);

        // Assert
        Should.Throw<ValidationException>(() => ValidateAndThrow(notification))
            .Message.ShouldContain("Message");
    }

     [Fact]
    public void Create_Should_ThrowValidationException_For_TooLongMessage()
    {
        // Arrange
        var validTitle = "Valid Title";
        var tooLongMessage = new string('M', 101); // Max length is 100
        var validUserId = Guid.NewGuid();

        // Act
        var notification = Notification.Create(validTitle, tooLongMessage, validUserId, _fakeTimeProvider);

        // Assert
        Should.Throw<ValidationException>(() => ValidateAndThrow(notification))
            .Message.ShouldContain("Message");
    }

    [Fact]
    public void Create_Should_ThrowValidationException_For_EmptyUserId()
    {
        // Arrange
        const string validTitle = "Valid Title";
        const string validMessage = "Valid message content.";
        var emptyUserId = Guid.Empty;

        // Act
        var notification = Notification.Create(validTitle, validMessage, emptyUserId, _fakeTimeProvider);

        // Assert
        Should.Throw<ValidationException>(() => ValidateAndThrow(notification))
            .Message.ShouldContain("User Id");
    }
}