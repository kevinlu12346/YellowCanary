namespace YellowCanaryLibrary.Extensions
{
    public static class EnumsExtension
    {
        public static OteTreatment ToOteTreatment(this string value)
        {
            switch(value)
            {
                case "OTE":
                    return OteTreatment.Ote;
                case "Not OTE":
                    return OteTreatment.Ote;
                default:
                    throw new ArgumentException("Invalid OTE code", value);
            }
        }

    }
}
