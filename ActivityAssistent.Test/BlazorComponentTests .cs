using ActivityAssistent.App.Interfaces.Audio;
using ActivityAssistent.App.Interfaces.ActionPoint;
using ActivityAssistent.Shared.Dtos.ActionPoints;
using ActivityAssistent.Shared.Dtos.Identity;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using Xunit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable 0619

namespace ActivityAssistent.Test;

public class BlazorComponentTests
{
    private static TestContext CreateContext()
    {
        var context = new TestContext();
        context.JSInterop.SetupVoid("mudKeyInterceptor.connect", _ => true);
        context.Services.AddMudServices();
        context.Services.AddScoped<IAudioRecorderService, FakeAudioRecorderService>();
        return context;
    }

    private static TestContext CreateActionPointsContext(FakeActionPointService actionPointService)
    {
        var context = new TestContext();
        context.JSInterop.SetupVoid("mudKeyInterceptor.connect", _ => true);
        context.Services.AddMudServices();
        context.Services.AddSingleton<IActionPointService>(actionPointService);
        return context;
    }

    [Fact]
    public async Task RecordPage_RendersHeadingAndDescription()
    {
        await using var context = CreateContext();

        var component = context.Render<global::ActivityAssistent.App.Components.Pages.SalesConversations.Record>(
            parameters => parameters.Add(p => p.ConversationId, Guid.NewGuid())
        );

        Assert.Contains("Record conversation", component.Markup);
        Assert.Contains("Start and stop a recording", component.Markup);
    }

    [Fact]
    public async Task RecordPage_ShowsReadyStatus_AndSaveButtonDisabled()
    {
        await using var context = CreateContext();

        var component = context.Render<global::ActivityAssistent.App.Components.Pages.SalesConversations.Record>(
            parameters => parameters.Add(p => p.ConversationId, Guid.NewGuid())
        );

        Assert.Contains("Ready to record.", component.Markup);

        var saveButton = component.FindAll("button")
            .FirstOrDefault(button => button.TextContent.Contains("Save & AI Analysis"));

        Assert.NotNull(saveButton);
        Assert.True(saveButton.HasAttribute("disabled"));
    }

    [Fact]
    public async Task RecordPage_DoesNotRenderAudioPlayer_WhenNoRecording()
    {
        await using var context = CreateContext();

        var component = context.Render<global::ActivityAssistent.App.Components.Pages.SalesConversations.Record>(
            parameters => parameters.Add(p => p.ConversationId, Guid.NewGuid())
        );

        Assert.Empty(component.FindAll("audio"));
    }

    [Fact]
    public async Task ActionPointsPage_RendersActionPointRow()
    {
        var conversationId = Guid.NewGuid();
        var actionPoint = new ActionPointDto
        {
            ActionPointId = Guid.NewGuid(),
            ConversationId = conversationId,
            Description = "Follow up",
            SalesUserId = Guid.NewGuid(),
            DueDate = DateTime.Today,
            IsCompleted = false,
            SubNrId = Guid.NewGuid()
        };
        var userProfile = new UserProfileDto
        {
            UserId = actionPoint.SalesUserId,
            FullName = "Test User",
            SubNrId = actionPoint.SubNrId
        };
        var actionPointService = new FakeActionPointService(actionPoint, userProfile);

        await using var context = CreateActionPointsContext(actionPointService);

        var component = context.Render<global::ActivityAssistent.App.Components.Pages.SalesConversations.ActionPoints>(
            parameters => parameters.Add(p => p.ConversationId, conversationId)
                                  .AddCascadingValue(userProfile)
        );

        Assert.Contains("Follow up", component.Markup);
        Assert.Contains("Open", component.Markup);
    }

    [Fact]
    public async Task ActionPointsPage_BackButton_NavigatesToDetail()
    {
        var conversationId = Guid.NewGuid();
        var actionPoint = new ActionPointDto
        {
            ActionPointId = Guid.NewGuid(),
            ConversationId = conversationId,
            Description = "Follow up",
            SalesUserId = Guid.NewGuid(),
            DueDate = DateTime.Today,
            IsCompleted = false,
            SubNrId = Guid.NewGuid()
        };
        var userProfile = new UserProfileDto
        {
            UserId = actionPoint.SalesUserId,
            FullName = "Test User",
            SubNrId = actionPoint.SubNrId
        };
        var actionPointService = new FakeActionPointService(actionPoint, userProfile);

        await using var context = CreateActionPointsContext(actionPointService);

        var component = context.Render<global::ActivityAssistent.App.Components.Pages.SalesConversations.ActionPoints>(
            parameters => parameters.Add(p => p.ConversationId, conversationId)
                                  .AddCascadingValue(userProfile)
        );

        var backButton = component.FindAll("button").FirstOrDefault(button => button.TextContent.Contains("Terug naar gesprek"));
        Assert.NotNull(backButton);
        backButton.Click();

        var navigationManager = context.Services.GetRequiredService<NavigationManager>();
        Assert.EndsWith($"/SalesConversation/Detail/{conversationId}", navigationManager.Uri, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task ActionPointsPage_ToggleCompleted_UpdatesStatus()
    {
        var conversationId = Guid.NewGuid();
        var actionPoint = new ActionPointDto
        {
            ActionPointId = Guid.NewGuid(),
            ConversationId = conversationId,
            Description = "Follow up",
            SalesUserId = Guid.NewGuid(),
            DueDate = DateTime.Today,
            IsCompleted = false,
            SubNrId = Guid.NewGuid()
        };
        var userProfile = new UserProfileDto
        {
            UserId = actionPoint.SalesUserId,
            FullName = "Test User",
            SubNrId = actionPoint.SubNrId
        };
        var actionPointService = new FakeActionPointService(actionPoint, userProfile);

        await using var context = CreateActionPointsContext(actionPointService);

        var component = context.Render<global::ActivityAssistent.App.Components.Pages.SalesConversations.ActionPoints>(
            parameters => parameters.Add(p => p.ConversationId, conversationId)
                                  .AddCascadingValue(userProfile)
        );

        Assert.Contains("Open", component.Markup);

        var doneButton = component.FindComponents<MudIconButton>()
            .First(button => button.Instance.Icon == Icons.Material.Filled.Done);

        doneButton.Find("button").Click();

        // Geeft Blazor de tijd om de statusverandering asynchroon te verwerken
        component.WaitForAssertion(() => Assert.Contains("Voltooid", component.Markup));
    }

    private sealed class FakeAudioRecorderService : IAudioRecorderService
    {
        public bool IsRecording { get; private set; }

        public Task StartRecordingAsync()
        {
            IsRecording = true;
            return Task.CompletedTask;
        }

        public Task<Stream> StopRecordingAsync()
        {
            IsRecording = false;
            return Task.FromResult<Stream>(new MemoryStream(new byte[] { 1, 2, 3 }));
        }
    }

    private sealed class FakeActionPointService : IActionPointService
    {
        private readonly ActionPointDto _actionPoint;
        private readonly UserProfileDto _userProfile;

        public FakeActionPointService(ActionPointDto actionPoint, UserProfileDto userProfile)
        {
            _actionPoint = actionPoint;
            _userProfile = userProfile;
        }

        public Task<ActionPointDto> CreateActionPointAsync(CreateActionPointDto ActionPoint, CancellationToken Token)
        {
            throw new NotSupportedException();
        }

        public Task<ActionPointDto> UpdateActionPointAsync(UpdateActionPointDto UpdatedActionPoint, CancellationToken Token)
        {
            var updated = new ActionPointDto
            {
                ActionPointId = UpdatedActionPoint.ActionPointId,
                ConversationId = UpdatedActionPoint.ConversationId,
                Description = UpdatedActionPoint.Description,
                SalesUserId = UpdatedActionPoint.SalesUserId,
                DueDate = UpdatedActionPoint.DueDate,
                IsCompleted = UpdatedActionPoint.IsCompleted,
                SubNrId = UpdatedActionPoint.SubNrId
            };

            _actionPoint.IsCompleted = updated.IsCompleted;
            return Task.FromResult(updated);
        }

        public Task<bool> DeleteActionPointAsync(Guid ActionPointId, CancellationToken Token)
        {
            throw new NotSupportedException();
        }

        public Task<ActionPointDto> GetActionPointByIdAsync(Guid ActionPointId, CancellationToken Token)
        {
            throw new NotSupportedException();
        }

        public Task<List<ActionPointDto>> GetActiveActionPointsAsync(string UserId, CancellationToken Token)
        {
            return Task.FromResult(new List<ActionPointDto> { _actionPoint });
        }

        public Task<List<ActionPointDto>> GetByConversationIdAsync(Guid ConversationId, CancellationToken Token)
        {
            return Task.FromResult(new List<ActionPointDto> { _actionPoint });
        }

        public Task<List<UserProfileDto>> GetDelegationUsersAsync(CancellationToken Token)
        {
            return Task.FromResult(new List<UserProfileDto> { _userProfile });
        }
    }
}