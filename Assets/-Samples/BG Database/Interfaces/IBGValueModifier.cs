using BansheeGz.BGDatabase;

public interface IBGValueModifier
{
    bool ModifyFieldValue(BGEntity entity, string fieldName, float adjustment);
}