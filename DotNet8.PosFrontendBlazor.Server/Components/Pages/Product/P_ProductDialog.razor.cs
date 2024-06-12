﻿using DotNet8.PosFrontendBlazor.Server.Models.Product;
using DotNet8.PosFrontendBlazor.Server.Models.ProductCategory;

namespace DotNet8.PosFrontendBlazor.Server.Components.Pages.Product;

public partial class P_ProductDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }

    private ProductRequestModel requestModel = new();

    private void Cancel() => MudDialog.Cancel();

    private ProductCategoryListResponseModel? categoryResponseModel;

    protected override async void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            await InjectService.EnableLoading();
            await ProductCategoryCodeList();
            StateHasChanged();
            await InjectService.DisableLoading();
        }
    }
    private async Task ProductCategoryCodeList()
    {
        categoryResponseModel = await HttpClientService.ExecuteAsync<ProductCategoryListResponseModel>
        (
            Endpoints.ProductCategory,
            EnumHttpMethod.Get
        );
    }

    private async Task SaveAsync()
    {
        if (validate())
        {
            var response = await HttpClientService.ExecuteAsync<ProductResponseModel>(
                Endpoints.Product,
                EnumHttpMethod.Post,
                requestModel
            );
            if (response.IsError)
            {
                InjectService.ShowMessage(response.Message, EnumResponseType.Error);
                return;
            }

            InjectService.ShowMessage(response.Message, EnumResponseType.Success);
            MudDialog.Close();
        }
    }
    private bool validate()
    {
        if (string.IsNullOrEmpty(requestModel.ProductName))
        {
            ShowWarningMessage("Product Name is required.");
            return false;
        }
        if (string.IsNullOrEmpty(requestModel.ProductCategoryCode))
        {
            ShowWarningMessage("Product Category is required.");
            return false;
        }
        if (!(requestModel.Price > 0))
        {
            ShowWarningMessage("Product Price must be greater than zero.");
            return false;
        }
        return true;
    }
    private void ShowWarningMessage(string message)
    {
        InjectService.ShowMessage(message, EnumResponseType.Warning);
    }
}