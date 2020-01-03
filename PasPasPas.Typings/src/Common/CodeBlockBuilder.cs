using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Globals.CodeGen;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     code block builder
    /// </summary>
    public class CodeBlockBuilder : IDisposable {

        /// <summary>
        ///     create a new code block builder
        /// </summary>
        /// <param name="pools"></param>
        public CodeBlockBuilder(IListPools pools) {
            ListPools = pools;
            OpCodes = pools.GetList<OpCode>();
        }

        /// <summary>
        ///     list pools
        /// </summary>
        public IListPools ListPools { get; }

        /// <summary>
        ///     generated op codes
        /// </summary>
        public IPoolItem<List<OpCode>> OpCodes { get; private set; }

        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        ///     dispose this object
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    OpCodes.Dispose();
                    OpCodes = default;
                }
                disposedValue = true;
            }
        }

        /// <summary>
        ///     dispose this code block builders
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     create a fixed code array
        /// </summary>
        /// <returns></returns>
        public ImmutableArray<OpCode> CreateCodeArray()
            => ListPools.GetFixedArray(OpCodes);

        /// <summary>
        ///     add a call
        /// </summary>
        /// <param name="callInfo"></param>
        public void AddCall(IInvocationResult callInfo)
            => OpCodes.Add(new OpCode(OpCodeId.Call));
    }
}
