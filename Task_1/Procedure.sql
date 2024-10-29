SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE CalculateStatistics
AS
BEGIN
DECLARE @TotalEvenNumber DECIMAL(38, 0);
DECLARE @MedianFractionalNumber DECIMAL(18, 8);

-- Сумма всех чётных целых чисел
SELECT @TotalEvenNumber = SUM(CAST(EvenNumber AS DECIMAL(38, 0))) FROM MainTable;

PRINT 'Total of all even numbers: ' + CAST(@TotalEvenNumber AS VARCHAR);

-- Медиана всех дробных чисел
WITH SortedFractionalNumbers AS
(
    SELECT FractionalNumber,
           ROW_NUMBER() OVER (ORDER BY FractionalNumber ASC) AS RowAsc,
           ROW_NUMBER() OVER (ORDER BY FractionalNumber DESC) AS RowDesc
    FROM MainTable
)
SELECT @MedianFractionalNumber = AVG(FractionalNumber)
FROM SortedFractionalNumbers
WHERE RowAsc = RowDesc
   OR RowAsc + 1 = RowDesc;

PRINT 'Median of all fractional numbers: ' + CAST(@MedianFractionalNumber AS VARCHAR(18));
END
GO
