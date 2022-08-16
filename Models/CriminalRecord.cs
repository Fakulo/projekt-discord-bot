using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Models
{
    public class CriminalRecord
    {
        public CriminalRecord()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Description("ID záznamu v databázi.")]
        public int CriminalRecordId { get; private set; }

        [Required]
        [StringLength(300, ErrorMessage = "Maximální délka Info je 300 znaků.")]
        [Description("Info o záznamu.")]
        public string Info { get; set; }

        [DefaultValue(" ")]
        [StringLength(1000, ErrorMessage = "Maximální délka ChatEvidence je 1000 znaků.")]
        [Description("Výpis zpráv.")]
        public string ChatEvidence { get; set; }

        /************************************************************/

        private Player _player;
        private ILazyLoader LazyLoader { get; set; }
        private CriminalRecord(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        [ForeignKey("Player")]
        public int PlayerId { get; set; }

        [IgnoreDataMember]
        public virtual Player Player
        {
            get => LazyLoader.Load(this, ref _player);
            set => _player = value;
        }

    }
}
