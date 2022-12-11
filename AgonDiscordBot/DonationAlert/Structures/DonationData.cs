using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgonDiscordBot.DonationAlert.Structures
{
    public struct DonationData
    {
        public int currentprogress { get; set; }
        public int DonateGoal { get; set; }
        public string currentCurrency { get; set; }
    }
}
