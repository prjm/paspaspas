using System;
using System.Collections.Generic;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Runtime;
using PasPasPas.Parsing.SyntaxTree.Standard;

namespace PasPasPas.Parsing.Parser {

    /// <summary>
    ///     helper class to find required units
    /// </summary>
    internal class RequiredUnitsFinder {

        private List<FileReference> requiredUnits
            = new List<FileReference>();
        private readonly IPathResolver fileResolver;
        readonly FileReference basePath;

        /// <summary>
        ///
        /// </summary>
        public IList<FileReference> RequiredUnits
            => requiredUnits;

        /// <summary>
        ///     create a new required units finder
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="currentPath"></param>
        public RequiredUnitsFinder(FileReference currentPath, IPathResolver resolver) {
            fileResolver = resolver;
            basePath = currentPath;
        }

        public void FindRequiredUnits(ISyntaxPart part) {
            switch (part) {
                case ProgramSymbol program:
                    FindRequiredUnitsOfProgram(program);
                    break;

                case LibrarySymbol library:
                    FindRequiredUnitsOfLibrary(library);
                    break;

                case PackageSymbol package:
                    FindRequiredUnitsOfPackage(package);
                    break;

                case UnitSymbol unit:
                    FindRequiredUnitsOfUnit(unit);
                    break;
            }
        }

        private void FindRequiredUnitsOfUnit(UnitSymbol unit) {
            var usesList = unit?.UnitInterface?.UsesClause?.UsesList;
                if (usesList == default)
                    return;

                FindRequiredUnitsOfUsesClause(usesList);
            }

   
        private void FindRequiredUnitsOfPackage(PackageSymbol package) {
            var usesList = package?.ContainsClause?.ContainsList;
            if (usesList == default)
                return;

            FindRequiredUnitsOfUsesClause(usesList);
        }

        private void FindRequiredUnitsOfLibrary(LibrarySymbol library) {
            var usesList = library?.Uses?.Files;
            if (usesList == default)
                return;

            FindRequiredUnitsOfUsesClause(usesList);
        }

        private void FindRequiredUnitsOfProgram(ProgramSymbol program) {
            var usesList = program?.Uses?.Files;
            if (usesList == default)
                return;

            FindRequiredUnitsOfUsesClause(usesList);
        }

        private void FindRequiredUnitsOfUsesClause(NamespaceFileNameListSymbol usesList) {
            foreach (var usesClause in usesList.Items) {
                var path = //
                    (usesClause.QuotedFileName?.Symbol?.Token.ParsedValue as IStringValue)?.AsUnicodeString ??
                    usesClause?.NamespaceName?.CompleteName + ".pas";

                if (string.IsNullOrWhiteSpace(path))
                    continue;

                var fileRef = new FileReference(path);
                var file = fileResolver.ResolvePath(basePath, fileRef);
                if (file.IsResolved)
                    requiredUnits.Add(file.TargetPath);
            }
        }

        private void FindRequiredUnitsOfUsesClause(NamespaceNameListSymbol usesList) {
            foreach (var usesClause in usesList.Items) {
                var path = usesClause?.CompleteName + ".pas";

                if (string.IsNullOrWhiteSpace(path))
                    continue;

                var fileRef = new FileReference(path);
                var file = fileResolver.ResolvePath(basePath, fileRef);
                if (file.IsResolved)
                    requiredUnits.Add(file.TargetPath);
            }
        }
    }
}
