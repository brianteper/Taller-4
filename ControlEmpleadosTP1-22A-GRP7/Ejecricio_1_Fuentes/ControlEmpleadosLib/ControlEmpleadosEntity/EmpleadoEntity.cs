using System;

namespace ControlEmpleadosEntity
{
	/// <summary>
	/// Cada instancia de esta clase representa un empleado.
	/// </summary>
	public class EmpleadoEntity
	{
        public int Legajo { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Mail { get; set; }
        public DateTime Nacimiento { get; set; }
        public uint Sueldo { get; set; }

        public bool TieneEmail() {
            return !String.IsNullOrEmpty(this.Mail);
        }

        public bool TieneFechaNacimiento()
        {
            return this.Nacimiento != null;
        }

	}
}
