create table Myoffice
(
    id      int primary key,
    name    varchar(255),
    address varchar(255),
);

alter PROCEDURE NEWSID @NewId INT OUTPUT
AS
BEGIN
    SELECT @NewId = ISNULL(MAX(id), 0) + 1
    FROM Myoffice;
END;

CREATE TABLE LogTable
(
    LogId         INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(255),
    Data    NVARCHAR(MAX),
);

CREATE PROCEDURE usp_AddLog @Name NVARCHAR(255),
                          @Data NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO LogTable (Name, Data)
    VALUES (@Name, @Data);
END;

alter PROCEDURE CreateMyoffice @Json NVARCHAR(MAX)
AS
BEGIN
    
    create table #TempTable
    (
        id      int,
        name    varchar(255),
        address varchar(255),
    );
    
    -- get new id
    DECLARE @NewId INT;
    EXEC NEWSID @NewId OUTPUT;
    
    INSERT INTO #TempTable
    SELECT @NewId, name, address
    FROM OPENJSON(@Json)
             WITH (
                 id INT,
                 name NVARCHAR(255),
                 address NVARCHAR(255)
                 );
    
    
    
    INSERT INTO Myoffice (id, name, address)
    SELECT id, name, address
    FROM #TempTable;
    
    Exec usp_AddLog 'CreateMyoffice', @Json;
END;

CREATE PROCEDURE GetMyoffice @Json NVARCHAR(MAX) OUTPUT
AS
BEGIN
    SELECT @Json = (SELECT id, name, address
                    FROM Myoffice
                    FOR JSON AUTO);
END;

alter PROCEDURE UpdateMyoffice @Json NVARCHAR(MAX)
AS
BEGIN
    -- check exist
    IF NOT EXISTS (SELECT id
                   FROM Myoffice
                   WHERE id IN (SELECT id FROM OPENJSON(@Json) WITH (id INT)))
    BEGIN
        Exec WriteLog 'UpdateMyoffice-Notfound', @Json;
        RAISERROR('Id not found', 16, 1);
    END;

    UPDATE Myoffice
    SET name    = t.name,
        address = t.address
    FROM Myoffice m
             INNER JOIN OPENJSON(@Json)
                                 WITH (
                                     id INT,
                                     name NVARCHAR(255),
                                     address NVARCHAR(255)
                                     ) t ON m.id = t.id;
    
    Exec usp_AddLog 'UpdateMyoffice', @Json;
END;

alter PROCEDURE DeleteMyoffice @Json NVARCHAR(MAX)
AS
BEGIN
    IF NOT EXISTS (SELECT id
                   FROM Myoffice
                   WHERE id IN (SELECT id FROM OPENJSON(@Json) WITH (id INT)))
        BEGIN
            Exec WriteLog 'DeleteMyoffice-Notfound', @Json;
            RAISERROR('Id not found', 16, 1);
        END;


    DELETE FROM Myoffice WHERE id IN (SELECT id FROM OPENJSON(@Json) WITH (id INT));
    
    Exec usp_AddLog 'DeleteMyoffice', @Json;
END;