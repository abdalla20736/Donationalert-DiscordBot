using Newtonsoft.Json;


namespace AgonDiscordBot.DonationAlert
{

    public class additional_data
    {

        [JsonProperty("force_variation")]
        public int force_variation { get; set; }
        [JsonProperty("randomness")]
        public int randomness { get; set; }
    }
   
    public class Donator
    {
        //[JsonProperty("id")]
        public int id { get; set; }
        //[JsonProperty("alert_type")]
        //public int alert_type { get; set; }
        //[JsonProperty("is_shown")]
        //public string is_shown { get; set; }
        //[JsonProperty("additional_data")]
        //public additional_data additional_data { get; set; }
        //[JsonProperty("billing_system")]
        //public string billing_system { get; set; }
        //[JsonProperty("billing_system_type")]
        //public object billing_system_type { get; set; }
        //[JsonProperty("username")]
        public string username { get; set; }

        //[JsonProperty("amount")]
        public int amount { get; set; }
        //[JsonProperty("amount_formatted")]
        //public string amount_formatted { get; set; }
        //[JsonProperty("amount_main")]
        //public int amount_main { get; set; }
        //[JsonProperty("currency")]
        public string currency { get; set; }

        //[JsonProperty("message")]
        public string message { get; set; }
        //[JsonProperty("header")]
        //public string header { get; set; }

        //[JsonProperty("date_created")]
        public string date_created { get; set; }
        //[JsonProperty("emotes")]
        //public int emotes { get; set; }
        //[JsonProperty("ap_id")]
        //public int ap_id { get; set; }
        //[JsonProperty("_is_test_alert")]
        //public bool _is_test_alert { get; set; }
        //[JsonProperty("message_type")]
        //public string message_type { get; set; }

        public Donator(string username, int amount, string currency/*string message,string date_created*/)
        {
            //this.id = id;
            this.username = username;
            this.amount = amount;
            this.currency = currency;
         //   this.message = message;
         //   this.date_created = date_created;

        }
        public static Donator Deserialize(string jsonString)
        {
            return JsonConvert.DeserializeObject<Donator>(jsonString);
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ConfigJson
    {
        [JsonProperty("token")]
        public string token { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static ConfigJson Deserialize(string jsonString)
        {
            return JsonConvert.DeserializeObject<ConfigJson>(jsonString);
        }
    }
}
