using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Griesoft.Xamarin.RatingGateway.Helpers;
using Newtonsoft.Json;

namespace Griesoft.Xamarin.RatingGateway.Cache
{
    /// <summary>
    /// A default implementation of <see cref="IRatingConditionCache"/>, which is used by the rating gateway. Condition names and values
    /// will be deserialized and serialized to JSON and are written to a text file in the app directory of the application.
    /// </summary>
    /// <remarks>
    /// Platforms like Android and iOS do automatically backup the contents of the app directory. So if you don't want the system to backup
    /// your condition cache, you will need to disable it for the file that this cache is using. By default the file name is "RatingConditionCache".
    /// You may also create a custom implementation of <see cref="IRatingConditionCache"/> and use this default implementation as template.
    /// </remarks>
    public class DefaultRatingConditionCache : IRatingConditionCache
    {
        private readonly string _conditionCacheFileName;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly Lazy<List<ConditionCacheDto>> _lazyConditionCacheDtos;

        /// <summary>
        /// 
        /// </summary>
        public DefaultRatingConditionCache() : this("RatingConditionCache") { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">The name of the file that the cache will use or create if it doesn't exist.</param>
        public DefaultRatingConditionCache(string fileName)
        {
            _conditionCacheFileName = $"{fileName}.txt";

            _serializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
            _serializerSettings.Converters.Insert(0, new PrimitiveJsonConverter());

            _lazyConditionCacheDtos = new Lazy<List<ConditionCacheDto>>(() =>
                File.Exists(Path.Combine(CacheHelpers.AppDataDirectory, _conditionCacheFileName))
                ? JsonConvert.DeserializeObject<List<ConditionCacheDto>>(File.ReadAllText(Path.Combine(CacheHelpers.AppDataDirectory, _conditionCacheFileName)))
                : new List<ConditionCacheDto>());
        }

        /// <inheritdoc/>
        public bool Load(string conditionName, ICachableCondition condition)
        {
            var cachedValueDto = _lazyConditionCacheDtos.Value.FirstOrDefault(dto => dto.ConditionName == conditionName);

            if (cachedValueDto == null || cachedValueDto.CurrentValue == null)
            {
                return false;
            }

            condition.ManipulateState(cachedValueDto.CurrentValue);

            return true;
        }

        /// <inheritdoc/>
        public void Save(string conditionName, ICachableCondition condition)
        {
            var existingCachedValueDto = _lazyConditionCacheDtos.Value.FirstOrDefault(dto => dto.ConditionName == conditionName);

            if (existingCachedValueDto == null)
            {
                _lazyConditionCacheDtos.Value.Add(condition.ToConditionCacheDto(conditionName));
            }
            else
            {
                _lazyConditionCacheDtos.Value[_lazyConditionCacheDtos.Value.IndexOf(existingCachedValueDto)] = condition.ToConditionCacheDto(conditionName);
            }

            File.WriteAllText(Path.Combine(CacheHelpers.AppDataDirectory, _conditionCacheFileName), JsonConvert.SerializeObject(_lazyConditionCacheDtos.Value, _serializerSettings));
        }

        /// <inheritdoc/>
        public void Delete(string conditionName)
        {
            var cachedValueDto = _lazyConditionCacheDtos.Value.FirstOrDefault(dto => dto.ConditionName == conditionName);

            if (cachedValueDto == null)
            {
                return;
            }

            _lazyConditionCacheDtos.Value.Remove(cachedValueDto);

            File.WriteAllText(Path.Combine(CacheHelpers.AppDataDirectory, _conditionCacheFileName), JsonConvert.SerializeObject(_lazyConditionCacheDtos.Value, _serializerSettings));
        }
    }
}
