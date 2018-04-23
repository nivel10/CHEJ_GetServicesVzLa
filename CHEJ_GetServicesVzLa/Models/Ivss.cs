namespace CHEJ_GetServicesVzLa.Models
{
	using Newtonsoft.Json;

    public class Ivss
    {
		[JsonProperty(PropertyName = "error")]
		public bool Error { get; set; }

		[JsonProperty(PropertyName = "descripcion")]
        public string Descripcion { get; set; }

		[JsonProperty(PropertyName = "cedula")]
        public string Cedula { get; set; }

		[JsonProperty(PropertyName = "nombre")]
        public string Nombre { get; set; }

		[JsonProperty(PropertyName = "sexo")]
        public string Sexo { get; set; }

		[JsonProperty(PropertyName = "nacimiento")]
        public string Nacimiento { get; set; }

		[JsonProperty(PropertyName = "numeropatronal")]
        public string NumeroPatronal { get; set; }

		[JsonProperty(PropertyName = "empresa")]
        public string Empresa { get; set; }

		[JsonProperty(PropertyName = "ingreso")]
        public string Ingreso { get; set; }

		[JsonProperty(PropertyName = "estatus")]
        public string Estatus { get; set; }

		[JsonProperty(PropertyName = "afiliacion")]
        public string Afiliacion { get; set; }

		[JsonProperty(PropertyName = "contingencia")]
        public string Contingencia { get; set; }

		[JsonProperty(PropertyName = "semanas")]
        public string Semanas { get; set; }

		[JsonProperty(PropertyName = "salarios")]
        public string Salarios { get; set; }
    }
}