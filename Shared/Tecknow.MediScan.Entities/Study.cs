using System.Collections.Generic;

namespace Tecknow.MediScan.Entities
{
    public class Study : Person
    {
        public string StudyUid { get; set; }
        public string StudyName { get; set; }
        public string StudyDate { get; set; }
        public List<Series> Series { get; set; }
    }
}