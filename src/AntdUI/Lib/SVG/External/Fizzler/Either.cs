// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

namespace Fizzler
{
    #region Imports

    using System;
    using System.Collections.Generic;

    #endregion

    // Adapted from Mono Rocks

    internal abstract class Either<TA, TB>
            : IEquatable<Either<TA, TB>>
    {
        private Either() { }

        public static Either<TA, TB> A(TA value)
        {
            if (value == null) throw new ArgumentNullException("value");
            return new AImpl(value);
        }

        public static Either<TA, TB> B(TB value)
        {
            if (value == null) throw new ArgumentNullException("value");
            return new BImpl(value);
        }

        public override abstract bool Equals(object obj);
        public abstract bool Equals(Either<TA, TB> obj);
        public override abstract int GetHashCode();
        public override abstract string ToString();
        public abstract TResult Fold<TResult>(Func<TA, TResult> a, Func<TB, TResult> b);

        private sealed class AImpl : Either<TA, TB>
        {
            private readonly TA _value;

            public AImpl(TA value)
            {
                _value = value;
            }

            public override int GetHashCode()
            {
                return _value.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as AImpl);
            }

            public override bool Equals(Either<TA, TB> obj)
            {
                var a = obj as AImpl;
                return a != null
                    && EqualityComparer<TA>.Default.Equals(_value, a._value);
            }

            public override TResult Fold<TResult>(Func<TA, TResult> a, Func<TB, TResult> b)
            {
                if (a == null) throw new ArgumentNullException("a");
                if (b == null) throw new ArgumentNullException("b");
                return a(_value);
            }

            public override string ToString()
            {
                return _value.ToString();
            }
        }

        private sealed class BImpl : Either<TA, TB>
        {
            private readonly TB _value;

            public BImpl(TB value)
            {
                _value = value;
            }

            public override int GetHashCode()
            {
                return _value.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as BImpl);
            }

            public override bool Equals(Either<TA, TB> obj)
            {
                var b = obj as BImpl;
                return b != null
                    && EqualityComparer<TB>.Default.Equals(_value, b._value);
            }

            public override TResult Fold<TResult>(Func<TA, TResult> a, Func<TB, TResult> b)
            {
                if (a == null) throw new ArgumentNullException("a");
                if (b == null) throw new ArgumentNullException("b");
                return b(_value);
            }

            public override string ToString()
            {
                return _value.ToString();
            }
        }
    }
}