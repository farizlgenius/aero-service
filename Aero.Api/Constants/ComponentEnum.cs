namespace AeroService.Constants
{
    public static class ComponentEnum
    {
        public enum AcrServiceMode
        {
            ReaderMode = 0,
            StrikeMode = 1,
            AcrMode = 2,
            ApbMode = 3,
            ReaderOut = 4,
            SpareFlag,AccessControlFlag
        }

        public enum AeroModel
        {
            AEROX1100 = 196,
            AEROX100 = 193, AEROX200 = 194,
            AEROX300 = 195
        }

    }
}
