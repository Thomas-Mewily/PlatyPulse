using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatyDataBase.Entities
{
    public class Quest
    {
        public int QuestID { get; set; }
        public string QuestName { get; set; }
        public string QuestDescription { get; set; }
        public string Category { get; set; }
        public int Difficulty { get; set; }
        public int XP { get; set; }
    }

}
