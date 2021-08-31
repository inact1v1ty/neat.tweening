using System;
using System.Linq;
using UnityEngine;

namespace Neat.Tweening {
    public static partial class Extensions {
        public static T GetComponentInParentWhere<T>(this Transform transform, Func<T, bool> predicate)
            where T : Component {
            var currentTransform = transform;

            while (true) {
                var maybeT = currentTransform
                    .GetComponents<T>()
                    .FirstOrDefault(predicate);

                if (maybeT != null) {
                    return maybeT;
                }

                currentTransform = currentTransform.parent;

                if (currentTransform == null) {
                    return null;
                }
            }
        }
    }
}
