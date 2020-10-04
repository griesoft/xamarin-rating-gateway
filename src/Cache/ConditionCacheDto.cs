using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Griesoft.Xamarin.RatingGateway.Cache
{
    /// <summary>
    /// A data transfer object that is used to save and load a value for a specific condition.
    /// </summary>
    public class ConditionCacheDto
    {
        /// <summary>
        /// The name of the condition.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string ConditionName { get; set; } = string.Empty;

        /// <summary>
        /// The current value of the condition.
        /// </summary>
        [DisallowNull]
        [JsonProperty(TypeNameHandling = TypeNameHandling.All, ItemTypeNameHandling = TypeNameHandling.All)]
        public object? CurrentValue { get; set; }
    }
}
