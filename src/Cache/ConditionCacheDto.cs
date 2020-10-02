using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Griesoft.Xamarin.RatingGateway.Cache
{
    /// <summary>
    /// 
    /// </summary>
    public class ConditionCacheDto
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string ConditionName { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [DisallowNull]
        [JsonProperty(TypeNameHandling = TypeNameHandling.All, ItemTypeNameHandling = TypeNameHandling.All)]
        public object? CurrentValue { get; set; }
    }
}
