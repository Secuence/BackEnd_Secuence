namespace SecuenceBack.Models
{
	public class Respuesta<T> : RespuestaElement<List<T>>
	{
		public Respuesta()
		{
			this.Data = Activator.CreateInstance<List<T>>();
		}
	}

	public class RespuestaElement<T>
	{
		public int Ok { get; set; }
		public T Data { get; set; }
		public string Message { get; set; } = string.Empty;
		public int id { get; set; } = 0;
	}
}
