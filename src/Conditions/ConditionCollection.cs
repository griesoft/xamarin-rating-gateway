using System.Collections.Generic;
using System.Linq;
using Griesoft.Xamarin.RatingGateway.Cache;

namespace Griesoft.Xamarin.RatingGateway.Conditions
{
    /// <summary>
    /// A collection class for <see cref="IRatingCondition"/>'s.
    /// </summary>
    public class ConditionCollection
    {
        private readonly Dictionary<string, IRatingCondition> _conditions;

        /// <summary>
        /// 
        /// </summary>
        public ConditionCollection()
        {
            _conditions = new Dictionary<string, IRatingCondition>();
            RatingConditionCache = new DefaultRatingConditionCache();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IRatingCondition this[string key] => _conditions[key];

        /// <summary>
        /// The rating condition cache that does manage the reading and writing of cached values to the file system.
        /// </summary>
        public IRatingConditionCache RatingConditionCache { get; set; }

        /// <summary>
        /// The condition collection returned as an enumerable.
        /// </summary>
        public IEnumerable<KeyValuePair<string, IRatingCondition>> RatingConditions => _conditions.AsEnumerable();

        /// <summary>
        /// True if the condition collection contains any condition that is of type <see cref="ConditionType.Prerequisite"/>.
        /// </summary>
        public bool HasPrerequisiteConditions => _conditions.Any(condition => condition.Value.ConditionType == ConditionType.Prerequisite);

        /// <summary>
        /// True if the condition collection contains any condition that is of type <see cref="ConditionType.Requirement"/>.
        /// </summary>
        public bool HasRequiredConditions => _conditions.Any(condition => condition.Value.ConditionType == ConditionType.Requirement);

        /// <summary>
        /// True if the condition collection contains only conditions that are of type <see cref="ConditionType.Prerequisite"/>.
        /// </summary>
        /// <remarks>
        /// Make sure that this always returns false. If it doesn't, the rating gateway will never prompt the user to review your application. 
        /// </remarks>
        public bool HasOnlyPrerequisiteConditions => _conditions.Count > 0 && _conditions.All(condition => condition.Value.ConditionType == ConditionType.Prerequisite);


        /// <summary>
        /// Add a condition to the collection.
        /// </summary>
        /// <param name="conditionName">The unique name of the condition.</param>
        /// <param name="condition">The condition instance that will be added to the collection.</param>
        /// <exception cref="System.ArgumentException">Thrown if a condition with the given name already exists in the collection.</exception>
        public virtual void AddCondition(string conditionName, IRatingCondition condition)
        {
            _conditions.Add(conditionName, condition);

            if (condition is ICachableCondition cachableCondition && cachableCondition.CacheCurrentValue)
            {
                if (!RatingConditionCache.Load(conditionName, cachableCondition))
                {
                    RatingConditionCache.Save(conditionName, cachableCondition);
                }
            }
        }

        /// <summary>
        /// Add multiple conditions to the collection at once.
        /// </summary>
        /// <param name="conditions">A collection of key-value pairs where the key is used as the unique condition name and the value is added to the condition collection.</param>
        /// <exception cref="System.ArgumentException">Thrown if a condition with the given name already exists in the collection.</exception>
        public virtual void AddCondition(IEnumerable<KeyValuePair<string, IRatingCondition>> conditions)
        {
            foreach (var condition in conditions)
            {
                AddCondition(condition.Key, condition.Value);
            }
        }

        /// <summary>
        /// Remove a condition with the specified name from the collection if it exists.
        /// </summary>
        /// <param name="conditionName">The unique name of the condition.</param>
        /// <param name="removeFromCache">If true a cached state of the condition will also be removed from the cache. By default true.</param>
        public virtual void RemoveCondition(string conditionName, bool removeFromCache = true)
        {
            _conditions.Remove(conditionName);

            if (removeFromCache)
            {
                RatingConditionCache.Delete(conditionName);
            }
        }

        /// <summary>
        /// Reset all condition states in the collection.
        /// </summary>
        public virtual void ResetAllConditions()
        {
            foreach (var condition in _conditions)
            {
                condition.Value.Reset();

                SaveCachableConditionState(condition.Key, condition.Value);
            }
        }

        /// <summary>
        /// Check whether the collections contains the <paramref name="key"/> or not.
        /// </summary>
        /// <param name="key"></param>
        /// <returns><c>true</c> if the collections contains the key, otherwise <c>false</c>.</returns>
        public virtual bool ContainsKey(string key)
        {
            return _conditions.ContainsKey(key);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditionName"></param>
        /// <param name="condition"></param>
        protected void SaveCachableConditionState(string conditionName, IRatingCondition condition)
        {
            if (condition is ICachableCondition cachableCondition && cachableCondition.CacheCurrentValue)
            {
                RatingConditionCache.Save(conditionName, cachableCondition);
            }
        }
    }
}
