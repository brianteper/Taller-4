using System;
using System.Collections.Generic;
using System.Text;
using ControlEmpleadosData;

namespace ControlEmpleadosDataFactory
{
    public class Factory
    {
        private Factory()
        {
        }
        
        public static IFactory GetInstance()
        {
            string motor = System.Configuration.ConfigurationSettings.AppSettings["Motor"];
            System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(motor);
            return (IFactory)assembly.CreateInstance(motor + ".Factory");
        }
    }
}
