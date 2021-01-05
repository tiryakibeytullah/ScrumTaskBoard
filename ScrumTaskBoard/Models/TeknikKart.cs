using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScrumTaskBoard.Models
{
    [Table("TeknikKart")]
    public class TeknikKart
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int teknikKart_Id { get; set; }
        [Required(), MaxLength(100)]
        public string projeAdi { get; set; }
        [Required(), MaxLength(50)]
        public string teknikUzmanAdı { get; set; }
        [Required()]
        public DateTime tarih { get; set; }
        [Required()]
        public int tahminiSure { get; set; }
        [Required()]
        public int gerceklesenSure { get; set; }
        [Required(), MaxLength(1000)]
        public string isinAciklamasi { get; set; }
        [Required(), MaxLength(1000)]
        public string notlar { get; set; }
        public List<Is> Is { get; set; }

    }
}