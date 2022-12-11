using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using AgonDiscordBot.Bot;
using AgonDiscordBot.DonationAlert.Structures;

namespace AgonDiscordBot.DonationAlert
{
    public class DonationAlert
    {
        private Bot.Bot CurrentBot;
        private int testo = 0;
        private DonationData donationData;

        private async Task SaveProgress(bool IsRead = false)
        {

            if (!File.Exists(@"progress.txt")) { 
                Console.WriteLine("Progress file not found");
                return;
            }
            if (IsRead) { 
                 // Read the file and display it line by line.  
                foreach (string line in System.IO.File.ReadLines(@"progress.txt"))
                {
                    var data = line.Split("/");
                     donationData.currentprogress = Convert.ToInt32(data[0]);
                     donationData.DonateGoal = Convert.ToInt32(data[1]);
                     donationData.currentCurrency = data[2];
                }
            }
            else
            {
                await File.WriteAllTextAsync("progress.txt", $"{donationData.currentprogress}/{donationData.DonateGoal}/{donationData.currentCurrency}");
            }
        }
        public DonationAlert(ref Bot.Bot bot){
            CurrentBot = bot;
         }
        private async Task<ConfigJson> ConfigurationFile()
        {
            var json = string.Empty;
            using (var fs = File.OpenRead("ConfigDA.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<ConfigJson>(json);
        }
        public async Task  DonationAlertStart() 
        {
            await SaveProgress(true);
            ConfigJson Configuration =  await ConfigurationFile();
            var socket = new SocketIOClient.SocketIO("wss://socket.donationalerts.ru:443", new SocketIOClient.SocketIOOptions
            {  EIO = 3 }) ;

            if (socket != null)
            {
                ConfigJson mr = new ConfigJson()
                { token = Configuration.token, type = Configuration.type };
                socket.OnConnected += async (sender, e) =>
                {
                    // Emit Token
                    await socket.EmitAsync("add-user", mr);
                    Console.WriteLine($"DonationAlerts Api Connected");
                };
                
                socket.On("update-alert_widget", async (data) =>
                {
                    Console.WriteLine("Type:   {0}\r\n", data);
                });
                socket.OnError += async (sender, e) =>
                {
                    Console.WriteLine("Error CONNECTION\n" + e);
                };
                socket.OnDisconnected += async (sender,e) =>
                {
                    Console.WriteLine("DonationAlerts Api Client Has Been Disconnected \n " + e);
                };
                socket.On("donation", async (donate) =>
                {

                    //MyResponse ReceiveData = JsonConvert.DeserializeObject<MyResponse>(jsonResult);
                    JArray jsonArray = JArray.Parse(donate.ToString()); //                              get Json Object in Array format
                    Console.WriteLine(jsonArray);
                    var data = JObject.Parse(jsonArray[0].ToString());//                                convert the array into json object 
                    dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(data.ToString());//    Convert The json object to C# Properites
                                                                                          // Donator Person = new Donator(json.username, json.amount, json.currency);//                                                assign the converted properites to Donator Object
                    string donator_name = json.username;
                    int Donatedamount = json.amount;
                    string amount = $"{Donatedamount}  {json.currency}";
                    string date_created = json.date_created;
                    donationData.currentprogress = donationData.currentprogress + Donatedamount;
                    await SaveProgress(false);
                    await CurrentBot.DonateNotification(donator_name,amount,date_created,donationData).ConfigureAwait(false);
                });
                socket.On("update-user_general_widget_settings", async (info) =>
                {
                    Console.WriteLine("New donation 1");
                    Console.WriteLine(info.ToString());
                });
                socket.On("alert-show", async  (info) =>
                {
                    Console.WriteLine("New donation 1");
                    Console.WriteLine(info.ToString());
                });
                socket.On("goal", (info) =>
                {
                    Console.WriteLine("New donation 1");
                    Console.WriteLine(info.ToString());
                });
                await socket.ConnectAsync();
            }
            else
            {
                Console.WriteLine("Socket Equal to Null");
            }


        }
    }
}
