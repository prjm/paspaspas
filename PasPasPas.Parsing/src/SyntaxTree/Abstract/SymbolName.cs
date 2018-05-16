using System.Linq;
using System.Collections.Generic;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     abstract symbol name
    /// </summary>
    public abstract class SymbolName {

        /// <summary>
        ///     complete name
        /// </summary>
        public abstract string CompleteName { get; }

        /// <summary>
        ///     simple name 
        /// </summary>
        public virtual string Name
            => CompleteName;

        /// <summary>
        ///     namespace
        /// </summary>
        public virtual string NamespaceName
            => string.Empty;

        /// <summary>
        ///     format name as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => CompleteName;

    }

    /// <summary>
    ///     simple symbol name
    /// </summary>
    public class SimpleSymbolName : SymbolName {

        private readonly string name;

        /// <summary>
        ///     create a simple symbol name
        /// </summary>
        /// <param name="name">symbol name</param>
        public SimpleSymbolName(string name)
            => this.name = name;

        /// <summary>
        ///     complete name
        /// </summary>
        public override string CompleteName
            => name;
    }

    /// <summary>
    ///     symbol name with namespace
    /// </summary>
    public class NamespacedSymbolName : SymbolName {

        private IList<string> names
            = new List<string>();

        /// <summary>
        ///     get the complete name
        /// </summary>
        public override string CompleteName
            => string.Join(".", names);

        /// <summary>
        ///     namespace
        /// </summary>
        public override string NamespaceName
            => string.Join(".", names.DropLast());

        /// <summary>
        ///     name
        /// </summary>
        public override string Name
            => names.LastOrDefault();

        /// <summary>
        ///     append a name part
        /// </summary>
        /// <param name="name"></param>
        public void Append(string name) => names.Add(name);
    }

    /// <summary>
    ///     symbol file name
    /// </summary>
    public class NamespacedSymbolFileName : NamespacedSymbolName {

    }

    /// <summary>
    ///     symbol name part with generic definition
    /// </summary>
    public class GenericSymbolNamePart : SymbolName {

        private string name;

        private IList<string> parameters;

        /// <summary>
        ///     generate a new name part
        /// </summary>
        /// <param name="namePart"></param>
        public GenericSymbolNamePart(string namePart) {
            name = namePart;
        }

        /// <summary>
        ///     name part
        /// </summary>
        public override string Name
            => name;

        /// <summary>
        ///     parameters
        /// </summary>
        public IList<string> Parameters
            => parameters;

        /// <summary>
        ///     complete name
        /// </summary>
        public override string CompleteName {
            get {
                var result = name;

                if (parameters != null && parameters.Count > 0) {
                    result = string.Concat(result, "<", string.Join(",", parameters), ">");
                }

                return result;
            }
        }

        /// <summary>
        ///     add an generic parameter
        /// </summary>
        /// <param name="genericPart"></param>
        public void AddParameter(string genericPart) {
            if (parameters == null)
                parameters = new List<string>();

            parameters.Add(genericPart);
        }
    }

    /// <summary>
    ///     generic symbol name
    /// </summary>
    public class GenericSymbolName : SymbolName {

        /// <summary>
        ///     parts
        /// </summary>
        private IList<GenericSymbolNamePart> parts
            = new List<GenericSymbolNamePart>();

        /// <summary>
        ///     namespace
        /// </summary>
        public override string NamespaceName
            => string.Join(".", parts.Select(t => t.Name).DropLast());

        /// <summary>
        ///     complete name
        /// </summary>
        public override string CompleteName
            => string.Join(".", NamespaceParts);

        /// <summary>
        ///     symbol name
        /// </summary>
        public override string Name
            => parts.LastOrDefault()?.Name;

        /// <summary>
        ///     name part
        /// </summary>
        public GenericSymbolNamePart NamePart
            => parts.LastOrDefault();

        /// <summary>
        ///     namespace parts
        /// </summary>
        public IEnumerable<GenericSymbolNamePart> NamespaceParts
            => parts.DropLast();

        /// <summary>
        ///     add a name
        /// </summary>
        /// <param name="namePart"></param>
        public void AddName(string namePart)
            => parts.Add(new GenericSymbolNamePart(namePart));

        /// <summary>
        ///     add a generic part
        /// </summary>
        /// <param name="genericPart">part to add</param>
        public void AddGenericPart(string genericPart) {
            var lastPart = parts.Last();
            lastPart.AddParameter(genericPart);
        }
    }

}
