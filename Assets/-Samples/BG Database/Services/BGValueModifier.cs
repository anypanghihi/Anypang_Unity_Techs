using UnityEngine;
using System;
using BansheeGz.BGDatabase;
using Cysharp.Threading.Tasks;

public class BGValueModifier : IBGValueModifier
{
    public bool ModifyFieldValue(BGEntity entity, string fieldName, float adjustment)
    {
        try
        {
            string stringValue = entity.Get<string>(fieldName);
            if (!float.TryParse(stringValue, out float currentValue))
            {
                Debug.LogWarning($"'{fieldName}'의 값을 float으로 변환할 수 없습니다: {stringValue}");
                return false;
            }

            float newValue = currentValue + adjustment;
            entity.Set<string>(fieldName, newValue.ToString());

            Debug.Log($"[{entity.Meta.Name}] '{fieldName}'의 값이 {currentValue} → {newValue}로 변경됨");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"필드 '{fieldName}'의 값을 처리하는 중 오류 발생: {ex.Message}");
            return false;
        }
    }
}