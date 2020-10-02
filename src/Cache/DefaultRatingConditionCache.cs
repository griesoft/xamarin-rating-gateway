using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Griesoft.Xamarin.RatingGateway.Helpers;
using Newtonsoft.Json;

namespace Griesoft.Xamarin.RatingGateway.Cache
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultRatingConditionCache : IRatingConditionCache
    {
        private readonly string _conditionCacheFileName;
        private readonly Lazy<List<ConditionCacheDto>> _lazyConditionCacheDtos;

        /// <summary>
        /// 
        /// </summary>
        public DefaultRatingConditionCache() : this("RatingConditionCache") { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public DefaultRatingConditionCache(string fileName)
        {
            _conditionCacheFileName = $"{fileName}.txt";

            _lazyConditionCacheDtos = new Lazy<List<ConditionCacheDto>>(() =>
                File.Exists(Path.Combine(CacheHelpers.AppDataDirectory, _conditionCacheFileName))
                ? JsonConvert.DeserializeObject<List<ConditionCacheDto>>(File.ReadAllText(Path.Combine(CacheHelpers.AppDataDirectory, _conditionCacheFileName)))
                : new List<ConditionCacheDto>());
        }

        /// <inheritdoc/>
        public void Load(string conditionName, ICachableCondition condition)
        {
            var cachedValueDto = _lazyConditionCacheDtos.Value.FirstOrDefault(dto => dto.ConditionName == conditionName);

            if (cachedValueDto == null || cachedValueDto.CurrentValue == null)
            {
                return;
            }

            condition.ManipulateState(cachedValueDto.CurrentValue);
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

            File.WriteAllText(Path.Combine(CacheHelpers.AppDataDirectory, _conditionCacheFileName), JsonConvert.SerializeObject(_lazyConditionCacheDtos.Value));
        }
    }
}
