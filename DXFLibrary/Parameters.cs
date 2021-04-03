using System;

namespace DXFLibrary
{
    public class Parameter
    {
        #region Fields
        object _value;
        SourceType _source;

        public enum SourceType
        {
            None = 0,
            Command = 1,
            Registry = 2,
            App = 3
        }
        #endregion
        #region Constructor
        public Parameter(string value)
        {
            this._value = value;
            _source = SourceType.None;
        }
        public Parameter(string value, SourceType source)
        {
            this._value = value;
            this._source = source;
        }
        #endregion
        #region Parameters
        public object Value
        {
            set
            {
                this._value = value;
            }
            get
            {
                return (_value);
            }
        }

        public SourceType Source
        {
            set
            {
                _source = value;
            }
            get
            {
                return (_source);
            }
        }
        #endregion
        #region Methods
        public override string ToString()
        {
            return (Convert.ToString(_value));
        }
        #endregion
    }
}
