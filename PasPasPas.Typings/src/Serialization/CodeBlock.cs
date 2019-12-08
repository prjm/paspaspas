using System;
using System.Collections.Generic;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Serialization {

    internal static class ProcedureKindHelper {

        public static byte ToByte(this ProcedureKind kind)
            => (byte)kind;

        public static ProcedureKind ToProcedureKind(this byte kind) {
            var result = (ProcedureKind)kind;
            if (!Enum.IsDefined(typeof(ProcedureKind), result))
                throw new TypeReaderWriteException();
            return result;
        }
    }

    /// <summary>
    ///     code block
    /// </summary>
    internal class CodeBlock : Tag {

        public CodeBlock(Reference referenceToRoutines, StringRegistry strings) {
            ReferenceToRoutines = referenceToRoutines;
            stringData = strings;
        }

        /// <summary>
        ///     reference
        /// </summary>
        public Reference ReferenceToRoutines { get; }

        /// <summary>
        ///     tag
        /// </summary>
        public override uint Kind
            => Constants.CodeBlockTag;

        private readonly List<IRoutine> routines
            = new List<IRoutine>();

        private readonly StringRegistry stringData;

        internal override void ReadData(uint kind, TypeReader typeReader) {
            var n = typeReader.ReadUint();
            routines.Capacity = (int)n;

            for (var i = 0; i < n; i++) {
                var nameIndex = typeReader.ReadUint();
                var name = stringData[nameIndex];
                var routine = typeReader.Types.TypeCreator.CreateGlobalRoutine(name);
                routines.Add(routine);
                var paramCount = typeReader.ReadUint();
                var paramTag = new ParameterGroupTag();
                for (var j = 0; j < paramCount; j++) {
                    typeReader.ReadTag(paramTag);
                    paramTag.AddToRoutine(routine);
                }
            }
        }

        internal override void WriteData(TypeWriter typeWriter) {
            var n = (uint)routines.Count;
            typeWriter.WriteUint(n);

            for (var i = 0; i < n; i++) {
                var routine = routines[i];
                n = stringData[routine.Name];
                typeWriter.WriteUint(n);
                n = (uint)routine.Parameters.Count;
                typeWriter.WriteUint(n);
                var paramTag = new ParameterGroupTag();
                for (var j = 0; j < routine.Parameters.Count; j++) {
                    paramTag.Initialize(routine.Parameters[j]);
                    typeWriter.WriteTag(paramTag);
                }
            }
        }

        internal void PrepareRoutines(IUnitType unit) {
            foreach (var symbol in unit.Symbols) {
                var reference = symbol.Value as Globals.Types.Reference;
                if (reference.Kind == ReferenceKind.RefToGlobalRoutine) {
                    var routine = reference.Symbol as IRoutine;
                    routines.Add(routine);
                    var _ = stringData[routine.Name];
                }
            }
        }

        internal void AddToUnit(IUnitType unit) {
            foreach (var routine in routines) {
                var reference = new Globals.Types.Reference(ReferenceKind.RefToGlobalRoutine, routine);
                unit.Symbols.Add(routine.Name, reference);
            }
        }
    }
}