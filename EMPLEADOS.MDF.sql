/****** Object:  Table [dbo].[Empleados]    Script Date: 03/20/2013 22:14:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Empleados]') AND type in (N'U'))
DROP TABLE [dbo].[Empleados]
GO
/****** Object:  Table [dbo].[Empleados]    Script Date: 03/20/2013 22:14:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Empleados]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Empleados](
	[EmpleadoLegajo] [int] IDENTITY(1,1) NOT NULL,
	[EmpleadoNombre] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[EmpleadoApellido] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[EmpleadoTelefono] [varchar](20) COLLATE Modern_Spanish_CI_AS NULL,
	[EmpleadoMail] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[EmpleadoFechaNacimiento] [date] NULL,
	[EmpleadoSueldo] [int] NULL
)
END
GO
