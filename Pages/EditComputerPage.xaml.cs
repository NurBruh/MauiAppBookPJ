using MauiAppBookPJ.Models;

namespace MauiAppBookPJ.Pages;

public partial class EditComputerPage : ContentPage
{
    private Computer _computer;
    private string selectedImage;

    public EditComputerPage(Computer computer)
    {
        InitializeComponent();
        _computer = computer;
        FillFields();
    }

    private void FillFields()
    {
        modelEntry.Text = _computer.Model;
        cpuEntry.Text = _computer.CPU;
        gpuEntry.Text = _computer.GPU;
        ramEntry.Text = _computer.RAM;
        storageEntry.Text = _computer.Storage;
        priceEntry.Text = _computer.Price.ToString("F2");
        selectedImage = _computer.ImagePath;

        imagePreview.Source = selectedImage;
        imagePreview.IsVisible = true;
    }

    private void OnImageTapped(object sender, TappedEventArgs e)
    {
        if (e?.Parameter is string image)
        {
            selectedImage = image;
            imagePreview.Source = selectedImage;
            imagePreview.IsVisible = true;
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(modelEntry.Text) ||
            string.IsNullOrWhiteSpace(cpuEntry.Text) ||
            string.IsNullOrWhiteSpace(gpuEntry.Text) ||
            string.IsNullOrWhiteSpace(ramEntry.Text) ||
            string.IsNullOrWhiteSpace(storageEntry.Text) ||
            !decimal.TryParse(priceEntry.Text, out var price) ||
            string.IsNullOrEmpty(selectedImage))
        {
            await DisplayAlert("������", "��������� ��� ���� � �������� �����������.", "OK");
            return;
        }

        _computer.Model = modelEntry.Text;
        _computer.CPU = cpuEntry.Text;
        _computer.GPU = gpuEntry.Text;
        _computer.RAM = ramEntry.Text;
        _computer.Storage = storageEntry.Text;
        _computer.Price = price;
        _computer.ImagePath = selectedImage;

        await App.DbService.UpdateComputerAsync(_computer);
        await DisplayAlert("�������", "���������� ���������", "OK");
        await Navigation.PopAsync();
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        var answer = await DisplayAlert("�������", "�� �������, ��� ������ ������� ��?", "��", "���");
        if (answer)
        {
            await App.DbService.DeleteComputerAsync(_computer);
            await Navigation.PopAsync();
        }
    }
}