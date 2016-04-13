namespace EnumsNET
{
    internal struct OptionalOutParameter<T>
        where T : struct
    {
        public T Value;

        public OptionalOutParameter(T value)
        {
            Value = value;
        }

        public static implicit operator T(OptionalOutParameter<T> o) => o.Value;

        public static implicit operator OptionalOutParameter<T>(T o) => new OptionalOutParameter<T>(o);
    }
}
