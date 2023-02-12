using Domain.Repositories.UserRepository.Models;
using Infrastructure.Cache.CachedUserView.Mapper;
using Infrastructure.Cache.CachedUserView.Models;
using Infrastructure.Cache.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Infrastructure.Cache.CachedUserView
{
    internal class CachedUserViewRepository : ICachedUserViewRepository
    {
        private readonly IDistributedCache _cache;
        private const string UserSettingsKey = "user_settings_{0}";
        private static JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
        };

        public CachedUserViewRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<UserControlView> GetUserById(long id, CancellationToken cancellationToken)
        {
            try
            {
                var key = string.Format(UserSettingsKey, id);

                var data = await _cache.GetStringAsync(key, cancellationToken);

                var userViewModel = JsonSerializer.Deserialize<UserControlView>(data, _options);

                return userViewModel;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task UpdateUserViewAsync(UpdateCachedUserControlViewInput input, CancellationToken cancellationToken)
        {
            try
            {
                var key = string.Format(UserSettingsKey, input.Id);

                var data = await _cache.GetStringAsync(key, cancellationToken);

                if (data is null)
                {
                    var firstCachedUserControlView = input.MapInputToUserControlView();

                    await _cache.SetStringAsync(key, firstCachedUserControlView, cancellationToken);

                    return;
                }

                var oldCachedUserControlViewModel = JsonSerializer.Deserialize<UserControlView>(data!, _options);

                var newCachedUserControlView = oldCachedUserControlViewModel!.MapInputToUserControlView(input);

                await _cache.SetStringAsync(key, newCachedUserControlView, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
