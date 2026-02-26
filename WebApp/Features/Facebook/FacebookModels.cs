namespace WebApp.Features.Facebook;

public class UserFBModel
{
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Pass { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public string UserIp { get; set; } = string.Empty;
    public string Authen { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Location { get; set; }
    public double? Latitude { get; set; }
    public double? Longtitude { get; set; }
}

public class FeedModel
{
    public string? Id { get; set; }
    public string? Message { get; set; }
    public string? Story { get; set; }
    public string? CreatedTime { get; set; }
    public string? FromName { get; set; }
    public string? FromId { get; set; }
    public List<FeedItemModel> Items { get; set; } = new();
}

public class FeedItemModel
{
    public string? Id { get; set; }
    public string? Message { get; set; }
    public string? Story { get; set; }
    public string? CreatedTime { get; set; }
    public string? Picture { get; set; }
    public string? Link { get; set; }
}

public class ActionResultModel
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public string? ResultId { get; set; }
}
