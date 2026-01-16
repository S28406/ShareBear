using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Pro.Client.Services;
using Pro.Shared.Dtos;

namespace Pro.Client.Views;

public partial class AdminUsersPage : Page
{
    private ObservableCollection<AdminUserRowVm> _rows = new();

    public AdminUsersPage()
    {
        InitializeComponent();
        UsersGrid.ItemsSource = _rows;

        Loaded += async (_, __) => await LoadAsync();
    }

    private async Task LoadAsync()
    {
        try
        {
            if (AppState.CurrentUser?.Role != "Admin")
            {
                MessageBox.Show("Admins only.");
                return;
            }

            var q = (SearchBox.Text ?? "").Trim();
            var users = await Api.Instance.AdminGetUsersAsync(string.IsNullOrWhiteSpace(q) ? null : q);

            var vm = users.Select(u => new AdminUserRowVm(u)).ToList();
            _rows = new ObservableCollection<AdminUserRowVm>(vm);
            UsersGrid.ItemsSource = _rows;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Failed to load users:\n" + ex.Message, "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void Search_Click(object sender, RoutedEventArgs e) => await LoadAsync();

    private async void Reset_Click(object sender, RoutedEventArgs e)
    {
        SearchBox.Text = "";
        await LoadAsync();
    }

    private async void SearchBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) await LoadAsync();
    }

    private async void SaveRole_Click(object sender, RoutedEventArgs e)
    {
        if (((FrameworkElement)sender).DataContext is not AdminUserRowVm row)
            return;

        try
        {
            row.IsSaving = true;

            await Api.Instance.AdminUpdateUserRoleAsync(row.Id, new UpdateUserRoleRequestDto(row.SelectedRole));

            row.OriginalRole = row.SelectedRole;
            row.MarkSaved();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Failed to update role:\n" + ex.Message, "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            row.IsSaving = false;
        }
    }
}

public class AdminUserRowVm : INotifyPropertyChanged
{
    public Guid Id { get; }
    public string Username { get; }
    public string Email { get; }

    public ObservableCollection<string> Roles { get; } =
        new(new[] { "Customer", "Seller", "Admin" });

    private string _originalRole;
    public string OriginalRole
    {
        get => _originalRole;
        set
        {
            var v = NormalizeRole(value);
            if (_originalRole == v) return;
            _originalRole = v;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanSave));
            OnPropertyChanged(nameof(SaveButtonText));
        }
    }

    private string _selectedRole;
    public string SelectedRole
    {
        get => _selectedRole;
        set
        {
            var v = NormalizeRole(value);
            if (_selectedRole == v) return;
            _selectedRole = v;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanSave));
            OnPropertyChanged(nameof(SaveButtonText));
        }
    }

    private bool _isSaving;
    public bool IsSaving
    {
        get => _isSaving;
        set
        {
            if (_isSaving == value) return;
            _isSaving = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanSave));
            OnPropertyChanged(nameof(SaveButtonText));
        }
    }

    public bool CanSave =>
        !IsSaving &&
        !string.Equals(SelectedRole, OriginalRole, StringComparison.OrdinalIgnoreCase);

    public string SaveButtonText => IsSaving ? "Saving..." : (CanSave ? "Save" : "Saved");

    public AdminUserRowVm(AdminUserRowDto dto)
    {
        Id = dto.Id;
        Username = dto.Username;
        Email = dto.Email;

        _originalRole = NormalizeRole(dto.Role);
        _selectedRole = _originalRole;
    }

    public void MarkSaved()
    {
        OnPropertyChanged(nameof(CanSave));
        OnPropertyChanged(nameof(SaveButtonText));
    }

    private static string NormalizeRole(string? r)
    {
        var role = (r ?? "Customer").Trim();
        if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase)) return "Admin";
        if (role.Equals("Seller", StringComparison.OrdinalIgnoreCase)) return "Seller";
        return "Customer";
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
