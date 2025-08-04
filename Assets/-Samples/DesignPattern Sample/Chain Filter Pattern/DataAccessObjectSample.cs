using UnityEngine;


// DAO
public interface IDAO<Key,Value>
{
    public void Create(Key key, Value value);
    public void Read(Key key);
    public void Update(Key key, Value value);
    public void Delete(Key key);

    public void DeleteAll();
    public void UpdateAll(Key key);
}


// VO
public class UserVO
{
    public string id;
    public string name;
}

public class BbsVO
{
    public int id;
    public string content;
}


// DAO implemnet
public class UserDAO : IDAO<string, UserVO>
{
    public void Create(string key, UserVO value)
    {
        
    }

    public void Read(string key)
    {

    }

    public void Update(string key, UserVO value)
    {
     
    }

    public void Delete(string key)
    {

    }


    public void UpdateAll(string key)
    {
     
    }

    public void DeleteAll()
    {

    }

}


public class BbsDAO : IDAO<string, BbsVO>
{
    public void Create(string key, BbsVO value)
    {

    }

    public void Read(string key)
    {

    }

    public void Update(string key, BbsVO value)
    {

    }

    public void Delete(string key)
    {

    }


    public void UpdateAll(string key)
    {

    }

    public void DeleteAll()
    {

    }

}


public class DataAccessObjectSample : MonoBehaviour
{
    UserDAO userDAO = new UserDAO();

    private void Awake()
    {
        UserVO newUser = new UserVO();
        newUser.id = "id";
        newUser.name = "test";

        userDAO.Create(newUser.id, newUser);
    }

}
