﻿namespace Eucyon_Tribes.Models.DTOs.BattleDTOs
{
    public class BattleResposeDto
    {  
        public int BattleId { get; }
        public int AttackerKingdomId { get; }
        public string Attacker { get; }
        public int DefenderKingdomId { get; }
        public string Defender { get; }
        public DateTime Fought_at { get; }
        public Outcome Outcome { get; }
        public string OutcomeType { get; }

        public BattleResposeDto(int battleId, int attackerKingdomId, string attacker, 
            int defenderKingdomId, string defender, DateTime fought_at, Outcome outcome)
        {
            BattleId = battleId;
            AttackerKingdomId = attackerKingdomId;
            Attacker = attacker;
            DefenderKingdomId = defenderKingdomId;
            Defender = defender;
            Fought_at = fought_at;
            Outcome = outcome;
            OutcomeType = outcome.ToString();
        }
    }
}