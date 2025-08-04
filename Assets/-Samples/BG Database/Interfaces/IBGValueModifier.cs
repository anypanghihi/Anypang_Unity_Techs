using BansheeGz.BGDatabase;
using Cysharp.Threading.Tasks;

public interface IBGValueModifier
{
    bool ModifyFieldValue(BGEntity entity, string fieldName, float adjustment);
}