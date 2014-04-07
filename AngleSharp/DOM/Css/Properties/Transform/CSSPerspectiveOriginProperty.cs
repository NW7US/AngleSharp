﻿namespace AngleSharp.DOM.Css.Properties
{
    using System;

    /// <summary>
    /// More information available at:
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/perspective-origin
    /// </summary>
    sealed class CSSPerspectiveOriginProperty : CSSProperty
    {
        #region Fields

        CSSCalcValue _x;
        CSSCalcValue _y;

        #endregion

        #region ctor

        public CSSPerspectiveOriginProperty()
            : base(PropertyNames.PerspectiveOrigin)
        {
            _inherited = false;
            _x = CSSCalcValue.Center;
            _y = CSSCalcValue.Center;
        }

        #endregion

        #region Methods

        protected override Boolean IsValid(CSSValue value)
        {
            if (value == CSSValue.Inherit)
                return true;
            else if (value is CSSValueList)
                return SetXY((CSSValueList)value);
            
            return SetSingle(value);
        }

        Boolean SetSingle(CSSValue value)
        {
            var calc = value.ToCalc();

            if (calc != null)
            {
                _x = calc;
                _y = calc;
                return true;
            }
            else if (value is CSSIdentifierValue)
            {
                var ident = ((CSSIdentifierValue)value).Value;

                switch (ident.ToLower())
                {
                    case "left":
                        _x = CSSCalcValue.Zero;
                        _y = CSSCalcValue.Center;
                        return true;

                    case "center":
                        _x = CSSCalcValue.Center;
                        _y = CSSCalcValue.Center;
                        return true;

                    case "right":
                        _x = CSSCalcValue.Full;
                        _y = CSSCalcValue.Center;
                        return true;

                    case "top":
                        _x = CSSCalcValue.Center;
                        _y = CSSCalcValue.Zero;
                        return true;

                    case "bottom":
                        _x = CSSCalcValue.Center;
                        _y = CSSCalcValue.Full;
                        return true;
                }
            }

            return false;
        }

        Boolean SetXY(CSSValueList list)
        {
            if (list.Length == 2)
            {
                var x = GetMode(list[0], "left", "right");
                var index = 1;

                if (x == null)
                    index--;

                var y = GetMode(list[index], "top", "bottom");

                if (y != null && x == null)
                    x = GetMode(list[1], "left", "right");

                if (x != null && y != null)
                {
                    _x = x;
                    _y = y;
                    return true;
                }
            }

            return false;
        }

        static CSSCalcValue GetMode(CSSValue value, String minIdentifier, String maxIdentifier)
        {
            var calc = value.ToCalc();

            if (calc == null && value is CSSIdentifierValue)
            {
                var ident = ((CSSIdentifierValue)value).Value;

                if (minIdentifier.Equals(ident, StringComparison.OrdinalIgnoreCase))
                    calc = CSSCalcValue.Zero;
                else if (maxIdentifier.Equals(ident, StringComparison.OrdinalIgnoreCase))
                    calc = CSSCalcValue.Full;
                else if ("center".Equals(ident, StringComparison.OrdinalIgnoreCase))
                    calc = CSSCalcValue.Center;
            }

            return calc;
        }

        #endregion

    }
}
