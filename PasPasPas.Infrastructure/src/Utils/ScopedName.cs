#nullable disable
using System;

namespace PasPasPas.Infrastructure.Utils {

    /// <summary>
    ///     kind of a scoped name part
    /// </summary>
    public enum ScopedNamePartKind {

        /// <summary>
        ///     undefined
        /// </summary>
        Undefined,

        /// <summary>
        ///     standard item
        /// </summary>
        StartItem,

        /// <summary>
        ///     subitem
        /// </summary>
        SubItem,

        /// <summary>
        ///     generic part
        /// </summary>
        GenericPart
    }

    /// <summary>
    ///     part of a scoped name
    /// </summary>
    public readonly struct ScopedNamePart : IEquatable<ScopedNamePart> {

        private readonly ScopedNamePartKind kind;

        /// <summary>
        ///     create a new scoped part
        /// </summary>
        /// <param name="partValue">part value</param>
        /// <param name="partKind">kind</param>
        public ScopedNamePart(string partValue, ScopedNamePartKind partKind) {
            Value = partValue;
            kind = partKind;
        }

        /// <summary>
        ///     check equality
        /// </summary>
        /// <param name="other">other object to check</param>
        /// <returns><c>true</c> if equal</returns>
        public bool Equals(ScopedNamePart other)
            => other.kind == kind && string.Equals(other.Value, Value, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            var result = 17;
            unchecked {
                result = result * 31 + (int)kind;
                result = result * 31 + StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
                return result;
            }
        }

        /// <summary>
        ///     format the part as string
        /// </summary>
        /// <returns>formatted part</returns>
        public override string ToString() {
            switch (kind) {
                case ScopedNamePartKind.StartItem:
                    return Value;
                case ScopedNamePartKind.SubItem:
                    return $".{Value}";
                case ScopedNamePartKind.GenericPart:
                    return $"<{Value}>";
            }

            return string.Empty;
        }

        /// <summary>
        ///     part value
        /// </summary>
        public string Value { get; }

        /// <summary>
        ///     override equals operator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(ScopedNamePart left, ScopedNamePart right)
            => left.Equals(right);

        /// <summary>
        ///     operator unequals operator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(ScopedNamePart left, ScopedNamePart right)
            => !(left == right);

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
            => obj is ScopedNamePart && Equals((ScopedNamePart)obj);
    }

    /// <summary>
    ///     a scoped name
    /// </summary>
    public class ScopedName : IEquatable<ScopedName> {

        private readonly ScopedNamePart[] parts;
        private readonly string stringCache;


        /// <summary>
        ///     create a new simple scoped name
        /// </summary>
        /// <param name="simpleName">simple name</param>
        public ScopedName(string simpleName) {
            stringCache = simpleName;
            parts = new ScopedNamePart[] {
                new ScopedNamePart(simpleName, ScopedNamePartKind.StartItem)
            };
        }

        /// <summary>
        ///     create a new scoped name
        /// </summary>
        /// <param name="parts1"></param>
        public ScopedName(string[] parts1) {
            stringCache = string.Empty;
            parts = new ScopedNamePart[parts1.Length];
            for (var i = 0; i < parts1.Length; i++) {
                if (i > 0)
                    stringCache += ".";
                stringCache += parts1[i];
                parts[i] = new ScopedNamePart(parts1[i], i < 1 ? ScopedNamePartKind.StartItem : ScopedNamePartKind.SubItem);
            }
        }


        /// <summary>
        ///     create a new simple scoped name with prefix
        /// </summary>
        /// <param name="simpleName">simple name</param>
        /// <param name="prefix">name prefix</param>
        public ScopedName(string prefix, string simpleName) {
            stringCache = $"{prefix}.{simpleName}";
            parts = new ScopedNamePart[] {
                new ScopedNamePart(prefix, ScopedNamePartKind.StartItem),
                new ScopedNamePart(simpleName, ScopedNamePartKind.SubItem),
            };

        }


        /// <summary>
        ///     test for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ScopedName other) {
            var result = parts.Length == other.parts.Length;

            if (result)
                for (var i = 0; i < parts.Length; i++)
                    if (!parts[i].Equals(other.parts[i]))
                        return false;

            return result;
        }

        /// <summary>
        ///     test for equality
        /// </summary>
        /// <param name="obj">object to test</param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (!(obj is ScopedName otherName))
                return false;
            return Equals(otherName);
        }

        /// <summary>
        ///     get the hash code for this name
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            var result = 17;

            unchecked {
                for (var i = 0; i < parts.Length; i++)
                    result = result * 31 + parts[i].GetHashCode();
                return result;
            }
        }

        /// <summary>
        ///     name length
        /// </summary>
        public int Length
            => parts.Length;

        /// <summary>
        ///     get the first part of this name
        /// </summary>
        public string FirstPart
            => Length > 0 ? parts[0].Value : string.Empty;

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string this[int index]
            => parts[index].Value;

        /// <summary>
        ///     string cache
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => stringCache;

        /// <summary>
        ///     remove the first part of this name
        /// </summary>
        /// <returns></returns>
        public ScopedName RemoveFirstPart() {
            var newParts = new string[Math.Max(0, parts.Length - 1)];

            for (var i = 1; i < parts.Length; i++)
                newParts[i - 1] = parts[i].Value;

            return new ScopedName(newParts);
        }
    }

}
