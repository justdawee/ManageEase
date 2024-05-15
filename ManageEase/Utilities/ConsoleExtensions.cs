namespace ManageEase.Utilities
{
    public static class ConsoleExtensions
    {
        // Method to read a password from the console input without displaying the characters
        public static string ReadPassword()
        {
            string password = string.Empty;
            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey(intercept: true);
                if (keyInfo.Key != ConsoleKey.Enter && keyInfo.Key != ConsoleKey.Backspace)
                {
                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[0..^1];
                    Console.Write("\b \b");
                }
            } while (keyInfo.Key != ConsoleKey.Enter);

            return password;
        }

        // Asynchronous method to display a countdown timer in the console
        public static async Task ReturnTimerAsync(int seconds, string action)
        {
            int i = seconds;
            while (i > 0)
            {
                Console.WriteLine($"Az alkalmazás {i} másodperc múlva {action}.");
                await Task.Delay(1000);
                i--;
            }
        }
    }
}