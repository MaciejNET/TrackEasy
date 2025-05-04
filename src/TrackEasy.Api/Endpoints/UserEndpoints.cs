using MediatR;
using TrackEasy.Api.AuthorizationHandlers;
using TrackEasy.Application.Users.ConfirmEmail;
using TrackEasy.Application.Users.CreateAdmin;
using TrackEasy.Application.Users.CreatePassenger;
using TrackEasy.Application.Users.FindUser;
using TrackEasy.Application.Users.GenerateResetPasswordToken;
using TrackEasy.Application.Users.GenerateToken;
using TrackEasy.Application.Users.GenerateTwoFactorToken;
using TrackEasy.Application.Users.ResetPassword;
using TrackEasy.Application.Users.UpdateUser;

namespace TrackEasy.Api.Endpoints;

public class UserEndpoints : IEndpoints
{
    public static void MapEndpoints(RouteGroupBuilder rootGroup)
    {
        var group = rootGroup.MapGroup("/users").WithTags("Users");

        group.MapGet("/", async (ISender sender, CancellationToken cancellationToken) =>
            {
                var query = new FindUserQuery();
                var user = await sender.Send(query, cancellationToken);
                return Results.Ok(user);
            })
            .RequireAuthorization()
            .WithName("GetUser")
            .Produces<UserDto>()
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Get user.")
            .WithOpenApi();
        
        group.MapPost("/passenger", async (CreatePassengerCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            await sender.Send(command, cancellationToken);
            return Results.Ok();
        })
            .WithName("CreatePassenger")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Create a new passenger.")
            .WithOpenApi();
        
        group.MapPost("/admin", async (CreateAdminCommand command, ISender sender, CancellationToken cancellationToken) => 
        {
            await sender.Send(command, cancellationToken);
            return Results.Ok();
        })
            .RequireAdminAccess()
            .WithName("CreateAdmin")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Create a new admin.")
            .WithOpenApi();
        
        group.MapPost("/token", async (GenerateTokenCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            var token = await sender.Send(command, cancellationToken);
            return Results.Ok(token);
        })
            .WithName("GenerateToken")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Generate JWT token.")
            .WithOpenApi();
        
        group.MapPost("/token-2fa", async (GenerateTwoFactorTokenCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            var token = await sender.Send(command, cancellationToken);
            return Results.Ok(token);
        })
            .RequireAuthorization()
            .WithName("GenerateTwoFactorToken")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Generate 2FA token.")
            .WithOpenApi();
        
        group.MapPost("/reset-password", async (ResetPasswordCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            await sender.Send(command, cancellationToken);
            return Results.Ok();
        })
            .RequireAuthorization()
            .WithName("ResetPassword")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Reset password.")
            .WithOpenApi();
        
        group.MapPost("/reset-password-token", async (GenerateResetPasswordTokenCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            var token = await sender.Send(command, cancellationToken);
            return Results.Ok(token);
        })
            .RequireAuthorization()
            .WithName("GenerateResetPasswordToken")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Generate reset password token.")
            .WithOpenApi();
        
        group.MapPost("/confirm-email", async (ConfirmEmailCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            await sender.Send(command, cancellationToken);
            return Results.Ok();
        })
            .WithName("ConfirmEmail")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Confirm email.")
            .WithOpenApi();
        
        group.MapPatch("{id:guid}/update", async (Guid id, UpdateUserCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            command = command with { Id = id };
            await sender.Send(command, cancellationToken);
            return Results.NoContent();
        })
            .RequireAuthorization()
            .WithName("UpdateUser")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Update user.")
            .WithOpenApi();
    }
}