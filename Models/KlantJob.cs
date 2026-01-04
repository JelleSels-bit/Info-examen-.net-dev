using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Interimkantoor.Models
{
    public class KlantJob
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Selecteer een klant")]
        public string KlantId { get; set; }
        public int JobId { get; set; }

        public Klant? Klant { get; set; }

        [JsonIgnore]
        public Job? Job { get; set; }
    }
}
