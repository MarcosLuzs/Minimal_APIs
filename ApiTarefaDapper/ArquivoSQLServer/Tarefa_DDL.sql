USE TarefasDemoDB;
GO

CREATE TABLE Tarefas(
	[Id] INT IDENTITY (1,1)NOT NULL,
	[Atividade] NVARCHAR(255)NOT NULL,
	[Status] NVARCHAR(100)NOT NULL
);