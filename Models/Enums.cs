using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Models
{
    public class Enums
    {
        public enum PointType
        {
            Pokestop,
            Gym,
            ExGym,
            Portal
        }

        public enum PokemonType
        {
            None,
            Normal,
            Fighting,
            Flying,
            Poison,
            Ground,
            Rock,
            Bug,
            Ghost,
            Steel,
            Fire,
            Water,
            Grass,
            Electric,
            Psychic,
            Ice,
            Dragon,
            Dark,
            Fairy
        }
        public enum PokemonForm
        {
            Normal,
            Alolan,
            Galarian,
            Hisuian
        }
        public enum Team
        {
            Valor,
            Mystic,
            Instinct,
            None
        }
        public enum Generation : int
        {
            Kanto = 1,
            Johto = 2,
            Hoenn = 3,
            Sinnoh = 4,
            Unova = 5,
            Kalos = 6,
            Alola = 7,
            Galar = 8,
            Paldea = 9
        }

        public enum WarningPhase : int
        {
            None = 0,
            První = 1,
            Druhá = 2,
            Třetí = 3,
            Finální = 4
        }

        //TODO: Používat i ChangePositionOrAdd
        /// <summary>
        /// Stav vstupního bodu
        /// </summary>
        public enum State
        {
            AddToDB,
            AddToDBCheck,
            ChangeName,
            ChangeNameCheck,
            ChangePosition,
            ChangePositionCheck,
            ChangePositionOrAdd,
            Duplicate,
            Unreachable
        }

        public enum NeedCheck
        {
            No,
            Yes,
            Checked
        }

        public enum PointsCategory
        {
            Other,
            RaidReport,
            RaidReportMeet,
            RaidParticipation,
            QuestReport,
            Transgression,
            Violation
        }
        
        public enum FindStatus
        {
            ChannelFound,
            MultipleChannelsFound,
            EmojiFound,
            MultipleEmojisFound,
            HatchTimeFound,
            EndTimeFound,
            PokemonFound,
            MultiplePokemonFound,
            PointFound,
            MultiplePointsFound,
            NotFound
        }
        public enum RaidJoinStatus
        {
            Join,
            Invite,
            Plus1,
            Plus2,
            Plus3,
            Plus4,
            Minus1,
            Minus2,
            Minus3,
            Minus4,
            Leave
        }

    }
}
