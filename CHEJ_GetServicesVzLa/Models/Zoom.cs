namespace CHEJ_GetServicesVzLa.Models
{
	using System.Collections.Generic;
	using Newtonsoft.Json;

	public class Zoom
    {
		[JsonProperty(PropertyName = "error")]
		public bool Error { get; set; }

		[JsonProperty(PropertyName = "descripcion")]
        public string Descripcion { get; set; }

		[JsonProperty(PropertyName = "referencia")]
        public string Referencia { get; set; }

		[JsonProperty(PropertyName = "estatus")]
		public string Estatus { get; set; }

		[JsonProperty(PropertyName = "tipoenvio")]
        public string TipoEnvio { get; set; }

		[JsonProperty(PropertyName = "fecha")]
        public string Fecha { get; set; }

		[JsonProperty(PropertyName = "origen")]
        public string Origen { get; set; }

		[JsonProperty(PropertyName = "destino")]
        public string Destino { get; set; }

		[JsonProperty(PropertyName = "seguimiento")]
		public List<Seguimiento> Seguimiento { get; set; }
    }
}