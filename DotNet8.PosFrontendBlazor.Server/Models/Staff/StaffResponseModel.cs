﻿namespace DotNet8.PosFrontendBlazor.Server.Models.Staff;

public class StaffResponseModel : ResponseModel
{
    public DataModel Data { get; set; } = new DataModel();
}

public class DataModel
{
    public StaffModel Staff { get; set; }
}