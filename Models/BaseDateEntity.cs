using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Models
{
    public class BaseDateEntity
    {      
        [DataType(DataType.DateTime)]
        [Description("Datum a čas vytvoření záznamu.")]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        [Description("Datum a čas poslední změny záznamu.")]
        public DateTime UpdatedAt { get; set; }

    }
}
