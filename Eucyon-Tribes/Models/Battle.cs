namespace Eucyon_Tribes.Models
{
    public enum Outcome { Victory, Defeat };
    
    public class Battle
    {
        public int Id { get; set; }

        public int AttackerId { get; set; }
        public Kingdom Attacker { get; set; } = null!;

        public int DefenderId { get; set; }
        public Kingdom Defender { get; set; } = null!;

        public Outcome Outcome { get; set; }    
        public DateTime Fought_at { get; set; }

        public Battle(int AttackerKingdomId, int DefenderKingdomId, Outcome outcome)
        {
            Fought_at = DateTime.Now;  
            this.Outcome = outcome;
            AttackerId = AttackerKingdomId;
            DefenderId = DefenderKingdomId;
        }

        public Battle()
        {
            Fought_at = DateTime.Now;
        }
    }
}
