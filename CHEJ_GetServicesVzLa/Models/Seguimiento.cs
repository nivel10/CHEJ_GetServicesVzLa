namespace CHEJ_GetServicesVzLa.Models
{
	using Newtonsoft.Json;

    public class Seguimiento
    {
		[JsonProperty(PropertyName = "oficina")]
		public string Oficina { get; set; }

		[JsonProperty(PropertyName = "motivo")]
        public string Motivo { get; set; }

		[JsonProperty(PropertyName = "estatus")]
        public string Estatus { get; set; }

		[JsonProperty(PropertyName = "fecha")]
        public string Fecha { get; set; }      
    }
}