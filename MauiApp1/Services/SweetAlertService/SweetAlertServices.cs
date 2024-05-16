using CurrieTechnologies.Razor.SweetAlert2;
using MauiApp1.Services.ApiCallServices;
using Microsoft.AspNetCore.Components;

namespace MauiApp1.Services.SweetAlertService;

public class SweetAlertServices(CurrieTechnologies.Razor.SweetAlert2.SweetAlertService sweetAlert, NavigationManager navigationManager, IApiCall apiCall) : ISweetAlertServices
{
    public async Task<SweetAlertResult> ShowAsync(string title, string message, SweetAlertIcon icon, bool showConfirmButton = true, string confirmButtonText = "OK !", string cancelButtonText = "NO", bool showCancelButton = false) =>
        //return await sweetAlert.FireAsync($"{title.ToUpper()} !", message, icon);
        await sweetAlert.FireAsync(new()
        {
            Title = title.ToUpper(),
            Html = message,
            Icon = icon,
            ShowCancelButton = showCancelButton,
            ShowConfirmButton = showConfirmButton,
            ConfirmButtonText = confirmButtonText,
            CancelButtonText = cancelButtonText
        });

    public async Task ShowAsync(int returnValue, string returnType, string returnMessage, Uri callBackUrl)
    {
        var returnMessages = returnMessage.Split("|");
        var returnMessage1 = returnMessages.ElementAtOrDefault(0) ?? string.Empty;
        var returnMessage2 = returnMessages.ElementAtOrDefault(1) ?? string.Empty;
        var returnIcon = returnValue == 1 ? SweetAlertIcon.Success : SweetAlertIcon.Error;

        await sweetAlert.FireAsync(new()
        {
            Title = returnType.ToUpper(),
            Html = $"""<h4 class="mt-3">{returnMessage1}</h4><span><p><strong>{returnMessage2}<strong></p></span>""",
            Icon = returnIcon,
            ConfirmButtonText = "OK !"
        });

        if (returnValue == 1)
        {
            NavigateWithUniqueId(callBackUrl.ToString());
        }
    }

    public async Task ShowAsync(int returnValue, string returnType, string returnMessage, Uri callBackUrl, string queryString)
    {
        var result = await sweetAlert.FireAsync(new()
        {
            Title = returnType.ToUpper(),
            Html = $"""
                       <span>
                           <h5>
                               <strong>Are you sure want to Delete ?<strong>
                           </h5>
                       </span>
                       <span>
                            <p>
                                <strong>( {returnMessage} )<strong>
                            </p>
                        </span>
                    """,
            Icon = SweetAlertIcon.Warning,
            ShowCancelButton = true,
            ShowConfirmButton = true,
            ConfirmButtonText = "YES",
            CancelButtonText = "NO"
        });

        if (result.IsConfirmed)
        {
            Dictionary<string, object> spParameters = new()
            {
                { "QueryString", queryString },
                { "DeleteValue", returnMessage }
            };
            var returnVal = apiCall.ReturnVal(await apiCall.CallTwoAsync(spParameters, "CITL_Delete"));
            await ShowAsync(returnVal.Item1, returnVal.Item2, returnVal.Item3, callBackUrl);
        }
    }

    public Task ShowAsync(int returnValue, string returnType, string returnMessage, Uri callBackUrl, string reportName, string recordNo) =>
        //var returnMessages = returnMessage.Split("|");
        //var returnMessage1 = string.Empty;
        //string returnMessage2;
        //SweetAlertIcon returnIcon;
        //if (returnValue == 1)
        //{
        //    returnMessage1 = returnMessages[0];
        //    returnMessage2 = returnMessages[1];
        //    returnIcon = SweetAlertIcon.Success;
        //}
        //else
        //{
        //    returnMessage2 = returnMessages[0];
        //    returnIcon = SweetAlertIcon.Error;
        //}
        //if (globalApplicationVariables.EnableAudio)
        //{
        //    await PlayAudio(returnValue);
        //}
        //var result = await sweetAlert.FireAsync(new()
        //{
        //    Title = returnType.ToUpper(),
        //    Html = $"""
        //            <h4 class="mt-3">{returnMessage1}</h4>
        //               <span>
        //                   <h5>
        //                       <strong>Do you want to Print ?<strong>
        //                   </h5>
        //               </span>
        //               <span>
        //                    <p>
        //                        <strong>{returnMessage2}<strong>
        //                    </p>
        //                </span>
        //            """,
        //    Icon = returnIcon,
        //    ShowCancelButton = true,
        //    ShowConfirmButton = true,
        //    ConfirmButtonText = "YES",
        //    CancelButtonText = "NO"
        //});
        //if (result.IsConfirmed)
        //{
        //    var fileName = fileSystem.GetReportName(recordNo, FileExtension.Pdf);
        //    await report.ExportReportAsync(reportName, fileName, recordNo);
        //    var directory = IFileSystemService.TempReportsPath;
        //    var sysDriveFullPath = fileSystem.CombinePath(directory, fileName);
        //    var parameters = new Dictionary<string, object> { { "PdfFileName", fileName }, { "PdfFileFullDrivePath", sysDriveFullPath } };
        //    navigationManager.NavigateTo($"HomeX?QStringX={callBackUrl}");
        //    await cv.Model.ShowAsync(null, typeof(CitlPdfViewer), parameters, "100%");
        //}
        //else
        //{
        //    navigationManager.NavigateTo($"HomeX?QStringX={callBackUrl}");
        //}
        Task.CompletedTask;


    private void NavigateWithUniqueId(string baseUri)
    {
        baseUri = baseUri.Split("&UId")[0];
        var newUri = $"{baseUri}&UId={Guid.NewGuid()}";
        navigationManager.NavigateTo(newUri, false);
    }
}