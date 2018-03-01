---https://docs.microsoft.com/ru-ru/sql/relational-databases/tables/lesson-2-2-populating-a-hierarchical-table-using-hierarchical-methods
--Вставка корневого элемента
INSERT INTO [dbo].[Tariffs]
           ([OrgNode]
           ,[Use]
           ,[Day]
           ,[TimeS]
           ,[TimeF]
           ,[Tariff])
     VALUES
           (hierarchyid::GetRoot()
           ,1
           ,'Все'
           ,'00:00:00'
           ,'23:59:59'
           ,100)
GO

--Добавление дня недели
DECLARE @Week hierarchyid   
SELECT @Week = hierarchyid::GetRoot()  
FROM Tariffs ;  

INSERT INTO [dbo].[Tariffs]
		   ([OrgNode]
           ,[Use]
           ,[Day]) 
VALUES  
(@Week.GetDescendant(NULL, NULL), 0, 'Понедельник') ; 
---процедура для добавления дня недели
CREATE PROC AddDay(@use bit, @day varchar(15))   
AS   
BEGIN  
   DECLARE @dOrgNode hierarchyid, @lc hierarchyid  
   SELECT @dOrgNode = OrgNode   
   FROM Tariffs   
   WHERE Day = 'Все'  
   SET TRANSACTION ISOLATION LEVEL SERIALIZABLE  
   BEGIN TRANSACTION  
      SELECT @lc = max(OrgNode)   
      FROM Tariffs   
      WHERE OrgNode.GetAncestor(1) = @dOrgNode ;  

      INSERT Tariffs (OrgNode, [Use], Day)  
      VALUES(@dOrgNode.GetDescendant(@lc, NULL), @use, @day)  
   COMMIT  
END ;  
GO  
 

----Создание новой процедуры на добавление нового тарифа
{
@day - день для которого должно быть создан новый тарифа
@idtariff - числовой индефикатор нового нарифа
@times - время начала действия
@timef - времмя конца действия 
@cost - цена за час у нового тарифа
}


CREATE PROC AddTariff(@day varchar(15), @idtariff int, @times time(7), @timef time(7), @cost int)   
AS   
BEGIN  
   DECLARE @dOrgNode hierarchyid, @lc hierarchyid  
   SELECT @dOrgNode = OrgNode   
   FROM Tariffs   
   WHERE Day = @day  
   SET TRANSACTION ISOLATION LEVEL SERIALIZABLE  
   BEGIN TRANSACTION  
      SELECT @lc = max(OrgNode)   
      FROM Tariffs   
      WHERE OrgNode.GetAncestor(1) = @dOrgNode ;  

      INSERT Tariffs (OrgNode, idTariff, TimeS, TimeF, Tariff)  
      VALUES(@dOrgNode.GetDescendant(@lc, NULL), @idtariff, @times, @timef, @cost)  
   COMMIT  
END ;  
GO  

--выборка корневого элемента
SELECT  *  
FROM Tariffs 
WHERE OrgNode = hierarchyid::GetRoot() ;  
GO  
--выборка расписания по заданному дню недели
--если выборка по "Все" результат будет дни недели
DECLARE @day hierarchyid  

SELECT @day = OrgNode  
FROM Tariffs  
WHERE Day = 'Понедельник' ;  

SELECT *  
FROM Tariffs  
WHERE OrgNode.GetAncestor(1) = @day   
--исспользование
EXEC AddDayWeek 'True', 'Среда' ; 
--исспользование процедуры
EXEC AddTariff 'Все', 1, '00:00:00', '09:00:00', 50 ;
--использование функции
	SELECT * FROM GetTariffs('Все');
	SELECT * FROM GetRoot();



