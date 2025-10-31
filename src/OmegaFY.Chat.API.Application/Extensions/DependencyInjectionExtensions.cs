using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OmegaFY.Chat.API.Application.Commands.Auth.Login;
using OmegaFY.Chat.API.Application.Commands.Auth.Logoff;
using OmegaFY.Chat.API.Application.Commands.Auth.RefreshToken;
using OmegaFY.Chat.API.Application.Commands.Auth.RegisterNewUser;
using OmegaFY.Chat.API.Application.Commands.Chat.AddMemberToGroup;
using OmegaFY.Chat.API.Application.Commands.Chat.ChangeGroupConfig;
using OmegaFY.Chat.API.Application.Commands.Chat.CreateGroupConversation;
using OmegaFY.Chat.API.Application.Commands.Chat.RemoveMemberFromGroup;
using OmegaFY.Chat.API.Application.Commands.Chat.SendMessage;
using OmegaFY.Chat.API.Application.Commands.Users.AcceptFriendshipRequest;
using OmegaFY.Chat.API.Application.Commands.Users.RejectFriendshipRequest;
using OmegaFY.Chat.API.Application.Commands.Users.RemoveFriendship;
using OmegaFY.Chat.API.Application.Commands.Users.SendFriendshipRequest;
using OmegaFY.Chat.API.Application.Events;
using OmegaFY.Chat.API.Application.Events.Auth.Login;
using OmegaFY.Chat.API.Application.Events.Auth.Logoff;
using OmegaFY.Chat.API.Application.Events.Auth.RefreshToken;
using OmegaFY.Chat.API.Application.Events.Auth.RegisterNewUser;
using OmegaFY.Chat.API.Application.Events.Chat.SendMessage;
using OmegaFY.Chat.API.Application.Events.Users.AcceptFriendshipRequest;
using OmegaFY.Chat.API.Application.Events.Users.RejectFriendshipRequest;
using OmegaFY.Chat.API.Application.Events.Users.RemoveFriendship;
using OmegaFY.Chat.API.Application.Events.Users.SendFriendshipRequest;
using OmegaFY.Chat.API.Application.Queries.Users.GetCurrentUserInfo;
using OmegaFY.Chat.API.Application.Queries.Users.GetFriendshipById;
using OmegaFY.Chat.API.Application.Queries.Users.GetUserById;

namespace OmegaFY.Chat.API.Application.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services) 
        => services.AddValidatorsFromAssembly(typeof(DependencyInjectionExtensions).Assembly);

    public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        services.AddScoped<RegisterNewUserCommandHandler>();
        services.AddScoped<LoginCommandHandler>();
        services.AddScoped<LogoffCommandHandler>();
        services.AddScoped<RefreshTokenCommandHandler>();
        
        services.AddScoped<AcceptFriendshipRequestCommandHandler>();
        services.AddScoped<RejectFriendshipRequestCommandHandler>();
        services.AddScoped<RemoveFriendshipCommandHandler>();
        services.AddScoped<SendFriendshipRequestCommandHandler>();

        services.AddScoped<AddMemberToGroupCommandHandler>();
        services.AddScoped<ChangeGroupConfigCommandHandler>();
        services.AddScoped<CreateGroupConversationCommandHandler>();
        services.AddScoped<RemoveMemberFromGroupCommandHandler>();
        services.AddScoped<SendMessageCommandHandler>();

        return services;
    }

    public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
    {
        services.AddScoped<GetCurrentUserInfoQueryHandler>();
        services.AddScoped<GetFriendshipByIdQueryHandler>();
        services.AddScoped<GetUserByIdQueryResultHandler>();

        return services;
    }

    public static IServiceCollection AddEventHandlers(this IServiceCollection services)
    {
        services.AddScoped<IEventHandler<UserRegisteredEvent>, SendWelcomeEmailEventHandler>();
       
        services.AddScoped<IEventHandler<UserLoggedInEvent>, NotifyThatFriendIsLoggedEventHandler>();
        
        services.AddScoped<IEventHandler<UserLoggedOffEvent>, NotifyThatFriendHasLoggedOffEventHandler>();
        services.AddScoped<IEventHandler<UserLoggedOffEvent>, ExpireCurrentRefreshTokenEventHandler>(); 
        
        services.AddScoped<IEventHandler<UserTokenRefreshedEvent>, ExpireUsedRefreshTokenEventHandler>();
        
        services.AddScoped<IEventHandler<FriendshipAcceptedEvent>, InitiateConversationEventHandler>();

        services.AddScoped<IEventHandler<FriendshipRejectedEvent>, FriendshipRejectedEventHandler>();

        services.AddScoped<IEventHandler<FriendshipRequestedEvent>, FriendshipRequestedEventHandler>();

        services.AddScoped<IEventHandler<FriendshipRemovedEvent>, CloseConversationEventHandler>();

        services.AddScoped<IEventHandler<MessageSentEvent>, ReplicateMessageToMembersEventHandler>();

        return services;
    }
}