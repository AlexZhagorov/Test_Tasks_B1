namespace Task_2.Models
{
    public class TotalAccountRecord
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public int AccountNumber { get; set; }
        public double ActiveIn { get; set; }
        public double PassiveIn { get; set; }
        public double Debet { get; set; }
        public double Credit { get; set; }
        public double ActiveOut { get; set; }
        public double PassiveOut { get; set; }
    }
}
