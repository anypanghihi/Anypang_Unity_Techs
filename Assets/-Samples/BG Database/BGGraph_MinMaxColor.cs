using BansheeGz.BGDatabase;
using UnityEngine;

[BGCalcUnitDefinition("Custom/Min Max Color")]
public class MinMaxColorUnit : BGCalcUnit
{
    private BGCalcValueInput a;
    private BGCalcValueInput b;
    private BGCalcValueInput c;
    private BGCalcValueInput d;
    private BGCalcValueInput e;

    public override void Definition()
    {
        a = ValueInput<float>("min", "a");
        b = ValueInput<float>("max", "b");
        c = ValueInput<float>("cur", "c");

        d = ValueInput<Color>("min", "d");
        e = ValueInput<Color>("max", "e");

        ValueOutput<Color>("min < cur < max", "f", GetValue);
    }

    private Color GetValue(BGCalcFlowI flow)
    {
        float min = flow.GetValue<float>(a);
        float max = flow.GetValue<float>(b);
        float cur = flow.GetValue<float>(c);

        Color dest = Color.white;
        if( cur < min)
        {
            dest = flow.GetValue<Color>(d);
        }
        else if (cur > max)
        {
            dest = flow.GetValue<Color>(e);
        }

        return dest;
    }
}