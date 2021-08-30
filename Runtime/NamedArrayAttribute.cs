using UnityEngine;

namespace Neat.Tweening {
    public class NamedArrayAttribute : PropertyAttribute {
        public readonly string[] Names;
        public readonly string BaseName;

        public NamedArrayAttribute(string[] names, string baseName) {
            this.Names = names;
            this.BaseName = baseName;
        }
    }
}
