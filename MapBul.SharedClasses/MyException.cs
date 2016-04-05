using System;
using MapBul.SharedClasses.Constants;

namespace MapBul.SharedClasses
{
    public class MyException : Exception
    {
        private readonly Error _error;

        public MyException(Error error)
        {
            _error = error;
        }

        public Error Error{get { return _error; }}


        protected bool Equals(MyException other)
        {
            return _error == other._error;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MyException) obj);
        }
        public override int GetHashCode()
        {
            return _error.Number;
        }
        public static bool operator == (MyException exception, int value)
        {
            if (exception == null)
                return false;
            return exception._error.Number == value;
        }
        public static bool operator !=(MyException exception, int value)
        {
            return !(exception == value);
        }
    }
}
