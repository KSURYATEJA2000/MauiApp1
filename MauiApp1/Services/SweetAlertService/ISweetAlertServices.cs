using CurrieTechnologies.Razor.SweetAlert2;

namespace MauiApp1.Services.SweetAlertService;

public interface ISweetAlertServices
{
    public Task<SweetAlertResult> ShowAsync(string title, string message, SweetAlertIcon icon, bool showConfirmButton = true, string confirmButtonText = "OK !", string cancelButtonText = "NO", bool showCancelButton = false);
    public Task ShowAsync(int returnValue, string returnType, string returnMessage, Uri callBackUrl);
    public Task ShowAsync(int returnValue, string returnType, string returnMessage, Uri callBackUrl, string queryString);
    public Task ShowAsync(int returnValue, string returnType, string returnMessage, Uri callBackUrl, string reportName, string recordNo);
}