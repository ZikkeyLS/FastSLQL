
namespace FastSLQL
{
    public static class SLDBSettings
    {
        public static bool Logging { get; private set; } = true;
        public static void SetLogging(bool result) => Logging = result;

        public const int LongSetupLenght = 3;
        public const int ShortSetupLenght = 2;
    }
}
