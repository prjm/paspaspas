using PasPasPas.Api;
using System;
using System.Collections.Generic;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     composed syntax part
    /// </summary>
    /// <typeparam name="TDetail">detail type</typeparam>
    public abstract class ComposedPart<TDetail> : SyntaxPartBase
        where TDetail : ISyntaxPart {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        protected ComposedPart(IParserInformationProvider informationProvider) : base(informationProvider) { }

        private Lazy<List<TDetail>> details
            = new Lazy<List<TDetail>>(() => new List<TDetail>());

        /// <summary>
        ///     add a detail
        /// </summary>
        /// <param name="detail">detail to add</param>
        public void Add(TDetail detail) {
            if (detail != null)
                details.Value.Add(detail);
        }

        /// <summary>
        ///     detail count
        /// </summary>
        public int Count
        {
            get
            {
                if (!details.IsValueCreated)
                    return 0;

                return details.Value.Count;
            }
        }

        /// <summary>
        ///     get items
        /// </summary>
        public IEnumerable<TDetail> Items => details.Value;

        /// <summary>
        ///     flatten details
        /// </summary>
        /// <param name="delimiter">delimiting function</param>
        /// <param name="formatter">formatter</param>
        /// <returns></returns>
        protected void FlattenToPascal(PascalFormatter formatter, Action<PascalFormatter> delimiter) {
            FlattenToPascal(formatter, delimiter, null, null);
        }

        /// <summary>
        ///     flatten details
        /// </summary>
        /// <param name="delimiter">delimiting function</param>
        /// <param name="formatter">formatter</param>
        /// <param name="prefix">prefix</param>
        /// <param name="suffix">suffix</param>
        /// <returns></returns>
        protected void FlattenToPascal(PascalFormatter formatter, Action<PascalFormatter> delimiter, Action<PascalFormatter> prefix, Action<PascalFormatter> suffix) {
            int count = 0;

            if (!details.IsValueCreated)
                return;

            foreach (var detail in details.Value) {
                if (count > 0)
                    delimiter(formatter);

                if (prefix != null)
                    prefix(formatter);

                detail.ToFormatter(formatter);

                if (suffix != null)
                    suffix(formatter);

                count++;
            }
        }

    }
}
