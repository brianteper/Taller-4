using System;
using System.Runtime.Serialization;

namespace ControlEmpleadosBusiness
{
	public class boException : Exception
	{
		public boException(Exception ex) : base("Se produjo un error en la aplicación.", ex)
		{
		}
	}
}
