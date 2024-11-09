// THIS FILE IS PART OF Vanara PROJECT
// THE Vanara PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) dahall. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/dahall/Vanara

using System;
using System.Runtime.InteropServices;
using static Vanara.PInvoke.Macros;

#pragma warning disable IDE1006 // Naming Styles

namespace Vanara.PInvoke
{
    /// <summary>Helper structure to use for a pointer that can morph into a string, pointer or integer.</summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    [PInvokeData("winuser.h")]
    public struct ResourceId : IEquatable<string>, IEquatable<IntPtr>, IEquatable<int>, IEquatable<ResourceId>, IHandle
    {
        private IntPtr ptr;

        /// <summary>Gets or sets an integer identifier.</summary>
        /// <value>The identifier.</value>
        public int id
        {
            get => IS_INTRESOURCE(ptr) ? (ushort)ptr.ToInt32() : 0;
            set
            {
                if (value > ushort.MaxValue || value <= 0) throw new ArgumentOutOfRangeException(nameof(id));

#if NET40 || NET46 || NET48 || NET6_0
                ptr = (IntPtr)(ushort)value;
#else
                ptr = (ushort)value;
#endif
            }
        }

        /// <summary>Determines whether this value is an integer identifier for a resource.</summary>
        /// <returns>If the value is a resource identifier, the return value is <see langword="true"/>. Otherwise, the return value is <see langword="false"/>.</returns>
        public bool IsIntResource => IS_INTRESOURCE(ptr);

        /// <summary>Represent a NULL value.</summary>
        public static readonly ResourceId NULL = new ResourceId();

        /// <summary>Performs an implicit conversion from <see cref="SafeResourceId"/> to <see cref="System.Int32"/>.</summary>
        /// <param name="r">The r.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator int(ResourceId r) => r.id;

        /// <summary>Performs an implicit conversion from <see cref="ResourceId"/> to <see cref="IntPtr"/>.</summary>
        /// <param name="r">The r.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator IntPtr(ResourceId r) => r.ptr;

        /// <summary>Performs an implicit conversion from <see cref="int"/> to <see cref="ResourceId"/>.</summary>
        /// <param name="resId">The resource identifier.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ResourceId(int resId) => new ResourceId { id = resId };

        /// <summary>Performs an implicit conversion from <see cref="IntPtr"/> to <see cref="ResourceId"/>.</summary>
        /// <param name="p">The PTR.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ResourceId(IntPtr p) => new ResourceId { ptr = p };

        /// <summary>Performs an implicit conversion from <see cref="ResourceId"/> to <see cref="string"/>.</summary>
        /// <param name="r">The r.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator string(ResourceId r) => r.ToString();

        /// <summary>Determines whether the specified <see cref="System.Object"/>, is equal to this instance.</summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case null:
                    return false;

                case string s:
                    return Equals(s);

                case int i:
                    return Equals(i);

                case IntPtr p:
                    return Equals(p);

                case ResourceId r:
                    return Equals(r);

                case IHandle h:
                    return Equals(h.DangerousGetHandle());

                default:
                    if (!obj.GetType().IsPrimitive) return false;
                    try { return Equals(Convert.ToInt32(obj)); } catch { return false; }
            }
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode() => ptr.GetHashCode();

        /// <summary>Returns a <see cref="string"/> that represents this instance.</summary>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public override string? ToString() => IS_INTRESOURCE(ptr) ? $"#{ptr.ToInt32()}" : Marshal.PtrToStringAuto(ptr);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        public bool Equals(int other) => ptr.ToInt32().Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Equals(string? other) => string.Equals(ToString(), other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        public bool Equals(IntPtr other) => ptr.Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Equals(ResourceId other) => string.Equals(other.ToString(), ToString());

        /// <inheritdoc/>
        public IntPtr DangerousGetHandle() => ptr;
    }
}