﻿#nullable disable
namespace PasPasPas.Typings.Serialization {

    internal abstract class Tag {

        /// <summary>
        ///     tag kind
        /// </summary>
        public abstract uint Kind { get; }

        /// <summary>
        ///     read data from a stream
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="typeReader"></param>
        internal abstract void ReadData(uint kind, TypeReader typeReader);

        /// <summary>
        ///     write tag data
        /// </summary>
        /// <param name="typeWriter"></param>
        internal abstract void WriteData(TypeWriter typeWriter);
    }
}