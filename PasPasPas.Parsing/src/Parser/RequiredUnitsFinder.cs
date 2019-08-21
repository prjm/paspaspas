using System.Collections.Generic;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Runtime;
using PasPasPas.Parsing.SyntaxTree.Standard;

namespace PasPasPas.Parsing.Parser {

    /// <summary>
    ///     helper class to find required units
    /// </summary>
    internal class RequiredUnitsFinder {

        private List<IFileReference> requiredUnits
            = new List<IFileReference>();

        private readonly IPathResolver fileResolver;
        readonly IFileReference basePath;

        /// <summary>
        ///     required units
        /// </summary>
        public IList<IFileReference> RequiredUnits
            => requiredUnits;

        public ILogSource Log { get; }
        public bool HasMissingFiles { get; private set; }

        /// <summary>
        ///     create a new required units finder
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="log"></param>
        /// <param name="currentPath"></param>
        public RequiredUnitsFinder(IFileReference currentPath, IPathResolver resolver, ILogSource log) {
            fileResolver = resolver;
            Log = log;
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
                if (TryToResolveUnit(usesClause, out var file))
                    requiredUnits.Add(file);
                else {
                    Log.LogError(MessageNumbers.MissingFile);
                    HasMissingFiles = true;
                }
            }
        }

        private void FindRequiredUnitsOfUsesClause(NamespaceNameListSymbol usesList) {
            foreach (var usesClause in usesList.Items) {
                if (TryToResolveUnit(usesClause, out var file))
                    requiredUnits.Add(file);
                else {
                    Log.LogError(MessageNumbers.MissingFile);
                    HasMissingFiles = true;
                }
            }
        }

        public bool TryToResolveUnit(NamespaceFileNameSymbol usesClause, out IFileReference file) {
            var path = //
                (usesClause.QuotedFileName?.Symbol?.Token.ParsedValue as IStringValue)?.AsUnicodeString ??
                usesClause?.NamespaceName?.CompleteName + ".pas";

            if (string.IsNullOrWhiteSpace(path)) {
                file = default;
                return false;
            }

            var fileRef = basePath.CreateNewFileReference(path);
            var resolvedFile = fileResolver.ResolvePath(basePath, fileRef);
            if (resolvedFile.IsResolved) {
                file = resolvedFile.TargetPath;
                return true;
            }

            file = default;
            return false;
        }

        public bool TryToResolveUnit(NamespaceNameSymbol usesClause, out IFileReference file) {
            var path = usesClause?.CompleteName + ".pas";

            if (string.IsNullOrWhiteSpace(path)) {
                file = default;
                return false;
            }

            var fileRef = basePath.CreateNewFileReference(path);
            var resolvedFile = fileResolver.ResolvePath(basePath, fileRef);
            if (resolvedFile.IsResolved) {
                file = resolvedFile.TargetPath;
                return true;
            }


            Log.LogError(MessageNumbers.MissingFile);
            HasMissingFiles = true;
            file = default;
            return false;

        }

    }
}