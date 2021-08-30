using System;
using UnityEngine;

namespace Neat.Tweening {
    public abstract class Accessor {
        public abstract Type ValueType { get; }
    }

    [Serializable]
    public class Accessor<T> : Accessor {
        public override Type ValueType => typeof(T);

        [SerializeField] [HideInInspector] private string componentTypeName;
        [SerializeField] [HideInInspector] private string fieldName;

        private Component target;
        private Func<object, T> getter;
        private Action<object, T> setter;

        public bool Valid => init && getter != null && setter != null && target != null;

        public T Value {
            get {
                if (!Valid) throw new Exception("Accessor is not valid");

                return getter(target);
            }
            set => setter(target, value);
        }

        private bool init = false;

        public void Init(GameObject gameObject) {
            if (init) return;

            if (string.IsNullOrEmpty(componentTypeName) || string.IsNullOrEmpty(fieldName)) return;

            var componentType = Type.GetType(componentTypeName);

            if (componentType == null) {
                Debug.LogError($"Component {componentTypeName} not found on {gameObject.name}");

                return;
            }

            target = gameObject.GetComponent(componentType);
            (getter, setter) = TweenUtility.GetAccess<T>(componentTypeName, fieldName);

            if (getter == null || setter == null) {
                Debug.LogError($"Field or property with name {fieldName} not found on component {componentTypeName}");

                return;
            }

            init = true;
        }
    }
}
