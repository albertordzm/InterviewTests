namespace GraduationTracker
{
    public class ReportCard
    {
        public int StudentId { get; set; }
        public int DiplomaId { get; set; }
        public int Average { get; set; }
        public int Credits { get; set; }
        public STANDING Standing
        {
            get { return GraduationTracker.GetStanding(Average); }
        }
    }
}
