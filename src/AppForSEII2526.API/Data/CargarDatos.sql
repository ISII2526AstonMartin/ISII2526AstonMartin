

INSERT INTO [dbo].[AspNetUsers] ([Id], [Nombre], [Apellido1], [Apellido2], [NombreUsuario], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'4', N'Rafael', N'Martinez', N'Muñoz', N'rafamartinez', NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, 1, 1, NULL, 1, 1)


-- Habilitar IDENTITY_INSERT para TipoPan
SET IDENTITY_INSERT [dbo].[TipoPan] ON

INSERT INTO [dbo].[TipoPan] ([Id], [Nombre]) VALUES (1, N'Integral')
INSERT INTO [dbo].[TipoPan] ([Id], [Nombre]) VALUES (2, N'Semilla')
INSERT INTO [dbo].[TipoPan] ([Id], [Nombre]) VALUES (3, N'Normal')

-- Deshabilitar IDENTITY_INSERT para TipoPan
SET IDENTITY_INSERT [dbo].[TipoPan] OFF

-- Habilitar IDENTITY_INSERT para Bocadillo
SET IDENTITY_INSERT [dbo].[Bocadillo] ON

INSERT INTO [dbo].[Bocadillo] ([Id], [Nombre], [PVP], [Stock], [TipoPanId], [Tamanyo]) VALUES (1, N'Serrano', 5, 10, 1, 1)
INSERT INTO [dbo].[Bocadillo] ([Id], [Nombre], [PVP], [Stock], [TipoPanId], [Tamanyo]) VALUES (2, N'Bacon', 2, 3, 2, 0)
INSERT INTO [dbo].[Bocadillo] ([Id], [Nombre], [PVP], [Stock], [TipoPanId], [Tamanyo]) VALUES (3, N'BaconQueso', 3, 4, 3, 1)
INSERT INTO [dbo].[Bocadillo] ([Id], [Nombre], [PVP], [Stock], [TipoPanId], [Tamanyo]) VALUES (4, N'Atun', 4, 2, 1, 0)

-- Deshabilitar IDENTITY_INSERT para Bocadillo
SET IDENTITY_INSERT [dbo].[Bocadillo] OFF


-- Habilitar IDENTITY_INSERT para TiposBocadillos
SET IDENTITY_INSERT [dbo].[TiposBocadillos] ON
INSERT INTO [dbo].[TiposBocadillos] ([IdTipo], [NombreTipo]) VALUES (1, N'Vegano')
INSERT INTO [dbo].[TiposBocadillos] ([IdTipo], [NombreTipo]) VALUES (2, N'Vegetariano')
INSERT INTO [dbo].[TiposBocadillos] ([IdTipo], [NombreTipo]) VALUES (3, N'Sin Gluten')
INSERT INTO [dbo].[TiposBocadillos] ([IdTipo], [NombreTipo]) VALUES (4, N'Normal')
-- Deshabilitar IDENTITY_INSERT para TiposBocadillos
SET IDENTITY_INSERT [dbo].[TiposBocadillos] OFF


-- Habilitar IDENTITY_INSERT para BonoBocadillos
SET IDENTITY_INSERT [dbo].[BonoBocadillos] ON
INSERT INTO [dbo].[BonoBocadillos] ([BonoID], [CantidadDisponible], [NBocadillos], [NombreBono], [PVP], [TipoIdTipo]) VALUES (1, 5, 6, N'Bono1', 15, 1)
INSERT INTO [dbo].[BonoBocadillos] ([BonoID], [CantidadDisponible], [NBocadillos], [NombreBono], [PVP], [TipoIdTipo]) VALUES (2, 6, 2, N'Bono2', 11, 2)
INSERT INTO [dbo].[BonoBocadillos] ([BonoID], [CantidadDisponible], [NBocadillos], [NombreBono], [PVP], [TipoIdTipo]) VALUES (3, 3, 6, N'Bono3', 10, 3)
-- Deshabilitar IDENTITY_INSERT para BonoBocadillos
SET IDENTITY_INSERT [dbo].[BonoBocadillos] OFF



-- Habilitar IDENTITY_INSERT para TiposProductos
SET IDENTITY_INSERT [dbo].[TiposProductos] ON
INSERT INTO [dbo].[TiposProductos] ([ProductoID], [Nombre]) VALUES (1, N'Camiseta')
INSERT INTO [dbo].[TiposProductos] ([ProductoID], [Nombre]) VALUES (2, N'Gorra')
INSERT INTO [dbo].[TiposProductos] ([ProductoID], [Nombre]) VALUES (3, N'Boligrafo')
-- Deshabilitar IDENTITY_INSERT para TiposProductos
SET IDENTITY_INSERT [dbo].[TiposProductos] OFF

-- Habilitar IDENTITY_INSERT para Productos
SET IDENTITY_INSERT [dbo].[Productos] ON
INSERT INTO [dbo].[Productos] ([ProductoID], [Nombre], [PVP], [Stock], [Tipo_ProductoProductoID]) VALUES (4, N'Camiseta', 8, 48, 1)
INSERT INTO [dbo].[Productos] ([ProductoID], [Nombre], [PVP], [Stock], [Tipo_ProductoProductoID]) VALUES (5, N'Gorra', 4, 10, 2)
INSERT INTO [dbo].[Productos] ([ProductoID], [Nombre], [PVP], [Stock], [Tipo_ProductoProductoID]) VALUES (6, N'Boligrafo', 2, 48, 3)
-- Deshabilitar IDENTITY_INSERT para Productos
SET IDENTITY_INSERT [dbo].[Productos] OFF