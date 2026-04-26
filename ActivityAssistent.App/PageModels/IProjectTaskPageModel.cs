using ActivityAssistent.App.Models;
using CommunityToolkit.Mvvm.Input;

namespace ActivityAssistent.App.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}