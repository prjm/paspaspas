using System;
using System.Collections;
using System.Collections.Generic;

namespace PasPasPas.Infrastructure.Utils {

    internal enum SmallListMode : byte {
        Unknown = 0,
        Empty = 1,
        SingleElement = 2,
        MoreElements = 3
    }

    public class SmallList<T> : IList<T> where T : class {

        private object data;
        private SmallListMode mode = SmallListMode.Empty;

        public T this[int index] {
            get {
                switch (mode) {
                    case SmallListMode.SingleElement:
                        return (T)data;
                    case SmallListMode.MoreElements:
                        return ((IList<T>)data)[index];
                }
                throw new IndexOutOfRangeException();
            }
            set {
                if (mode == SmallListMode.SingleElement && index == 0) {
                    data = value;
                }
                else if (mode == SmallListMode.MoreElements) {
                    ((IList<T>)data)[index] = value;
                }
                throw new IndexOutOfRangeException();
            }
        }

        public int Count {
            get {
                switch (mode) {

                    case SmallListMode.Empty:
                        return 0;

                    case SmallListMode.SingleElement:
                        return 1;

                    case SmallListMode.MoreElements:
                        return ((IList<T>)data).Count;

                    default:
                        return 0;
                }
            }
        }

        public bool IsReadOnly => false;

        public void Add(T item) {

            if (mode == SmallListMode.Empty) {
                data = item;
                mode = SmallListMode.SingleElement;
            }
            else if (mode == SmallListMode.SingleElement) {
                var standardList = new List<T>();
                standardList.Add((T)data);
                standardList.Add(item);
                data = standardList;
                mode = SmallListMode.MoreElements;
            }
            else if (mode == SmallListMode.MoreElements) {
                ((IList<T>)data).Add(item);
            }

        }

        public void Clear() {
            switch (mode) {

                case SmallListMode.SingleElement:
                    data = null;
                    break;

                case SmallListMode.MoreElements:
                    data = null;
                    break;

            }

            mode = SmallListMode.Empty;
        }

        public bool Contains(T item) {

            switch (mode) {

                case SmallListMode.SingleElement:
                    return EqualityComparer<T>.Default.Equals(item, (T)data);

                case SmallListMode.MoreElements:
                    return ((IList<T>)data).Contains(item);

                default:
                    return false;
            }


        }

        public void CopyTo(T[] array, int arrayIndex) {

            switch (mode) {

                case SmallListMode.SingleElement:
                    array[arrayIndex] = (T)data;
                    break;

                case SmallListMode.MoreElements:
                    ((IList<T>)data).CopyTo(array, arrayIndex);
                    break;

            }


        }

        public IEnumerator<T> GetEnumerator() {
            if (mode == SmallListMode.MoreElements)
                return ((IList<T>)data).GetEnumerator();
            else
                return GetInternalEnumerator();
        }

        public IEnumerator<T> GetInternalEnumerator() {

            switch (mode) {

                case SmallListMode.Empty:
                    yield break;

                case SmallListMode.SingleElement:
                    yield return (T)data;
                    break;

                default:
                    yield break;
            }


        }

        public int IndexOf(T item) {

            switch (mode) {

                case SmallListMode.SingleElement:
                    return EqualityComparer<T>.Default.Equals(item, (T)data) ? 0 : -1;

                case SmallListMode.MoreElements:
                    return ((List<T>)data).IndexOf(item);

            }

            return -1;
        }


        public void Insert(int index, T item) {

            if (mode == SmallListMode.Empty && index == 0) {
                Add(item);
                return;
            }
            else if (mode == SmallListMode.SingleElement && index < 2) {
                var standardList = new List<T>();
                standardList.Add((T)data);
                standardList.Add(item);
                data = standardList;
                mode = SmallListMode.MoreElements;
                return;
            }
            else if (mode == SmallListMode.MoreElements) {
                ((IList<T>)data).Insert(index, item);
                return;
            }


            throw new IndexOutOfRangeException();
        }


        public bool Remove(T item) {

            switch (mode) {

                case SmallListMode.Empty:
                    return false;

                case SmallListMode.SingleElement: {
                        if (EqualityComparer<T>.Default.Equals(item, (T)data)) {
                            data = null;
                            mode = SmallListMode.Empty;
                            return true;
                        }
                        return false;
                    }

                case SmallListMode.MoreElements: {
                        var standardList = ((IList<T>)data);
                        standardList.Remove(item);
                        if (standardList.Count < 2) {
                            data = standardList[0];
                            standardList = null;
                            mode = SmallListMode.SingleElement;
                            return true;
                        }
                        return false;
                    }

                default:
                    return false;
            }

        }

        public void RemoveAt(int index) {

            if (mode == SmallListMode.SingleElement && index == 0) {
                data = null;
                mode = SmallListMode.Empty;
                return;
            }
            else if (mode == SmallListMode.MoreElements) {
                var standardList = ((IList<T>)data);
                standardList.RemoveAt(index);
                if (standardList.Count < 2) {
                    data = standardList[0];
                    standardList = null;
                    mode = SmallListMode.SingleElement;
                }
                return;
            }

            throw new IndexOutOfRangeException();
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
