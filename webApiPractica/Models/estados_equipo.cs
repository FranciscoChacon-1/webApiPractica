using System.ComponentModel.DataAnnotations;

namespace webApiPractica.Models
{
    public class estados_equipo
    {
        [Key]
        public int id_estados_equipo {  get; set; }
        public string descipcion { get; set; }
        public string estado { get; set; }
    }
}
