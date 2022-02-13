
namespace FastSLQL
{
    public static class FSLQLSettings
    {
        public static bool Logging { get; private set; } = true;
        public static void SetLogging(bool result) => Logging = result;

        internal const int LongSetupLenght = 3;
        internal const int ShortSetupLenght = 2;
    }
}
