namespace WebApp.Features.Facebook;

public interface IFacebookService
{
    Task<string?> PostMessageToPageAsync(string message);
    Task<UserFBModel> Login(UserFBModel model);
    Task<UserFBModel> Logout(UserFBModel model);
    Task<FeedModel> NewFeed(UserFBModel model);
    Task<ActionResultModel> PostImage(UserFBModel model, byte[] imageData, string fileName);
    Task<UserFBModel> PostStatus(UserFBModel model, string statusMessage);
    Task<UserFBModel> ReadNotify(UserFBModel model);
    Task<UserFBModel> RepMess(UserFBModel model, string recipientId, string message);
    Task<UserFBModel> Search(UserFBModel model, string query);
    Task<UserFBModel> AddFriend(UserFBModel model, string friendId);
}
