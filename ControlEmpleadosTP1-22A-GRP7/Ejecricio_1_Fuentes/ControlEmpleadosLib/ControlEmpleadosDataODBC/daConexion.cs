using System;
using System.Data;
using System.Data.Odbc;
using ControlEmpleadosData;
using System.Configuration;

namespace ControlEmpleadosDataODBC
{
	/// <summary>
	/// Esta clase es solo de uso interno en la librería por este
	/// motivo se antepone la palabra reservada "internal".
	/// </summary>
	internal class daConexion
	{
		/// <summary>
		/// Obtener una conexión abierta a la base de datos utilizando ODBC.
		/// </summary>
		/// <returns>Una conexión abierta a la base de datos.</returns>
		public IDbConnection GetOpenedConnection()
		{
            OdbcConnection connection = new OdbcConnection("DSN=ControlEmpleados");
			connection.Open();

			return connection;
		}
	}
}

