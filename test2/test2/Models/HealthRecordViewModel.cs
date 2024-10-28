namespace test2.Models
{
    public class HealthRecordViewModel
    {
        public int RecordId { get; set; }       // ID of the health record
        public int Pid { get; set; }            // Patient ID
        public string Did { get; set; }         // Doctor ID (optional if the doctor is already linked through authentication)
        public string Diagnosis { get; set; }   // Diagnosis information
        public string Description { get; set; } // Description of the health issue or record details
        public string Note { get; set; }        // Additional notes about the health record
        public DateTime DateExam { get; set; }  // Date of examination
    }

}
