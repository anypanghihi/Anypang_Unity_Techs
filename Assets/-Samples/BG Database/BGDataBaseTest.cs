using UnityEngine;
using BansheeGz.BGDatabase;
using System.Collections.Generic;

public class BGDataBaseTest : MonoBehaviour
{
    [SerializeField] private string TableName = "D3_UIField";
    [SerializeField] private string TableDescription;

    private IBGTableValidator tableValidator;
    private IBGValueModifier valueModifier;

    private void Awake()
    {
        // 기본 구현으로 초기화
        tableValidator = new BGTableValidator();
        valueModifier = new BGValueModifier();
    }

    private void Start()
    {
        var get = GetOPDictionary_ByResponseField("D3_MPCValve_AASTable");
    }

    public void ChangeValue_General()
    {
        // var table = BGRepo.I[TableName];
        // var entity = table.GetEntity(2); // "uFbgrGJOpE28KIkMNuU1kg;

        // string fieldValue = entity.Get<string>("var1");

        // Debug.Log(fieldValue);



        // float value = System.Convert.ToSingle(fieldValue) + Random.Range(-0.1f, 0.0f);

        // entity.Set<string>("var1", value.ToString());
        ChangeValue_With_Comment(TableName, 2);
    }

    public void ChangeValue_CodeGen()
    {
        var entity = BG_D3_UIField.GetEntity(2);

        string fieldValue = entity.f_var1;

        Debug.Log(fieldValue);



        float value = System.Convert.ToSingle(fieldValue) + Random.Range(-0.1f, 0.0f);

        entity.f_var1 = value.ToString();
    }

    public Dictionary<string, List<BGEntity>> GetOPDictionary_ByResponseField(string tableName, string opFieldName = "IsOP", string responseFieldName = "response")
    {
        var table = BGRepo.I[tableName];
        var result = new Dictionary<string, List<BGEntity>>();

        table.ForEachEntity(entity =>
        {
            if (entity.Meta.HasField(opFieldName) && entity.Get<bool>(opFieldName))
            {
                var children = entity.Get<List<BGEntity>>(responseFieldName);
                result.Add(entity.Name, children ?? new List<BGEntity>());
            }
        });

        foreach (var each in result)
        {
            Debug.Log(each.Key);
            foreach (var child in each.Value)
            {
                Debug.Log("     " + child.Name);
            }
        }

        return result;
    }

    public void ChangeValue_With_Comment(string tableName, int entityId)
    {
        var table = tableValidator.GetTable(tableName);
        if (table == null) return;

        var entity = tableValidator.GetEntity(table, entityId);
        if (entity == null) return;

        var field = tableValidator.GetFieldByComment(table, "Value");
        if (field == null) return;

        float adjustment = UnityEngine.Random.Range(-0.1f, 0.0f);
        valueModifier.ModifyFieldValue(entity, field.Name, adjustment);
    }
}