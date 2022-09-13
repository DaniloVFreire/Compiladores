using System;

public class Token
{
	private Tuple<int,int> position;
	private string value;
	private string type;
	private string scope;
	private int dummyPointer;
	public Token(Tuple<int,int> _position, string _value, string _type,
		string _scope, int _dummyPointer)
	{
		this.position = _position;
		this.value = _value;
		this.type = _type;
		this.scope = _scope;
		this.dummyPointer = _dummyPointer;
	}
    public string getScope()
    {
		return this.scope;
    }
	public void setScope(string _scope)
	{
		this.scope = _scope;
	}
}
