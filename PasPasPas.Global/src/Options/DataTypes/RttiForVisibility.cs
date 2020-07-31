using System;

namespace PasPasPas.Globals.Options.DataTypes {

    /// <summary>
    ///     <c>rtti</c> flag based on item visibility
    /// </summary>
    public class RttiForVisibility {

        /// <summary>
        ///     for private items
        /// </summary>
        public bool ForPrivate { get; set; }

        /// <summary>
        ///     for protected items
        /// </summary>
        public bool ForProtected { get; set; }

        /// <summary>
        ///     for public items
        /// </summary>
        public bool ForPublic { get; set; }

        /// <summary>
        ///     for published items
        /// </summary>
        public bool ForPublished { get; set; }

        /// <summary>
        ///     assign visibility
        /// </summary>
        /// <param name="visibility"></param>
        public void Assign(RttiForVisibility visibility) {
            ForPrivate = visibility.ForPrivate;
            ForProtected = visibility.ForProtected;
            ForPublic = visibility.ForPublic;
            ForPublished = visibility.ForPublished;
        }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj) {
            if (obj is null)
                return false;

            if (!(obj is RttiForVisibility other))
                return false;

            return
                other.ForPrivate == ForPrivate &&
                other.ForProtected == ForProtected &&
                other.ForPublic == ForPublic &&
                other.ForPublished == ForPublished;
        }

        /// <summary>
        ///     reset to default
        /// </summary>
        public void ResetToDefault() {
            ForPrivate = false;
            ForProtected = false;
            ForPublic = false;
            ForPublished = false;
        }

        /// <summary>
        ///     compute hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => HashCode.Combine(ForPrivate, ForProtected, ForPublic, ForPublished);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => "{" +
                    (ForPrivate ? "1" : "0") +
                    (ForProtected ? "1" : "0") +
                    (ForPublic ? "1" : "0") +
                    (ForPublished ? "1" : "0") +
                "}";

    }
}