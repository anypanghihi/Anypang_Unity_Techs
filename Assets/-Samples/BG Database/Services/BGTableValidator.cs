using UnityEngine;
using BansheeGz.BGDatabase;

public class BGTableValidator : IBGTableValidator
{
    public BGMetaEntity GetTable(string tableName)
    {
        var table = BGRepo.I[tableName];
        if (table == null)
        {
            Debug.LogError($"테이블 '{tableName}'을 찾을 수 없습니다.");
            return null;
        }
        return table;
    }

    public BGEntity GetEntity(BGMetaEntity table, int entityId)
    {
        var entity = table.GetEntity(entityId);
        if (entity == null)
        {
            Debug.LogWarning($"엔티티 ID {entityId}를 테이블 '{table.Name}'에서 찾을 수 없습니다.");
            return null;
        }
        return entity;
    }

    public BGField GetFieldByComment(BGMetaEntity table, string targetComment)
    {
        BGField result = null;
        table.ForEachField(field =>
        {
            if (field.Comment == targetComment)
                result = field;
        });

        if (result == null)
        {
            Debug.LogWarning($"테이블 '{table.Name}'에서 Comment가 '{targetComment}'인 필드를 찾을 수 없습니다.");
        }
        return result;
    }
}