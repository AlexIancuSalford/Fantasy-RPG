using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.UI.Quest
{
    [System.Serializable]
    public class Condition
    {
        [field : SerializeField] public string Predicate { get; set; } = string.Empty;
        [field : SerializeField] public string[] Args { get; set; } = null;

        public bool CheckEvaluators(IEnumerable<IEvaluator> evaluators)
        {
            return evaluators
                .Select(evaluator => evaluator.Evaluate(Predicate, Args))
                .Where(result => result.HasValue)
                .All(result => result.Value);
        }
    }
}
