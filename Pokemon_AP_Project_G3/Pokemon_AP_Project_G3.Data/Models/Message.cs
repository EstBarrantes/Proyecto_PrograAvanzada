using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokemon_AP_Project_G3.Data.Models
{
    [Table("Messages")] 
    public class Message
    {
        [Key]
        public int message_id { get; set; } 

        [Required]
        public int sender_id { get; set; } 

        [Required]
        public int receiver_id { get; set; } 

        [Required]
        public string content { get; set; } 

        public DateTime sent_date { get; set; } = DateTime.Now; 
    }
}
