﻿// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;

namespace AntdUI.Svg.Transforms
{
    public class SvgTransformCollection : List<SvgTransform>, ICloneable
    {
        private void AddItem(SvgTransform item)
        {
            base.Add(item);
        }

        public new void Add(SvgTransform item)
        {
            AddItem(item);
            OnTransformChanged();
        }

        public new void AddRange(IEnumerable<SvgTransform> collection)
        {
            base.AddRange(collection);
            OnTransformChanged();
        }

        public new void Remove(SvgTransform item)
        {
            base.Remove(item);
            OnTransformChanged();
        }

        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            OnTransformChanged();
        }

        /// <summary>
        /// Multiplies all matrices
        /// </summary>
        /// <returns>The result of all transforms</returns>
        public Matrix GetMatrix()
        {
            var transformMatrix = new Matrix();

            // Return if there are no transforms
            if (Count == 0) return transformMatrix;

            foreach (SvgTransform transformation in this)
            {
                transformMatrix.Multiply(transformation.Matrix(0, 0));
            }

            return transformMatrix;
        }

        public override bool Equals(object? obj)
        {
            if (Count == 0 && Count == base.Count) //default will be an empty list 
                return true;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public new SvgTransform this[int i]
        {
            get { return base[i]; }
            set
            {
                var oldVal = base[i];
                base[i] = value;
                if (oldVal != value)
                    OnTransformChanged();
            }
        }

        /// <summary>
        /// Fired when an SvgTransform has changed
        /// </summary>
        public event EventHandler<AttributeEventArgs> TransformChanged;

        protected void OnTransformChanged()
        {
            TransformChanged?.Invoke(this, new AttributeEventArgs { Attribute = "transform", Value = Clone() });
        }

        public object Clone()
        {
            var result = new SvgTransformCollection();
            foreach (var trans in this)
            {
                result.AddItem(trans.Clone() as SvgTransform);
            }
            return result;
        }

        public override string ToString()
        {
            if (Count < 1) return string.Empty;
            return (from t in this select t.ToString()).Aggregate((p, c) => p + " " + c);
        }
    }
}