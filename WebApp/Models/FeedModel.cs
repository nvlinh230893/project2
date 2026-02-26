namespace WebApp.Models;

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
