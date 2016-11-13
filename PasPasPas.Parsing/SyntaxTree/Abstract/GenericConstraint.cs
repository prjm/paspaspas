using PasPasPas.Parsing.SyntaxTree.Standard;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     generic constraint
    /// </summary>
    public class GenericConstraint : SymbolTableEntryBase {

        /// <summary>
        ///     constraint kind
        /// </summary>
        public GenericConstraintKind Kind { get; set; }

        /// <summary>
        ///     symbol name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     symbol name
        /// </summary>
        protected override string InternalSymbolName
        {
            get
            {
                switch (Kind) {
                    case GenericConstraintKind.Class:
                        return "class";
                    case GenericConstraintKind.Record:
                        return "record";
                    case GenericConstraintKind.Constructor:
                        return "constructor";
                }

                return Name?.CompleteName;
            }
        }
        /// <summary>
        ///     map a constraint kind
        /// </summary>
        /// <returns></returns>
        public static GenericConstraintKind MapKind(ConstrainedGeneric constraint) {

            if (constraint.RecordConstraint)
                return GenericConstraintKind.Record;
            else if (constraint.ClassConstraint)
                return GenericConstraintKind.Class;
            else if (constraint.ConstructorConstraint)
                return GenericConstraintKind.Constructor;
            else if (constraint.ConstraintIdentifier != null)
                return GenericConstraintKind.Identifier;
            else
                return GenericConstraintKind.Unknown;
        }


    }
}