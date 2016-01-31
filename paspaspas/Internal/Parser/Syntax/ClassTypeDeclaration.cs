﻿using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {


    /// <summary>
    ///     class type declarataion
    /// </summary>
    public class ClassTypeDeclaration : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ClassTypeDeclaration(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     class declaration
        /// </summary>
        public ClassDeclaration ClassDef { get; internal set; }

        /// <summary>
        ///     class helper
        /// </summary>
        public ClassHelperDef ClassHelper { get; internal set; }

        /// <summary>
        ///     class of declaration
        /// </summary>
        public ClassOfDeclaration ClassOf { get; internal set; }

        /// <summary>
        ///     interface definition
        /// </summary>
        public InterfaceDefinition InterfaceDef { get; internal set; }

        /// <summary>
        ///     object declaration
        /// </summary>
        public ObjectDeclaration ObjectDecl { get; internal set; }

        /// <summary>
        ///     record declaration
        /// </summary>
        public RecordDeclaration RecordDecl { get; internal set; }

        /// <summary>
        ///     record helper
        /// </summary>
        public RecordHelperDef RecordHelper { get; internal set; }

        /// <summary>
        ///     format class type declaration
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Part(ClassOf);
            result.Part(ClassDef);
            result.Part(ClassHelper);
            result.Part(InterfaceDef);
            result.Part(ObjectDecl);
            result.Part(RecordHelper);
            result.Part(RecordDecl);
        }
    }
}