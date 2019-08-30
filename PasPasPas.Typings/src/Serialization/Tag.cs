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
        /// <param name="length"></param>
        /// <param name="typeReader"></param>
        internal abstract void ReadData(uint kind, uint length, TypeReader typeReader);

        /// <summary>
        ///     tag length
        /// </summary>
        public abstract uint Length { get; }

        /// <summary>
        ///     write tag data
        /// </summary>
        /// <param name="typeWriter"></param>
        public abstract void WriteData(TypeWriter typeWriter);
    }
}