using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrackEasy.Api.AuthorizationHandlers;
using TrackEasy.Application.Users.ConfirmEmail;
using TrackEasy.Application.Users.CreateAdmin;
using TrackEasy.Application.Users.CreatePassenger;
using TrackEasy.Application.Users.DeleteUser;
using TrackEasy.Application.Users.FindUser;
using TrackEasy.Application.Users.GenerateResetPasswordToken;
using TrackEasy.Application.Users.GenerateToken;
using TrackEasy.Application.Users.GenerateTwoFactorToken;
using TrackEasy.Application.Users.ResetPassword;
using TrackEasy.Application.Users.ExternalLogin;
using TrackEasy.Application.Users.UpdateUser;
using TrackEasy.Domain.Users;

namespace TrackEasy.Api.Endpoints;

public class UserEndpoints : IEndpoints
{
    public static void MapEndpoints(RouteGroupBuilder rootGroup)
    {
        var group = rootGroup.MapGroup("/users").WithTags("Users");

        group.MapGet("/me", async (ISender sender, CancellationToken cancellationToken) =>
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

        group.MapGet("/external/{provider}", (string provider, string firstName, string lastName, DateOnly dateOfBirth, HttpContext httpContext) =>
        {
            var redirectUrl = $"/users/external/{provider}/callback";
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            properties.Items["firstName"] = firstName;
            properties.Items["lastName"] = lastName;
            properties.Items["dob"] = dateOfBirth.ToString("O");
            return Results.Challenge(properties, [provider]);
        })
            .AllowAnonymous()
            .WithName("ExternalLoginChallenge")
            .Produces(StatusCodes.Status401Unauthorized)
            .WithDescription("Initiate external login.")
            .WithOpenApi();

        group.MapGet("/external/{provider}/callback", async (string provider, ISender sender, HttpContext httpContext) =>
        {
            var info = await httpContext.RequestServices.GetRequiredService<SignInManager<User>>().GetExternalLoginInfoAsync();
            if (info is null || !string.Equals(info.LoginProvider, provider, StringComparison.OrdinalIgnoreCase))
            {
                return Results.BadRequest();
            }

            var firstName = info.AuthenticationProperties?.Items["firstName"];
            var lastName = info.AuthenticationProperties?.Items["lastName"];
            var dobRaw = info.AuthenticationProperties?.Items["dob"];

            if (firstName is null || lastName is null || dobRaw is null || !DateOnly.TryParse(dobRaw, out var dob))
            {
                return Results.BadRequest();
            }

            var command = new ExternalLoginCommand(provider, firstName, lastName, dob);
            var token = await sender.Send(command);

            await httpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return Results.Ok(token);
        })
            .AllowAnonymous()
            .WithName("ExternalLoginCallback")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Handle external login callback.")
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
        
        group.MapPost("/logout", async ([FromServices] SignInManager<User> signInManager, HttpContext httpContext) =>
        {
            await signInManager.SignOutAsync();
            await httpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return Results.Ok();
        })
        .RequireAuthorization()
        .WithName("Logout")
        .Produces(StatusCodes.Status200OK)
        .WithDescription("Logout user.")
        .WithOpenApi();
        
        group.MapDelete("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new DeleteUserCommand(id);
            await sender.Send(command, cancellationToken);
            return Results.NoContent();
        })
            .RequireAuthorization()
            .WithName("DeleteUser")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Delete user.")
            .WithOpenApi();
    }
}