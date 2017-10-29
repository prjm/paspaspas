﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public struct ScopedNamePart : IEquatable<ScopedNamePart> {

        private readonly string value;
        private readonly ScopedNamePartKind kind;

        /// <summary>
        ///     create a new scoped part
        /// </summary>
        /// <param name="partValue">part value</param>
        /// <param name="partKind">kind</param>
        public ScopedNamePart(string partValue, ScopedNamePartKind partKind) {
            value = partValue;
            kind = partKind;
        }

        /// <summary>
        ///     check equality
        /// </summary>
        /// <param name="other">other object to check</param>
        /// <returns><c>true</c> if equal</returns>
        public bool Equals(ScopedNamePart other)
            => (other.kind == this.kind) && (string.Equals(other.value, this.value, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            var result = 17;
            result = result * 31 + ((int)kind);
            result = result * 31 + StringComparer.OrdinalIgnoreCase.GetHashCode(this.value);
            return result;
        }

        /// <summary>
        ///     format the part as string
        /// </summary>
        /// <returns>formatted part</returns>
        public override string ToString() {
            switch (kind) {
                case ScopedNamePartKind.StartItem:
                    return value;
                case ScopedNamePartKind.SubItem:
                    return $".{value}";
                case ScopedNamePartKind.GenericPart:
                    return $"<{value}>";
            }

            return string.Empty;
        }

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
        ///     test for equlity
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
            var otherName = obj as ScopedName;
            if (otherName == null)
                return false;
            return Equals(otherName);
        }

        /// <summary>
        ///     get the hashcode for this name
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            var result = 17;
            for (var i = 0; i < parts.Length; i++)
                result = result * 31 + parts[i].GetHashCode();
            return result;
        }

        /// <summary>
        ///     string cache
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => stringCache;

    }

}
