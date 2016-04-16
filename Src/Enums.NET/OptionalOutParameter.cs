namespace EnumsNET
{
    internal class OptionalOutParameter<T>
        where T : struct
    {
        public T Value;

        public OptionalOutParameter()
        {
        }

        public OptionalOutParameter(T value)
        {
            Value = value;
        }

        public static implicit operator T(OptionalOutParameter<T> o) => o?.Value ?? default(T);

        public static implicit operator OptionalOutParameter<T>(T o) => new OptionalOutParameter<T>(o);
    }
}
