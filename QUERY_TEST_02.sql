SELECT
    i.name AS IndexName,
    OBJECT_NAME(i.object_id) AS TableName,
    c.name AS ColumnName,
    i.is_unique
FROM sys.indexes i
INNER JOIN sys.index_columns ic
    ON i.object_id = ic.object_id
   AND i.index_id = ic.index_id
INNER JOIN sys.columns c
    ON ic.object_id = c.object_id
   AND ic.column_id = c.column_id
WHERE OBJECT_NAME(i.object_id) IN
(
    'IdentityUsers',
    'IdentityRoles',
    'IdentityUserRoles'
)
ORDER BY TableName, IndexName;

SELECT
    fk.name AS FK_Name,
    OBJECT_NAME(fk.parent_object_id) AS ChildTable,
    OBJECT_NAME(fk.referenced_object_id) AS ParentTable
FROM sys.foreign_keys fk
WHERE OBJECT_NAME(fk.parent_object_id)
IN ('IdentityUserRoles');