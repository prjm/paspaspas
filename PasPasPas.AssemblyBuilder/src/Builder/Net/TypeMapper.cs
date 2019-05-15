using System;
using PasPasPas.Globals.Types;

namespace PasPasPas.AssemblyBuilder.Builder.Net {

    /// <summary>
    ///     type mappings
    /// </summary>
    public class TypeMapper {

        /// <summary>
        ///     map  a type to the target
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal Type Map(int value) {
            if (value == KnownTypeIds.NoType)
                return typeof(void);

            throw new InvalidOperationException();
        }

        /// <summary>
        ///     map a type from the target
        /// </summary>
        /// <param name="returnType"></param>
        /// <returns></returns>
        internal int Map(Type returnType) {
            if (returnType == typeof(void))
                return KnownTypeIds.NoType;

            throw new InvalidOperationException();
        }
    }
}
