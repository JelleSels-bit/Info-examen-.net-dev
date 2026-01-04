

INSERT INTO [dbo].[Klant] ([Id], [Naam], [Voornaam], [Gemeente], [Postcode], [Straat], [Huisnummer], [Bankrekeningnummer]) VALUES ('KIMA', 'Kiekeboe', 'Marcel', 'Geel', '2440', 'Kleinhoefstraat','4','BE12 3456 7890 1234')
INSERT INTO [dbo].[Klant] ([Id], [Naam], [Voornaam], [Gemeente], [Postcode], [Straat], [Huisnummer], [Bankrekeningnummer]) VALUES ('VDNL', 'Van Der Neffe', 'Leon', 'Lier', '2500' , 'Antwerpsestraat', '99' , 'BE23 4567 8901 2345')
INSERT INTO [dbo].[Klant] ([Id], [Naam], [Voornaam], [Gemeente], [Postcode], [Straat], [Huisnummer], [Bankrekeningnummer]) VALUES ('VDKF', 'Van De Kasseien', 'Firmin', 'Turnhout','2300', 'Campus Blairon', '800', 'BE34 5678 9012 3456')

SET IDENTITY_INSERT [dbo].[Job] ON
INSERT INTO [dbo].[Job] ([Id], [Omschrijving], [StartDatum], [EindDatum], [Locatie], [IsWerkschoenen], [IsBadge], [IsKleding],[AantalPlaatsen]) VALUES (1, N'Werken bij de bloemist', N'2022-01-01 00:00:00', N'2022-02-01 00:00:00', N'Straatnaam 15, 2300 Turnhout', 0, 1, 0,1)
INSERT INTO [dbo].[Job] ([Id], [Omschrijving], [StartDatum], [EindDatum], [Locatie], [IsWerkschoenen], [IsBadge], [IsKleding],[AantalPlaatsen]) VALUES (2, N'Werken bij de slager', N'2022-01-01 00:00:00', N'2022-02-01 00:00:00', N'Straatnaam 14, 2300 Turnhout', 0, 1, 1,2)
INSERT INTO [dbo].[Job] ([Id], [Omschrijving], [StartDatum], [EindDatum], [Locatie], [IsWerkschoenen], [IsBadge], [IsKleding],[AantalPlaatsen]) VALUES (3, N'Werken bij de bakker', N'2022-01-01 00:00:00', N'2022-02-01 00:00:00', N'Straatnaam 12, 2300 Turnhout', 0, 1, 1,4)
SET IDENTITY_INSERT [dbo].[Job] OFF

SET IDENTITY_INSERT [dbo].[KlantJob] ON
INSERT INTO [dbo].[KlantJob] ([Id], [KlantId], [JobId]) VALUES (1, N'VDNL', 2)
INSERT INTO [dbo].[KlantJob] ([Id], [KlantId], [JobId]) VALUES (2, N'VDKF', 1)
INSERT INTO [dbo].[KlantJob] ([Id], [KlantId], [JobId]) VALUES (3, N'KIMA', 3)
SET IDENTITY_INSERT [dbo].[KlantJob] OFF