using System;

namespace AgonDiscordBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var bot = new Bot.Bot();
            var DA = new DonationAlert.DonationAlert(ref bot);
            try
            {
                DA.DonationAlertStart().GetAwaiter().GetResult();
                bot.StartBot().GetAwaiter().GetResult();
                Console.WriteLine("Bot Started ");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Main Function Exception : {e}");
            }

        }
    }
}
