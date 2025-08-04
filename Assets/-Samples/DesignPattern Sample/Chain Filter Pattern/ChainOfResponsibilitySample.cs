using System;
using UnityEngine;


/// <summary>
/// Chain Of Responsibility 패턴
///
/// 내가 할수 있는 부분만 하고, 다음 객체에게 책임을 떠넘기는 패턴 (체인으로 연결되어 있듯이)
/// 해결 했다, 해결하지 못했다의 결과가 존재
/// 
/// <summary>

public abstract class SupportBaseHandler
{
    protected string name;
    SupportBaseHandler Next;


    public SupportBaseHandler SetNext(SupportBaseHandler next)
    {
        this.Next = next;
        return this.Next;
    }

    public void Support(Trouble trouble) 
    { 
        if( Resolve(trouble) )
        {
            Done(trouble);
        }
        else
        if( Next != null )
        {
            Next.Support(trouble);
        }
        else
        {
            Fail(trouble);
        }
    }

    public abstract bool Resolve(Trouble trouble);
    
    protected void Done(Trouble trouble)
    {
        Debug.Log("Done Trouble " + trouble.Number +" Resolve "+this.name );
    }
    protected void Fail(Trouble trouble)
    {
        Debug.Log("Fail Trouble " + trouble.Number);
    }

}

public class Trouble
{
    private int number;
    public int Number       { get { return number; } }

    public Trouble(int num) { this.number = num;   }
}


// SupportBaseHandler 구현부

public class LimitSupport : SupportBaseHandler
{
    private int minlimit;
    private int maxlimit;
    
    public LimitSupport(string name, int min, int max)
    {   
        base.name = name;   
        minlimit = min;
        maxlimit = max; 
    }

    public override bool Resolve(Trouble trouble)
    {
        int t = trouble.Number;
        bool isBetween = (t >= minlimit && t <= maxlimit);

        return isBetween;
    }
}

public class NoSupport : SupportBaseHandler
{
    public NoSupport(string name)
    {
        base.name=name;
    }

    public override bool Resolve(Trouble trouble)
    {
        return false;
    }
}

public class OddSupport : SupportBaseHandler
{
    public OddSupport(string name)
    {
        base.name = name;
    }

    public override bool Resolve(Trouble trouble)
    {
        if( trouble.Number % 2 == 1 ) 
        {
            return true;
        }

        return false;   
    }
}

public class SpecialSupport : SupportBaseHandler
{
    int specialNumber;

    public SpecialSupport(string name, int number)
    {
        base.name = name;
        specialNumber = number;
    }

    public override bool Resolve(Trouble trouble)
    {
        if (trouble.Number == specialNumber)
        {
            return true;
        }

        return false;
    }
}


public class ChainOfResponsibilitySample : MonoBehaviour
{
    private void Start()
    {
        SupportBaseHandler support1 = new LimitSupport("limit1", 100, 200);
        SupportBaseHandler support2 = new LimitSupport("limit2", 201, 300);
        SupportBaseHandler support3 = new NoSupport("no");
        SupportBaseHandler support4 = new OddSupport("odd");
        SupportBaseHandler support5 = new SpecialSupport("special", 333);

        support1.SetNext(support2).SetNext(support3).SetNext(support4).SetNext(support5);

        for(int i=0; i<350; i++) 
        { 
            support1.Support(new Trouble(i));
        }
    }

}
