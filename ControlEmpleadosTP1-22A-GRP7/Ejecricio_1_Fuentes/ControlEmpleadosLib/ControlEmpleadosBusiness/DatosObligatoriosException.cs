using System;
using System.Runtime.Serialization;

namespace ControlEmpleadosBusiness
{
	public class DatosObligatoriosException : Exception
	{
		public DatosObligatoriosException() : base("Los campos marcados con (*) son obligatorios.")
		{
		}
	}
}
