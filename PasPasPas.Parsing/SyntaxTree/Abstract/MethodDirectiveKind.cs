﻿namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     kind of method directive
    /// </summary>
    public enum MethodDirectiveKind {

        /// <summary>
        ///     undefined kind
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     reintroduce directive
        /// </summary>
        Reintroduce = 1,

        /// <summary>
        ///     overload
        /// </summary>
        Overload = 2,

        /// <summary>
        ///     assembler
        /// </summary>
        Assembler = 3,

        /// <summary>
        ///     inline
        /// </summary>
        Inline = 4,

        /// <summary>
        ///     final
        /// </summary>
        Final = 5,

        /// <summary>
        ///     abstract
        /// </summary>
        Abstract = 6,

        /// <summary>
        ///     cdecl
        /// </summary>
        Cdecl = 7,

        /// <summary>
        ///     pascal
        /// </summary>
        Pascal = 8,

        /// <summary>
        ///     register
        /// </summary>
        Register = 9,

        /// <summary>
        ///     safecall
        /// </summary>
        Safecall = 10,

        /// <summary>
        ///     stdcall
        /// </summary>
        StdCall = 11,

        /// <summary>
        ///     export
        /// </summary>
        Export = 12,

        /// <summary>
        ///     static
        /// </summary>
        Static = 13,

        /// <summary>
        ///     dynamic
        /// </summary>
        Dynamic = 14,

        /// <summary>
        ///     virtual
        /// </summary>
        Virtual = 15,

        /// <summary>
        ///     override
        /// </summary>
        Override = 16,

        /// <summary>
        ///     message
        /// </summary>
        Message = 17,

        /// <summary>
        ///     dispid directive
        /// </summary>
        DispId = 18,
    }
}
