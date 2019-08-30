using System.Collections.Generic;

namespace PasPasPas.Typings.Serialization {
    internal abstract class TagList {

        private readonly List<Tag> tags = new List<Tag>();

        public abstract uint Kind { get; }
        public uint NumberOfLists { get; internal set; }

        public void AddTag(Tag tag)
            => tags.Add(tag);

    }
}
