using System.Text.RegularExpressions;

namespace ManageEase.Utilities
{
    public static partial class Regexes
    {
        // Generated regular expression to validate email addresses
        [GeneratedRegex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")]    
        public static partial Regex EmailGeneratedRegex();
        
        // Generated regular expression to validate usernames
        [GeneratedRegex(@"\A[a-zA-Z0-9]+(?=([-_])?)(?:\1[a-zA-Z0-9]+)*\z")]     
        public static partial Regex UsernameGeneratedRegex();
        
        // Generated regular expression to validate strings containing only alphabetic characters (including accented Hungarian characters)
        [GeneratedRegex(@"^[a-zA-ZáéíóöőúüűÁÉÍÓÖŐÚÜŰ]+$")]
        public static partial Regex OnlyCharacterRegex();

        // Generated regular expression to validate strings containing only numeric characters
        [GeneratedRegex(@"^[0-9]+$")]
        public static partial Regex OnlyNumberRegex();
        
        // Regular expression to validate Hungarian addresses
        public static Regex AddressRegex = new Regex("^[A-ZÁÉÍÓÖŐÚÜŰa-záéíóöőúüű]+(?:[-\\s][A-ZÁÉÍÓÖŐÚÜŰa-záéíóöőúüű]+)*\\s+(utca|tér|köz|sétány|út|körút|sor|park)\\s+\\d+[a-zA-Z]?\\.$");
        
        // Generated regular expression to validate Hungarian phone numbers
        [GeneratedRegex("^(\\+36|06)[-\\s]?(1|20|30|31|50|70|90)[-\\s]?\\d{3}[-\\s]?\\d{4}$")]
        public static partial Regex PhoneNumberGeneratedRegex();
        
        // Generated regular expression to validate capitalized words (including Hungarian accented characters)
        [GeneratedRegex("^[A-ZÁÉÍÓÖŐÚÜŰ][a-záéíóöőúüű]+(?:[- ][A-ZÁÉÍÓÖŐÚÜŰ][a-záéíóöőúüű]+)*$")]
        public static partial Regex CapitalizedWordRegex();
    }
}