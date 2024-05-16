using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MauiApp1.Models;
using MauiApp1.Services.ApiCallServices;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;

namespace MauiApp1.Components.Pages
{
    public partial class ItemsMaster : ComponentBase
    {
        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(100);
        }

        protected override async Task OnParametersSetAsync()
        {
            if (QString1 != "0")
            {
                await LoadParameterDataAsync();
            }
            else
            {
                M0 = new();
            }
        }

        private async Task LoadParameterDataAsync()
        {
            var dt = ApiCall.Deserialize<DataTable>(await ApiCall.CallOneAsync($"SELECT ID, Name, Price, CAST(InternalImage AS VARCHAR(MAX)) [InternalImage] FROM dbo.Items WHERE ID = {QString1}", IApiCall.ApiGetDataTable));
            M0.Id = Convert.ToInt32(dt.Rows[0]["ID"]);
            M0.Price = Convert.ToDouble(dt.Rows[0]["Price"]);
            M0.Name = Convert.ToString(dt.Rows[0]["Name"]);
            M0.Image = !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["InternalImage"])) ? Convert.ToString(dt.Rows[0]["InternalImage"]) : null;
        }


        async void LoadImage(InputFileChangeEventArgs e)
        {
            var imageFiles = e.GetMultipleFiles();
            const string format = "image/png";
            if (imageFiles.Count > 0)
            {
                var file = imageFiles.First();
                var task = await file.RequestImageFileAsync(format, 100, 100);
                await using var stream = task.OpenReadStream();
                var buffer = new byte[task.Size];
                await stream.ReadAsync(buffer, 0, buffer.Length);
                M0.Image = $"data:image/png;base64,{Convert.ToBase64String(buffer)}";
                await InvokeAsync(StateHasChanged);
            }
        }


        void DeleteImage()
        {
            M0.Image = null;
        }


        private async void OnSubmit(EditContext context)
        {
            if (!context.Validate())
            {
                return;
            }

            var spParameters = new Dictionary<string, object>
            {
                { "ID", M0.Id ?? 0 },
                { "Name",M0.Name},
                { "Price", M0.Price },
                { "InternalImage", M0.Image },
            };

            var returnValue = ApiCall.ReturnVal(await ApiCall.CallTwoAsync(spParameters, "Items_Insert"));

            await SweetAlert.ShowAsync(returnValue.Item1, returnValue.Item2, returnValue.Item3, NavigationManager.ToAbsoluteUri(NavigationManager.Uri));
        }

        private ItemsModel M0 = new();

        [Parameter, SupplyParameterFromQuery(Name = "QString1")] public string QString1 { get; set; } = "0";
        [Parameter, SupplyParameterFromQuery(Name = "UId")] public string UId { get; set; }
    }
}
