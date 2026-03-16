using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Category;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.CategoryMessages;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayUpdatedCategoryViewModel : ViewModelBase
{
    public CategoryAdminData Category { get; }
    public IMessenger _messenger;

    public DisplayUpdatedCategoryViewModel(CategoryAdminData category, IMessenger messenger)
    {
        Category = category;
        _messenger = messenger;
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new NavigateBackToUpdateCategoryMessage());
    }
}
