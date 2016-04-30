namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     rtti flag based on item visibility
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
        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }

            var other = (RttiForVisibility)obj;

            return
                other.ForPrivate == ForPrivate &&
                other.ForProtected == ForProtected &&
                other.ForPublic == ForPublic &&
                other.ForPublished == ForPublished;
        }

        /// <summary>
        ///     compute hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            int result = 17;
            result = result * 31 + ForPrivate.GetHashCode();
            result = result * 31 + ForProtected.GetHashCode();
            result = result * 31 + ForPublic.GetHashCode();
            result = result * 31 + ForPublished.GetHashCode();
            return result;
        }

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