using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Quest
{
    public interface IEvaluator
    {
        public bool? Evaluate(Predicate predicate, string[] args);
    }
}
