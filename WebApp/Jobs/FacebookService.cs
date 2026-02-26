using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Elect.Data.EF.Interfaces.UnitOfWork;
using WebApp.Data.Interfaces;
using WebApp.Data.Models;
using WebApp.Models;

namespace WebApp.Jobs;

public class FacebookService
{
    private const string GraphApiBaseUrl = "https://graph.facebook.com/v18.0";

    private readonly HttpClient _httpClient;
    private readonly IRepository<UserFBEntity> _userFBRepo;
    private readonly IUnitOfWork _unitOfWork;

    public FacebookService(HttpClient httpClient, IRepository<UserFBEntity> userFBRepo, IUnitOfWork unitOfWork)
    {
        _httpClient = httpClient;
        _userFBRepo = userFBRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<string?> PostMessageToPageAsync(string message)
    {
        // TODO: implement Facebook Graph API call
        await Task.CompletedTask;
        return null;
    }

    // function : usermodel login(usermodel)
    // this function send (POST) data (username, password, deviceid, ip and etc) to facebook api login and recevie authen response and then save authen into user entity apply it to db
    public async Task<UserFBModel> Login(UserFBModel model)
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["email"] = model.Username,
            ["password"] = model.Pass,
            ["device_id"] = model.DeviceId,
            ["client_ip"] = model.UserIp
        });

        var response = await _httpClient.PostAsync($"{GraphApiBaseUrl}/auth/login", content);
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JsonElement>(json);

        if (result.TryGetProperty("access_token", out var tokenElement))
        {
            model.Authen = tokenElement.GetString() ?? string.Empty;
        }

        // Save authen into user entity
        var entity = _userFBRepo.GetSingle(x => x.Username == model.Username);
        if (entity == null)
        {
            entity = new UserFBEntity
            {
                Name = model.Name,
                Username = model.Username,
                Pass = model.Pass,
                DeviceId = model.DeviceId,
                UserIp = model.UserIp,
                Authen = model.Authen,
                Address = model.Address,
                City = model.City,
                Location = model.Location,
                Latitude = model.Latitude,
                Longtitude = model.Longtitude
            };
            _userFBRepo.Add(entity);
        }
        else
        {
            entity.Authen = model.Authen;
            entity.DeviceId = model.DeviceId;
            entity.UserIp = model.UserIp;
            _userFBRepo.Update(entity);
        }

        await _unitOfWork.SaveChangesAsync();

        return model;
    }

    // function : usermodel logout(usermodel)
    // this function clear authen and then save authen into user entity apply it to db
    public async Task<UserFBModel> Logout(UserFBModel model)
    {
        model.Authen = string.Empty;

        var entity = _userFBRepo.GetSingle(x => x.Username == model.Username);
        if (entity != null)
        {
            entity.Authen = string.Empty;
            _userFBRepo.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        return model;
    }

    // function : feedModel newfeed(usermodel)
    // this function send (POST) data (authen, deviceid, ip and etc) to facebook api new feed and recevie response and then return feedModel
    public async Task<FeedModel> NewFeed(UserFBModel model)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.Authen);

        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["device_id"] = model.DeviceId,
            ["client_ip"] = model.UserIp
        });

        var response = await _httpClient.PostAsync($"{GraphApiBaseUrl}/me/feed", content);
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JsonElement>(json);

        var feedModel = new FeedModel();

        if (result.TryGetProperty("data", out var dataElement))
        {
            foreach (var item in dataElement.EnumerateArray())
            {
                feedModel.Items.Add(new FeedItemModel
                {
                    Id = item.TryGetProperty("id", out var id) ? id.GetString() : null,
                    Message = item.TryGetProperty("message", out var msg) ? msg.GetString() : null,
                    Story = item.TryGetProperty("story", out var story) ? story.GetString() : null,
                    CreatedTime = item.TryGetProperty("created_time", out var time) ? time.GetString() : null,
                    Picture = item.TryGetProperty("picture", out var pic) ? pic.GetString() : null,
                    Link = item.TryGetProperty("link", out var link) ? link.GetString() : null
                });
            }
        }

        return feedModel;
    }

    // function : actionResultModel postImage(usermodel)
    // this function send (POST) data (authen, image, deviceid, ip and etc) to facebook api and recevie response and then return actionResultModel
    public async Task<ActionResultModel> PostImage(UserFBModel model, byte[] imageData, string fileName)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.Authen);

        using var formContent = new MultipartFormDataContent();
        formContent.Add(new ByteArrayContent(imageData), "source", fileName);
        formContent.Add(new StringContent(model.DeviceId), "device_id");
        formContent.Add(new StringContent(model.UserIp), "client_ip");

        var response = await _httpClient.PostAsync($"{GraphApiBaseUrl}/me/photos", formContent);
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JsonElement>(json);

        return new ActionResultModel
        {
            IsSuccess = response.IsSuccessStatusCode,
            Message = response.IsSuccessStatusCode ? "Image posted successfully" : json,
            ResultId = result.TryGetProperty("id", out var id) ? id.GetString() : null
        };
    }

    // function : usermodel postStatus(usermodel)
    public async Task<UserFBModel> PostStatus(UserFBModel model, string statusMessage)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.Authen);

        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["message"] = statusMessage,
            ["device_id"] = model.DeviceId,
            ["client_ip"] = model.UserIp
        });

        await _httpClient.PostAsync($"{GraphApiBaseUrl}/me/feed", content);

        return model;
    }

    // function : usermodel readNotify(usermodel)
    public async Task<UserFBModel> ReadNotify(UserFBModel model)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.Authen);

        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["device_id"] = model.DeviceId,
            ["client_ip"] = model.UserIp
        });

        await _httpClient.PostAsync($"{GraphApiBaseUrl}/me/notifications?include_read=true", content);

        return model;
    }

    // function : usermodel repMess(usermodel)
    public async Task<UserFBModel> RepMess(UserFBModel model, string recipientId, string message)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.Authen);

        var payload = new
        {
            recipient = new { id = recipientId },
            message = new { text = message },
            device_id = model.DeviceId,
            client_ip = model.UserIp
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        await _httpClient.PostAsync($"{GraphApiBaseUrl}/me/messages", jsonContent);

        return model;
    }

    // function : usermodel search(usermodel)
    public async Task<UserFBModel> Search(UserFBModel model, string query)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.Authen);

        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["q"] = query,
            ["type"] = "user",
            ["device_id"] = model.DeviceId,
            ["client_ip"] = model.UserIp
        });

        await _httpClient.PostAsync($"{GraphApiBaseUrl}/search", content);

        return model;
    }

    // function : usermodel addFriend(usermodel)
    public async Task<UserFBModel> AddFriend(UserFBModel model, string friendId)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.Authen);

        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["uid"] = friendId,
            ["device_id"] = model.DeviceId,
            ["client_ip"] = model.UserIp
        });

        await _httpClient.PostAsync($"{GraphApiBaseUrl}/me/friends/{friendId}", content);

        return model;
    }
}
