using System;
using System.Runtime.Serialization;

namespace ControlEmpleadosBusiness
{
	public class SueldoFueraDeRangoException : Exception
	{
		public SueldoFueraDeRangoException(int sueldoMinimo, int sueldoMaximo) : base("El sueldo debe estar entre $" + sueldoMinimo.ToString() + " y $" + sueldoMaximo.ToString() + ".")
		{
		}
	}
}
