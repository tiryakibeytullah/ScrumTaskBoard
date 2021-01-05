using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScrumTaskBoard.Models
{
    [Table("Is")]
    public class Is
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int isTakip_Id { get; set; }
        [Required()]
        public DateTime tarih { get; set; }
        [Required()]
        public int durum { get; set; }
        [Required(), MaxLength(200)]
        public string yapilacak_Is { get; set; }
        [Required(), MaxLength(1000)]
        public string aciklama { get; set; }
        [Required()]
        public int teknikKart_Id { get; set; }
    }
}