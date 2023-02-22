CREATE TABLE Sales (
	Id UNIQUEIDENTIFIER NOT NULL,
	ProductId UNIQUEIDENTIFIER NOT NULL REFERENCES Products(Id),
	ManagerId UNIQUEIDENTIFIER NOT NULL REFERENCES Managers(Id),
	Cnt INT NOT NULL DEFAULT 1,
	SaleDt DateTime NOT NULL DEFAULT CURRENT_TIMESTAMP,
	DeleteDt DateTime NULL,
	PRIMARY KEY (Id)
)

Insert Into Sales (Id, ProductId, ManagerId, Cnt, DeleteDt) Values (NEWID(),(SELECT TOP 1 Id from Products), (SELECT TOP 1 Id from Managers), 1, NULL)

SELECT * from Sales
