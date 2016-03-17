namespace EnumsNET
{
    internal class OptionalOutParameter<T>
        where T : struct
    {
        public T Result;

        public OptionalOutParameter()
        {
        }

        public OptionalOutParameter(T result)
        {
            Result = result;
        }

        public static implicit operator T(OptionalOutParameter<T> o) => o?.Result ?? default(T);

        public static implicit operator OptionalOutParameter<T>(T o) => new OptionalOutParameter<T>(o);
    }
}
