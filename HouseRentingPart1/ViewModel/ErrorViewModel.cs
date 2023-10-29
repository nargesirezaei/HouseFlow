namespace HouseFlowPart1.Models;

public class ErrorViewModel
{
    // Gets or sets the RequestId to identify the error.
    public string? RequestId { get; set; }
    // Determines whether to show the RequestId.
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}

