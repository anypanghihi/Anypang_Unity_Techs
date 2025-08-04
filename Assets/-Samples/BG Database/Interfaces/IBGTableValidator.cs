using BansheeGz.BGDatabase;

public interface IBGTableValidator
{
    BGMetaEntity GetTable(string tableName);
    BGEntity GetEntity(BGMetaEntity table, int entityId);
    BGField GetFieldByComment(BGMetaEntity table, string comment);
}