using AuthorizeOnRoute.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;

namespace AuthorizeOnRoute.Utilities;

public class BlazorSchoolUserService
{
    private readonly ProtectedLocalStorage _protectedLocalStorage;
    private readonly string _blazorSchoolStorageKey = "blazorSchoolIdentity";

    public BlazorSchoolUserService(ProtectedLocalStorage protectedLocalStorage)
    {
        _protectedLocalStorage = protectedLocalStorage;
    }

    public async Task<User?> FindUserFromDatabaseAsync(string username, string password)
    {
        var userListFromDatabase = new List<User>()
        {
            new()
            {
                Username = "blazorschool-normal",
                Password = "blazorschool",
                Roles = new()
                {
                    "normal_user"
                },
                Age = 12
            },
            new()
            {
                Username = "blazorschool-paid",
                Password = "blazorschool",
                Roles = new()
                {
                    "normal_user",
                    "paid_user"
                },
                Age = 14
            },
            new()
            {
                Username = "blazorschool-admin",
                Password = "blazorschool",
                Roles = new()
                {
                    "normal_user",
                    "paid_user",
                    "admin"
                },
                Age = 20
            }
        };

        var userInDatabase = userListFromDatabase.Single(u => u.Username == username && u.Password == password);

        if (userInDatabase is not null)
        {
            await PersistUserToBrowserAsync(userInDatabase);
        }

        return userInDatabase;
    }

    public async Task PersistUserToBrowserAsync(User user)
    {
        string userJson = JsonConvert.SerializeObject(user);
        await _protectedLocalStorage.SetAsync(_blazorSchoolStorageKey, userJson);
    }

    public async Task<User?> FetchUserFromBrowserAsync()
    {
        try // When Blazor Server is rendering at server side, there is no local storage. Therefore, put an empty try catch to avoid error
        {
            var fetchedUserResult = await _protectedLocalStorage.GetAsync<string>(_blazorSchoolStorageKey);

            if (fetchedUserResult.Success && !string.IsNullOrEmpty(fetchedUserResult.Value))
            {
                var user = JsonConvert.DeserializeObject<User>(fetchedUserResult.Value);

                return user;
            }
        }
        catch
        {
        }

        return null;
    }

    public async Task ClearBrowserUserDataAsync() => await _protectedLocalStorage.DeleteAsync(_blazorSchoolStorageKey);
}